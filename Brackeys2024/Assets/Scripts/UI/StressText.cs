using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class StressText : MonoBehaviour
{
    private TMP_Text text;

    void Start() => text = GetComponent<TMP_Text>();

    void Update()
    {
        if (StressManager.Instance == null)
            return;
        var stress = StressManager.Instance.Stress;
        text.text = $"Stress: {stress}";
    }
}
