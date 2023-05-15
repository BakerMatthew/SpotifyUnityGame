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

    public List<Track> GetListOfArtistsTopFourTracks()
    {
        return GetArtistTopTracks(GetArtistId());
    }

    private string GetArtistId()
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", SpotifyData.AccessToken);

            string url = $"https://api.spotify.com/v1/search?q={SpotifyDataState.Artist}&type=artist";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                ArtistsSearchResult spotifyArtists = JsonConvert.DeserializeObject<ArtistsSearchResult>(responseContent);
                return spotifyArtists.Artists.Items[0].Id;
            }
            throw new Exception($"Failed to find any artists with name {SpotifyDataState.Artist}");
        }
    }

    private List<Track> GetArtistTopTracks(string artistId)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", SpotifyData.AccessToken);

            string url = $"https://api.spotify.com/v1/artists/{artistId}/top-tracks?country=US";
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                ArtistTopTracksResponse artistTopTracks = JsonConvert.DeserializeObject<ArtistTopTracksResponse>(responseContent);

                var topTracks = new List<Track>();
                foreach (var track in artistTopTracks.Tracks)
                {
                    if (string.IsNullOrWhiteSpace(track.Preview_URL))
                    {
                        continue;
                    }
                    topTracks.Add(track);
                    if (topTracks.Count == 4)
                    {
                        return topTracks;
                    }
                }
            }
            throw new Exception($"Error retrieving top tracks: {response.StatusCode}");
        }
    }

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
                SpotifyData.AccessToken = spotifyToken.Access_Token;
            }
            else
            {
                throw new Exception("Could not get an access token!");
            }
        }
    }
}

#region Artists JSON Classes
public class ArtistsSearchResult
{
    public ArtistsContainer Artists { get; set; }
}
public class ArtistsContainer
{
    public List<Artist> Items { get; set; }
}

public class Artist
{
    public string Id { get; set; }
}
#endregion

#region Artist Top Tracks JSON Classes
public class ArtistTopTracksResponse
{
    public List<Track> Tracks { get; set; }
}
public class Track
{
    public string Name { get; set; }
    public string Preview_URL { get; set; }
}
#endregion

#region Access Token JSON Classes
public class SpotifyToken
{
    public string Access_Token { get; set; }
}
#endregion
