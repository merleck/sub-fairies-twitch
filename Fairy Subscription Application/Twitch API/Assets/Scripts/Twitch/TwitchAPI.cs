using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchAPI : MonoBehaviour
{
    public Api api;
    private string channel_name = "PixieLana";

	// Use this for initialization
	void Start ()
    {
        Application.runInBackground = true;
        api = new Api();

        api.Settings.ClientId = Secrets.live_client_id;
        api.Settings.AccessToken = Secrets.live_bot_access_token;
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            api.Invoke( api.Undocumented.GetChattersAsync(channel_name), GetChatterListCallback);
        }
	}


    private void GetChatterListCallback(List<TwitchLib.Api.Models.Undocumented.Chatters.ChatterFormatted> listOfChatters)
    {
        foreach(var chatterObject in listOfChatters)
        {
            Debug.Log(chatterObject.Username);
        }
    }
}
