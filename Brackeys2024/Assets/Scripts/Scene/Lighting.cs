using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class Lighting : MonoBehaviour
{
    public Light LightingFlash;
    private float timer = 0;
    private float waittimer = 0;
    public float BlinkInterval;
    public float BlinkSetsInterval;
    public AudioSource audio;
    public bool flash = false;

    // Start is called before the first frame update
    void Start()
    {
        LightingFlash = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        waittimer += Time.deltaTime;
        if (waittimer > BlinkSetsInterval) 
        {
            waittimer = 0;
            LightingFlash.intensity = 0;
        }
        if (timer >= BlinkInterval && waittimer >= 0.7f * BlinkSetsInterval)
        {
            RandomBlink();
            timer = 0f;
        }
    }

    private void RandomBlink()
    {
        if (flash)
        {
            bool vis = Random.Range(0, 2) == 0;
            if (vis)
            {
                LightingFlash.intensity = Random.Range(0,0.8f);
            }
            else
            {
                LightingFlash.intensity = 0;
            }
        }
        else
        {
            LightingFlash.intensity = 0;
        }
}
}
