using UnityEngine;

[System.Serializable]

public class Dialogues
{
    //the audio source of the dialogue 
    [Tooltip("The actual AudioSource that will play")]
    public AudioSource audio;
    
    //when it's time to play wait (t) seconds before playing
    //used to add a little breathing room before the next dialogue
    [Tooltip("The amount of seconds before the audio plays")]
    public float time;

    //the string text of the subtitle
    [Tooltip("The printed subtitle string")]
    public string subtitles;

    [Tooltip("A script to enable. If it's empty nothing will be enabled.")]
    public MonoBehaviour scriptToEnable;
}
