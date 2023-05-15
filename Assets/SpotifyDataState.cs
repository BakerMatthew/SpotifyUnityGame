using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotifyDataState : MonoBehaviour
{
    public SpotifySecretsData Secrets;
    public string AccessToken;

    //Hack to get the Artist passed across scenes
    public static string Artist;
}
