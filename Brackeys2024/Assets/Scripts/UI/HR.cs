using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HR : MonoBehaviour
{
    public static HR Instance;

    public GameObject audio;
    public AudioSource audioSource;

    private Text text;

    public string Text
    {
        get => text.text;
        set { text.text = value; }
    }

    public bool Show
    {
        get => text.enabled;
        set { text.enabled = value; }
    }

    private void Awake() => Instance = this;

    private void Start()
    {
        audioSource = audio.GetComponent<AudioSource>();
        text = GetComponent<Text>();
        text.enabled = true;
    }

    void Update() {
        //Update Heart Rate
        float Stress = StressManager.Instance.Stress;
        float HR = (int)(Stress * 80f + 60f);
        this.Text = HR.ToString();
        audioSource.pitch = Stress + 1;
    }

    public void PlayHR() {
        audioSource.Play();
    }
    public void StopHR()
    {
        audioSource.Stop();
    }
}
