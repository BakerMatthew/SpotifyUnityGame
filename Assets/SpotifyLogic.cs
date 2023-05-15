using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class SpotifyLogic : MonoBehaviour
{
    public SpotifyDataState SpotifyData;

    public void PrepareAccessToken()
    {
        using (HttpClient client = new HttpClient())
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", SpotifyData.Secrets.SpotifyClientID),
                new KeyValuePair<string, string>("client_secret", SpotifyData.Secrets.spotifyClientSecret)
            });

            HttpResponseMessage response = client.PostAsync("https://accounts.spotify.com/api/token", formContent).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var spotifyToken = JsonConvert.DeserializeObject<SpotifyToken>(responseContent);
                SpotifyData.AccessToken = spotifyToken.access_token;
            }
            else
            {
                throw new Exception("Could not get an access token!");
            }
        }
        Debug.Log($"Access token: {SpotifyData.AccessToken}");
    }

    private class SpotifyToken
    {
        public string access_token;
    }
}
