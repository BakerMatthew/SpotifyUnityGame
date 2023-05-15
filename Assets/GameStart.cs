using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
	[SerializeField] private string ArtistScene = "ArtistScene";
	public TMP_InputField ArtistInputField;
	
    public void NewGameButton()
	{
		if (string.IsNullOrEmpty(ArtistInputField.text))
		{
			return;
		}
		SpotifyDataState.Artist = ArtistInputField.text;
        SceneManager.LoadScene(ArtistScene);
	}
}
