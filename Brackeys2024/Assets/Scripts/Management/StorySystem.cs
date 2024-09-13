using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySystem : MonoBehaviour
{
    private static StorySystem Instance;

    public AudioSource AudioSource;

    private static readonly Dictionary<string, StoryAudio> audioDict = new();

    private void Awake() => Instance = this;

    private void Start() => LoadAllStoryAudios();

    // 在游戏开始时加载所有音频
    private void LoadAllStoryAudios()
    {
        // 加载 Resources 文件夹中的所有 AudioClip
        var audioClips = Resources.LoadAll<AudioClip>("");

        foreach (AudioClip clip in audioClips)
        {
            // 以音频文件名为 key，添加到字典中
            var storyFile = clip.name;

            // 创建并初始化一个新的 StoryAudio 对象，默认设置为可重复播放
            var audio = new StoryAudio(clip, true);

            // 将 StoryAudio 添加到字典中
            if (!audioDict.ContainsKey(storyFile))
            {
                audioDict.Add(storyFile, audio);
                Debug.Log($"Loaded audio: {storyFile}");
            }
        }
    }

    public static void AddStoryAudio(string storyFile, bool isReplayable = false)
    {
        // 检查是否已经存在该 StoryAudio
        if (audioDict.ContainsKey(storyFile))
        {
            Debug.LogWarning($"Audio {storyFile} already exists in the dictionary.");
            return;
        }

        // 加载音频文件
        var clip = Resources.Load<AudioClip>(storyFile);

        // 创建并初始化一个新的 StoryAudio 对象
        var audio = new StoryAudio(clip, isReplayable);

        // 将 StoryAudio 添加到字典中
        audioDict[storyFile] = audio;
    }

    public static void PlayStoryAudio(string storyFile)
    {
        if (Instance == null)
        {
            Debug.LogWarning("No story system component found. No audio will be played.");
            return;
        }
        if (!audioDict.ContainsKey(storyFile))
        {
            Debug.LogWarning($"Audio {storyFile} not found in the dictionary.");
            return;
        }

        StoryAudio audio = audioDict[storyFile];
        if (!audio.isReplayable && audio.timesPlayed > 0)
        {
            Debug.LogWarning($"Audio {storyFile} is not replayable.");
            return;
        }
        if (Instance.AudioSource.isPlaying && Instance.AudioSource.clip == audio.clip)
        {
            Debug.LogWarning($"Audio {storyFile} is already playing.");
            return;
        }

        Instance.AudioSource.clip = audio.clip;
        Instance.AudioSource.Play();
        Debug.Log($"Playing audio {storyFile}.");

        audio.timesPlayed++;
    }

    public static int GetTimesPlayed(string storyFile)
    {
        //  StoryAudio
        if (!audioDict.ContainsKey(storyFile))
        {
            Debug.LogWarning($"Audio {storyFile} not found in the dictionary.");
            return 0;
        }

        StoryAudio audio = audioDict[storyFile];
        return audio.timesPlayed;
    }

    public static void NotReplayable(string storyFile)
    {
        if (!audioDict.ContainsKey(storyFile))
        {
            Debug.LogWarning($"Audio {storyFile} not found in the dictionary.");
            return;
        }
        StoryAudio audio = audioDict[storyFile];
        audio.isReplayable = false;
    }
}

/// <summary>
/// StoryAudio
/// </summary>
public class StoryAudio
{
    public AudioClip clip;
    public int timesPlayed; // 播放次数
    public bool isReplayable; // 可否重复播放

    public StoryAudio(AudioClip clip, bool isReplayable)
    {
        this.clip = clip;
        this.isReplayable = isReplayable;
        timesPlayed = 0;
    }
}

