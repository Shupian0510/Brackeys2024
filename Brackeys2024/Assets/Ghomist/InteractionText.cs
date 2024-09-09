using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class InteractionText : MonoBehaviour
{
    public static InteractionText Instance;

    private TMP_Text text;

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
        text = GetComponent<TMP_Text>();
        text.enabled = false;
    }
}
