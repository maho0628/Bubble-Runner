using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MultiAudio : MonoBehaviour
{
    public AudioClip[] audioClipsBGM; // Array for multiple BGM clips
    public AudioClip[] audioClipsSE;  // Array for multiple SE clips
    public AudioClip[] audioClipsUI;  // Array for multiple UI clips

    public AudioSource bgmSource;
    public AudioSource seSource;
    public AudioSource uiSource; // New UI AudioSource

    // Audio Mixer Groups to assign different mixer settings
    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup seMixerGroup;
    public AudioMixerGroup uiMixerGroup;

    private Dictionary<string, AudioClip> sEClipDictionary;
    private Dictionary<string, AudioClip> BGMClipDictionary;
    private Dictionary<string, AudioClip> UIClipDictionary; // Dictionary for UI clips

    //private SoundCoolTime seCoolTime;
    // Singleton
    public static MultiAudio ins;

    private void Awake()
    {
        // Singleton pattern
        if (ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        bgmSource = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        seSource = GameObject.FindWithTag("SE").GetComponent<AudioSource>();
        uiSource = GameObject.FindWithTag("UI").GetComponent<AudioSource>(); // New UI AudioSource

        // Assign mixer groups to the audio sources
        if (bgmSource != null) bgmSource.outputAudioMixerGroup = bgmMixerGroup;
        if (seSource != null)
        {
            seSource.outputAudioMixerGroup = seMixerGroup;
            //seCoolTime = seSource.GetComponent<SoundCoolTime>();
        }
        if (uiSource != null) uiSource.outputAudioMixerGroup = uiMixerGroup; // Assign UI Mixer Group

        // Initialize dictionaries for easy access by name
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        // SE clips
        sEClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClipsSE)
        {
            sEClipDictionary[clip.name] = clip;
        }

        // BGM clips
        BGMClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClipsBGM)
        {
            BGMClipDictionary[clip.name] = clip;
        }

        // UI clips
        UIClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClipsUI)
        {
            UIClipDictionary[clip.name] = clip;
        }
    }

    public void PlayBGM_ByName(string bgmName)
    {
#if DEBUG
        if (BGMClipDictionary == null)
        {
            Debug.LogWarning("BGMClipDictionary is not initialized.");
            return;
        }
#endif
        if (BGMClipDictionary.TryGetValue(bgmName, out var clip))
        {
            PlayBGM(clip);
            //Debug.Log($"Playing BGM: {bgmName}");
        }
        else
        {
            Debug.LogWarning("BGM with name not found: " + bgmName);
        }
    }

    public void PlaySEByName(string name)
    {
        if (sEClipDictionary.TryGetValue(name, out var clip))
        {
            PlaySE(clip);
        }
        else
        {
            Debug.LogWarning("SE with name not found: " + name);
        }
    }

    public void PlayUIByName(string name)
    {
        if (UIClipDictionary.TryGetValue(name, out var clip))
        {
            PlayUI(clip);
        }
        else
        {
            Debug.LogWarning("UI with name not found: " + name);
        }
    }

    private void PlaySE(AudioClip clip)
    {
        //if (!seCoolTime.canPlay)
        //{
        //    return;
        //}
        if (clip != null)
        {
            seSource.clip = clip;
            seSource.PlayOneShot(seSource.clip);
            //seCoolTime.canPlay = false;
            //Debug.Log("Playing SE: " + clip.name);
        }
        else
        {
            Debug.LogWarning("SE clip is null");
        }
    }

    private void PlayUI(AudioClip clip)
    {
        if (clip != null)
        {
            uiSource.clip = clip;
            uiSource.PlayOneShot(uiSource.clip);
            Debug.Log("Playing UI: " + clip.name);
        }
        else
        {
            Debug.LogWarning("UI clip is null");
        }
    }

    private void PlayBGM(AudioClip clip)
    {
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true; // Loop playback
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM clip is null");
        }
    }

    // Optional: Play SE by index
    public void ChooseSongs_SE(int index)
    {
        if (index >= 0 && index < audioClipsSE.Length)
        {
            PlaySE(audioClipsSE[index]);
        }
        else
        {
            Debug.LogWarning("SE index out of range");
        }
    }

    // Optional: Play UI sound by index
    public void ChooseSongs_UI(int index)
    {
        if (index >= 0 && index < audioClipsUI.Length)
        {
            PlayUI(audioClipsUI[index]);
        }
        else
        {
            Debug.LogWarning("UI index out of range");
        }
    }
}
