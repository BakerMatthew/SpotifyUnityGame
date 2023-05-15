using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI ArtistNameText;
    public Button OptionOneButton;
    public Button OptionTwoButton;
    public Button OptionThreeButton;
    public Button OptionFourButton;
    public AudioSource PreviewAudioSource;

    private SpotifyLogic Spotify;
    public TextAsset jsonFile;
    private int WinningButton;

    private void Start()
    {
        Spotify = new SpotifyLogic();
        Spotify.SpotifyData = new SpotifyDataState();

        Spotify.SpotifyData.Secrets = JsonUtility.FromJson<SpotifySecretsData>(jsonFile.text);

        ArtistNameText.text = SpotifyDataState.Artist;

        Spotify.PrepareAccessToken();
        var artistTracks = Spotify.GetListOfArtistsTopFourTracks();

        PrepareButtonGame(artistTracks);
    }

    private void PrepareButtonGame(List<Track> artistTracks)
    {
        WinningButton = UnityEngine.Random.Range(0, 3);

        var availableButtons = new List<Button>
        {
            OptionOneButton,
            OptionTwoButton,
            OptionThreeButton,
            OptionFourButton
        };

        for (int i = 0; i < availableButtons.Count; i++)
        {
            availableButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = artistTracks[i].Name;
            if (i == WinningButton)
            {
                availableButtons[i].onClick.AddListener(FoundWinningButton);
                StartCoroutine(LoadAndPlayAudioClip(artistTracks[i].Preview_URL));
            }
        }
    }

    private IEnumerator LoadAndPlayAudioClip(string audioUrl)
    {
        using (var audio = new WWW(audioUrl))
        {
            yield return audio;

            if (audio.error != null)
            {
                throw new Exception($"Failed to load audio clip: {audio.error}");
            }
            else
            {
                PreviewAudioSource.clip = audio.GetAudioClip(false, false, AudioType.MPEG);
                PreviewAudioSource.Play();
            }
        }
    }

    private void FoundWinningButton()
    {
        SceneManager.LoadScene("StartingScene");
    }

    public void ReturnToStartingScene()
    {
        SceneManager.LoadScene("StartingScene");
    }
}
