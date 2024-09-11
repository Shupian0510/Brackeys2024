using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(EventObject))]
public class LampListener : MonoBehaviour
{
    private EventObject @event;
    public GameObject Spotlight;
    public bool isVisible = true;
    private Renderer objectRenderer;
    public float randomInterval;
    private float timer = 0;
    private AudioSource audioSource;
    private bool isPlaying = false;



    private void Start()
    {
        @event = GetComponent<EventObject>();
        objectRenderer = GetComponent<Renderer>();
        audioSource =gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= randomInterval)
        {
            RandomBlink();
            timer = 0f;
        }
    }
    private void RandomBlink() {
        if (@event.IsEventOn)
        {
            if (isPlaying) { 
                audioSource.Play();
                isPlaying = false;
            }   
            bool vis = Random.Range(0, 2) == 0;
            if (vis)
            {
                Spotlight.SetActive(false);
                objectRenderer.material.DisableKeyword("_EMISSION");
            }
            else
            {
                Spotlight.SetActive(true);
                objectRenderer.material.EnableKeyword("_EMISSION");
            }
            
        }
        else
        {
            if (!isPlaying) { 
                audioSource.Stop();
                isPlaying = true;
            }
            Spotlight.SetActive(true);
            objectRenderer.material.EnableKeyword("_EMISSION");
            
        }
    }
}
