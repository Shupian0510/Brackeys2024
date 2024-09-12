using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySystem : MonoBehaviour
{
    public AudioSource audioSource;
    private Dictionary<string, StoryAudio> audioDict = new Dictionary<string, StoryAudio>();

    void Start()
    {
        LoadAllStoryAudios();  // 在游戏开始时加载所有音频
    }

    private void LoadAllStoryAudios()
    {
        // 加载 Resources 文件夹中的所有 AudioClip
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("");

        foreach (AudioClip clip in audioClips)
        {
            // 以音频文件名为 key，添加到字典中
            string storyFile = clip.name;

            // 创建并初始化一个新的 StoryAudio 对象，默认设置为可重复播放
            StoryAudio audio = new StoryAudio(clip, true);

            // 将 StoryAudio 添加到字典中
            if (!audioDict.ContainsKey(storyFile))
            {
                audioDict.Add(storyFile, audio);
                Debug.Log($"Loaded audio: {storyFile}");
            }
        }
    }

    public void AddStoryAudio(string storyFile, bool isReplayable = true)
    {
        // 检查是否已经存在该 StoryAudio
        if (audioDict.ContainsKey(storyFile))
        {
            Debug.Log($"Audio {storyFile} already exists in the dictionary.");
            return;
        }

        // 加载音频文件
        AudioClip clip = Resources.Load<AudioClip>(storyFile);

        // 创建并初始化一个新的 StoryAudio 对象
        StoryAudio audio = new StoryAudio(clip, isReplayable);

        // 将 StoryAudio 添加到字典中
        audioDict[storyFile] = audio;
    }

    public void PlayStoryAudio(string storyFile)
    {
        if (!audioDict.ContainsKey(storyFile))
        {
            Debug.LogWarning($"Audio {storyFile} not found in the dictionary.");
            return;
        }

        StoryAudio audio = audioDict[storyFile];
        if (!audio.isReplayable && audio.timesPlayed > 0)
        {
            Debug.Log($"Audio {storyFile} is not replayable.");
            return;
        }
        if (audioSource.isPlaying && audioSource.clip == audio.clip)
        {
            Debug.Log($"Audio {storyFile} is already playing.");
            return;
        }

        audioSource.clip = audio.clip;
        audioSource.Play();
        Debug.Log($"Playing audio {storyFile}.");

        audio.timesPlayed++;
    }

    public int GetTimesPlayed(string storyFile)
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

    public void NotReplayable(string storyFile) {
        StoryAudio audio = audioDict[storyFile];
        audio.isReplayable = false;
    }
}

// StoryAudio 
public class StoryAudio
{
    public AudioClip clip;
    public int timesPlayed; // 播放次数
    public bool isReplayable; // 可否重复播放
    public StoryAudio(AudioClip clip, bool isReplayable)
    {
        this.clip = clip;
        this.timesPlayed = 0;
        this.isReplayable = isReplayable;
    }
}
