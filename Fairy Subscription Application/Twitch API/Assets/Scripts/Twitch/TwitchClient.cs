using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using UnityEngine.UI;

public class TwitchClient : MonoBehaviour
{
    public Client client;

    public GameObject fairy1;
    public GameObject fairy2;
    public GameObject fairy3;
    public GameObject fairyStreamer;
    public GameObject fairyMod;

    public Color streamerColor;

    private string channel_name = "merleck";
	// Use this for initialization
	void Start ()
    { 
        //run this script always while game is running
        Application.runInBackground = true;
       
        //set up bot and tell it the channel to join
        ConnectionCredentials credentials = new ConnectionCredentials("botmerleck", Secrets.bot_access_token);
        client = new Client();

        client.Initialize(credentials, channel_name);

        //Events bot will listen for
        client.OnMessageReceived += MyMessageReceivedFunction;
        //connect bot to channel
        client.Connect();
	}
	
	// Update is called once per frame
	private void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("bot sent message");
            client.SendMessage(channel_name, "This is a test message from the bot");
        }
	}

    private void MyMessageReceivedFunction(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        string fairyCommand = "!fairy ";

        if (e.ChatMessage.IsBroadcaster && e.ChatMessage.Message.StartsWith(fairyCommand))
        {
            Debug.Log("spawn fairy broadcaster");
            //Special fairy for streamer
            SpawnFairy("streamer", 666, e.ChatMessage.Username, true, false);
        }
        else if (e.ChatMessage.IsModerator && e.ChatMessage.Message.StartsWith(fairyCommand))
        {
            Debug.Log("spawn fairy Moderator");
            //Special fairy for streamer
            SpawnFairy(e.ChatMessage.Message.Substring(fairyCommand.Length), 333, e.ChatMessage.Username, false, true);
        }
        else if (e.ChatMessage.IsSubscriber && e.ChatMessage.Message.StartsWith(fairyCommand))
        {
            Debug.Log("spawn fairy subscriber");
            //spawn fairy based on color specified and subscription length of user
            SpawnFairy(e.ChatMessage.Message.Substring(fairyCommand.Length), e.ChatMessage.SubscribedMonthCount, e.ChatMessage.Username, false, false);
        }

        Debug.Log(e.ChatMessage.Username + ":" + e.ChatMessage.Message + ", months subscribed: " + e.ChatMessage.SubscribedMonthCount);
    }

    private void SpawnFairy(string colour, int sublength, string name, bool streamer, bool mod)
    {
        //default white colour
        Vector4 vecColour = new Vector3(0.9607f, 0.9607f, 0.9607f);
        switch (colour)
        {
            case "red":
                vecColour = Color.red;
                break;
            case "blue":
                vecColour = Color.blue;
                break;
            case "green":
                vecColour = Color.green;
                break;
            case "lime":
                vecColour = new Color(0.5490196f, 0.9882353f, 0.1921569f);
                break;
            case "streamer":
                vecColour = streamerColor;
                break;
            default:
                break;
        }
        
        switch (sublength)
        {
            //first 3 months tier 1
            case 0:
            case 1:
            case 2:
                {
                    GameObject temp = Instantiate(fairy1, new Vector3(0, 5, 0), Quaternion.identity, null);
                    ParticleSystem[] psystems = temp.GetComponentsInChildren<ParticleSystem>();
                    Text pName = temp.GetComponentInChildren<Text>();
                    pName.text = name;
                    foreach (ParticleSystem p in psystems)
                    {
                        var main = p.main;
                        main.startColor = new Color(vecColour.x, vecColour.y, vecColour.z, main.startColor.color.a);

                        //If editing the dust particle also change the color over lifetime
                        if (p.gameObject.name == "Dust")
                        {
                            Gradient gradient = new Gradient();
                            GradientColorKey[] gck = new GradientColorKey[2];
                            GradientAlphaKey[] gak = new GradientAlphaKey[2];

                            gck[0].color = vecColour;
                            gck[0].time = 0.0f;
                            gck[1].color = vecColour;
                            gck[1].time = 1.0f;

                            gak[0].alpha = 1.0f;
                            gak[0].time = 0.0f;
                            gak[1].alpha = 0.0f;
                            gak[1].time = 1.0f;

                            gradient.SetKeys(gck, gak);

                            var particleGradient = p.colorOverLifetime;
                            particleGradient.color = gradient;
                        }
                    }
                }
                break;

            //months 3-6 tier 2
            case 3:
            case 4:
            case 5:
                {
                    GameObject temp = Instantiate(fairy2, new Vector3(0, 5, 0), Quaternion.identity, null);
                    ParticleSystem[] psystems = temp.GetComponentsInChildren<ParticleSystem>();
                    Text pName = temp.GetComponentInChildren<Text>();
                    pName.text = name;
                    foreach (ParticleSystem p in psystems)
                    {
                        var main = p.main;
                        main.startColor = new Color(vecColour.x, vecColour.y, vecColour.z, main.startColor.color.a);

                        //If editing the dust particle also change the color over lifetime
                        if (p.gameObject.name == "Dust")
                        {
                            Gradient gradient = new Gradient();
                            GradientColorKey[] gck = new GradientColorKey[2];
                            GradientAlphaKey[] gak = new GradientAlphaKey[2];

                            gck[0].color = vecColour;
                            gck[0].time = 0.0f;
                            gck[1].color = vecColour;
                            gck[1].time = 1.0f;

                            gak[0].alpha = 1.0f;
                            gak[0].time = 0.0f;
                            gak[1].alpha = 0.0f;
                            gak[1].time = 1.0f;

                            gradient.SetKeys(gck, gak);

                            var particleGradient = p.colorOverLifetime;
                            particleGradient.color = gradient;
                        }
                    }
                }
                break;
            //mods
            case 333:
                {
                    GameObject temp = Instantiate(fairyMod, new Vector3(0, 5, 0), Quaternion.identity, null);
                    ParticleSystem[] psystems = temp.GetComponentsInChildren<ParticleSystem>();
                    Text pName = temp.GetComponentInChildren<Text>();
                    pName.text = name;
                    foreach (ParticleSystem p in psystems)
                    {
                        var main = p.main;
                        main.startColor = new Color(vecColour.x, vecColour.y, vecColour.z, main.startColor.color.a);

                        //If editing the dust particle also change the color over lifetime
                        if (p.gameObject.name == "Dust")
                        {
                            Gradient gradient = new Gradient();
                            GradientColorKey[] gck = new GradientColorKey[2];
                            GradientAlphaKey[] gak = new GradientAlphaKey[2];

                            gck[0].color = vecColour;
                            gck[0].time = 0.0f;
                            gck[1].color = vecColour;
                            gck[1].time = 1.0f;

                            gak[0].alpha = 1.0f;
                            gak[0].time = 0.0f;
                            gak[1].alpha = 0.0f;
                            gak[1].time = 1.0f;

                            gradient.SetKeys(gck, gak);

                            var particleGradient = p.colorOverLifetime;
                            particleGradient.color = gradient;
                        }
                    }
                }
                break;
            //streamer
            case 666:
                {
                    GameObject temp = Instantiate(fairyStreamer, new Vector3(0, 5, 0), Quaternion.identity, null);
                    ParticleSystem[] psystems = temp.GetComponentsInChildren<ParticleSystem>();
                    Text pName = temp.GetComponentInChildren<Text>();
                    pName.text = name;
                    foreach (ParticleSystem p in psystems)
                    {
                        var main = p.main;
                        main.startColor = new Color(vecColour.x, vecColour.y, vecColour.z, main.startColor.color.a);

                        //If editing the dust particle also change the color over lifetime
                        if (p.gameObject.name == "Dust")
                        {
                            Gradient gradient = new Gradient();
                            GradientColorKey[] gck = new GradientColorKey[2];
                            GradientAlphaKey[] gak = new GradientAlphaKey[2];

                            gck[0].color = vecColour;
                            gck[0].time = 0.0f;
                            gck[1].color = vecColour;
                            gck[1].time = 1.0f;

                            gak[0].alpha = 1.0f;
                            gak[0].time = 0.0f;
                            gak[1].alpha = 0.0f;
                            gak[1].time = 1.0f;

                            gradient.SetKeys(gck, gak);

                            var particleGradient = p.colorOverLifetime;
                            particleGradient.color = gradient;
                        }
                    }
                }
                break;
            //month 6+ tier 3
            default:
                {
                    GameObject temp = Instantiate(fairy3, new Vector3(0, 5, 0), Quaternion.identity, null);
                    ParticleSystem[] psystems = temp.GetComponentsInChildren<ParticleSystem>();
                    Text pName = temp.GetComponentInChildren<Text>();
                    pName.text = name;
                    foreach (ParticleSystem p in psystems)
                    {
                        var main = p.main;
                        main.startColor = new Color(vecColour.x, vecColour.y, vecColour.z, main.startColor.color.a);

                        //If editing the dust particle also change the color over lifetime
                        if (p.gameObject.name == "Dust")
                        {
                            Gradient gradient = new Gradient();
                            GradientColorKey[] gck = new GradientColorKey[2];
                            GradientAlphaKey[] gak = new GradientAlphaKey[2];

                            gck[0].color = vecColour;
                            gck[0].time = 0.0f;
                            gck[1].color = vecColour;
                            gck[1].time = 1.0f;

                            gak[0].alpha = 1.0f;
                            gak[0].time = 0.0f;
                            gak[1].alpha = 0.0f;
                            gak[1].time = 1.0f;

                            gradient.SetKeys(gck, gak);

                            var particleGradient = p.colorOverLifetime;
                            particleGradient.color = gradient;
                        }
                    }
                }
                break;
        }

    }
}
