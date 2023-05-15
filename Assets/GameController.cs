using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI ArtistNameText;

    private void Start()
    {
        ArtistNameText.text = SpotifyDataState.Artist;
    }

    public void ReturnToStartingScene()
    {
        SceneManager.LoadScene("StartingScene");
    }
}
