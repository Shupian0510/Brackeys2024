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

    private string stress25 = "stress25";
    private string stress50 = "stress50";
    private string stress75 = "stress75";
    private string stress90 = "stress90";

    private int HR;

    /// <summary>
    /// Increasing stress per second
    /// </summary>
    public float IncreasingRate = 0.01f;
    public float DecreasingRate = 0.01f;
    public float IncreasePow = 1.5f;

    private void Awake() => Instance = this;

    private void Update()
    {
        //Play Stress related audio
        var eventCount = EventManager.Instance.EventCount;
        if (eventCount > 0)
            Stress += Time.deltaTime * IncreasingRate * Mathf.Pow(eventCount, IncreasePow);
        else if (Stress > 0)
            Stress -= Time.deltaTime * DecreasingRate;

        if (Stress > 0.25 && Stress < 0.5) {
            StorySystem.NotReplayable(stress25);
            StorySystem.PlayStoryAudio(stress25);
        }else if (Stress > 0.5 && Stress < 0.75)
        {
            StorySystem.NotReplayable(stress50);
            StorySystem.PlayStoryAudio(stress50);
        }else if (Stress > 0.75 && Stress < 0.9)
        {
            StorySystem.NotReplayable(stress75);
            StorySystem.PlayStoryAudio(stress75);
        }else if (Stress > 0.9 && Stress < 1)
        {
            StorySystem.NotReplayable(stress90);
            StorySystem.PlayStoryAudio(stress90);
        }

        if (Stress > StressLimit)
        {
            OnStressedOut?.Invoke();
        }
    }

    public void CalmDown(float calmDown) => Stress -= calmDown;
}
