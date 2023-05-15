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

    private SpotifyLogic Spotify;
    public TextAsset jsonFile;

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

    }

    public void ReturnToStartingScene()
    {
        SceneManager.LoadScene("StartingScene");
    }
}
