using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressSlider : MonoBehaviour
{
    public static ProgressSlider Instance;

    private Slider slider;

    public bool Show
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public float Value
    {
        get => slider.value;
        set { slider.value = value; }
    }

    private void Awake() => Instance = this;

    private void Start()
    {
        slider = GetComponent<Slider>();
        gameObject.SetActive(false);
    }
}
