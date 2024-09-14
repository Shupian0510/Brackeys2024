using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySystem : MonoBehaviour
{
    private static StorySystem Instance;

    public AudioSource AudioSource;

    private static readonly Dictionary<string, StoryAudio> audioDict = new();
    private Queue<StoryAudio> audioQueue = new Queue<StoryAudio>(); // FIFO 队列
    private bool isPlaying = false; // 当前是否正在播放音频

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
        //加载之后再进入
        StoryFlowControl.startvoice();
    }

    public static void AddStoryAudio(string storyFile, bool isReplayable = false)
    {
        // 检查是否已经存在该 StoryAudio
        if (audioDict.ContainsKey(storyFile))
        {
            //Debug.LogWarning($"Audio {storyFile} already exists in the dictionary.");
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

        // 将音频加入队列
        Instance.audioQueue.Enqueue(audio);
        Debug.Log($"Audio {storyFile} added to queue.");

        // 如果当前没有音频在播放，开始播放队列中的音频
        if (!Instance.isPlaying)
        {
            Instance.PlayNextInQueue();
        }
    }

    private void PlayNextInQueue()
    {
        if (audioQueue.Count > 0)
        {
            // 从队列中取出下一个音频进行播放
            StoryAudio audioToPlay = audioQueue.Peek();
            Instance.AudioSource.clip = audioToPlay.clip;

            if (audioToPlay.isReplayable)
            {
                Instance.AudioSource.Play();
            }
            else if (!audioToPlay.isReplayable && audioToPlay.timesPlayed < 1) {
                Instance.AudioSource.Play();
            }
            
            isPlaying = true;

            Debug.Log($"Playing audio {audioToPlay.clip.name}.");

            // 设置协程等待音频播放完毕
            StartCoroutine(WaitForAudioToEnd(audioToPlay));
            
        }
        else
        {
            // 队列为空，播放结束
            isPlaying = false;
            Debug.Log("No more audio in the queue.");
        }
    }

    private IEnumerator WaitForAudioToEnd(StoryAudio currentAudio)
    {
        // 等待音频播放结束
        yield return new WaitWhile(() => AudioSource.isPlaying);

        // 音频播放结束
        currentAudio.timesPlayed++;
        isPlaying = false;

        audioQueue.Dequeue();
        // 播放队列中的下一个音频
        PlayNextInQueue();
    }

    public static int GetTimesPlayed(string storyFile)
    {
        // 获取 StoryAudio 的播放次数
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
    public static bool IsPlaying() {
        return Instance.AudioSource.isPlaying;
    }
    public static bool HasAudioInQueue()
    {
        // 检查队列中是否还有音频
        return Instance.audioQueue.Count > 0;
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
