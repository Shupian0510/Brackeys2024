using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StressManager : MonoBehaviour
{
    public static StressManager Instance;
    public static event UnityAction OnStressedOut;

    public float Stress = 0;
    public float StressLimit = 1;

    /// <summary>
    /// Increasing stress per second
    /// </summary>
    public float IncreasingRate = 0.01f;
    public float DecreasingRate = 0.01f;
    public float IncreasePow = 1.5f;

    private void Awake() => Instance = this;

    private void Update()
    {
        var eventCount = EventManager.Instance.EventCount;
        if (eventCount > 0)
            Stress += Time.deltaTime * IncreasingRate * Mathf.Pow(eventCount, IncreasePow);
        else if (Stress > 0)
            Stress -= Time.deltaTime * DecreasingRate;

        if (Stress > StressLimit)
        {
            OnStressedOut?.Invoke();
        }
    }

    public void CalmDown(float calmDown) => Stress -= calmDown;
}
