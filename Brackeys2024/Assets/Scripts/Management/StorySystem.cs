using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySystem : MonoBehaviour
{
    public AudioSource audioSource;
    private Dictionary<string, StoryAudio> audioDict = new Dictionary<string, StoryAudio>();

    void Start()
    {
        LoadAllStoryAudios();  // ����Ϸ��ʼʱ����������Ƶ
    }

    private void LoadAllStoryAudios()
    {
        // ���� Resources �ļ����е����� AudioClip
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("");

        foreach (AudioClip clip in audioClips)
        {
            // ����Ƶ�ļ���Ϊ key����ӵ��ֵ���
            string storyFile = clip.name;

            // ��������ʼ��һ���µ� StoryAudio ����Ĭ������Ϊ���ظ�����
            StoryAudio audio = new StoryAudio(clip, true);

            // �� StoryAudio ��ӵ��ֵ���
            if (!audioDict.ContainsKey(storyFile))
            {
                audioDict.Add(storyFile, audio);
                Debug.Log($"Loaded audio: {storyFile}");
            }
        }
    }

    public void AddStoryAudio(string storyFile, bool isReplayable = true)
    {
        // ����Ƿ��Ѿ����ڸ� StoryAudio
        if (audioDict.ContainsKey(storyFile))
        {
            Debug.Log($"Audio {storyFile} already exists in the dictionary.");
            return;
        }

        // ������Ƶ�ļ�
        AudioClip clip = Resources.Load<AudioClip>(storyFile);

        // ��������ʼ��һ���µ� StoryAudio ����
        StoryAudio audio = new StoryAudio(clip, isReplayable);

        // �� StoryAudio ��ӵ��ֵ���
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
    public int timesPlayed; // ���Ŵ���
    public bool isReplayable; // �ɷ��ظ�����
    public StoryAudio(AudioClip clip, bool isReplayable)
    {
        this.clip = clip;
        this.timesPlayed = 0;
        this.isReplayable = isReplayable;
    }
}
