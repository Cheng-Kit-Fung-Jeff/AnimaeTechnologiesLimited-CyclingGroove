using System.Collections.Generic;
using UnityEngine;

public class CKF_MusicManager : MonoBehaviour
{
    public static CKF_MusicManager instance;
    public List<AudioSourceProfile> audioSources = new();
    [System.Serializable]
    public struct AudioSourceProfile
    {
        public string key;
        public AudioSource audioSource;
    }
    
    private readonly static Dictionary<string, AudioSource> audioSourcesMap= new();
    
    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
        instance = this;
        foreach (var p in audioSources)
            if(!audioSourcesMap.ContainsKey(p.key))
                audioSourcesMap.Add(p.key, p.audioSource);
        
    }

    public static void Play(string key)
    {
        if (!audioSourcesMap.ContainsKey(key)) return;
        if (audioSourcesMap[key].isPlaying) return;
        if (!audioSourcesMap[key].isPlaying && audioSourcesMap[key].time > 0 && audioSourcesMap[key].time < audioSourcesMap[key].clip.length)
            audioSourcesMap[key].UnPause();
        else
            audioSourcesMap[key].Play();
    }

    public static void ForcePlay(string key)
    {
        if (!audioSourcesMap.ContainsKey(key)) return;
        audioSourcesMap[key].Play();
    }

    public static void Pause(string key)
    {
        if (!audioSourcesMap.ContainsKey(key)) return;
        audioSourcesMap[key].Pause();
    }
    public static void UnPause(string key)
    {
        if (!audioSourcesMap.ContainsKey(key)) return;
        audioSourcesMap[key].UnPause();
    }
}
