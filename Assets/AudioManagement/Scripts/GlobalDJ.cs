using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class GlobalDJ : MonoBehaviour
{
    [SerializeField] List<AudioClip> Clips;
    [SerializeField] public float EffectsMaxVolume;
    [SerializeField] public float EffectsMinVolume;
    [SerializeField] public float MusicMaxVolume;
    [SerializeField] public float MusicMinVolume;
    [SerializeField] public AudioMixer GlobalMixer;
    [SerializeField] public string EffectsPrefsVariableName;
    [SerializeField] public string MusicPrefsVariableName;
    [SerializeField] public string EffectsExposedVolumeName;
    [SerializeField] public string MusicExposedVolumeName;
    

    public static GlobalDJ Instance;

    AudioSource audioSource;
    float originalVolume;
    int localIndexSong;
    Coroutine downVolumeCoroutine;
    Coroutine upVolumeCoroutine;
    bool finallyPlayEnabled;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad (gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        originalVolume = audioSource.volume;
        CheckAudioSettings();
    }

    public void PlaySong(int indexSong, bool loop)
    {
        localIndexSong = indexSong;
        audioSource.loop = loop;

        float time = 0;
        if(audioSource.isPlaying)
        {
            time = 5;
            downVolumeCoroutine = StartCoroutine(AudioHelper.StartFade(this.audioSource, time, 0f));
        }
        
        finallyPlayEnabled = true;
        Invoke("FinallyPlay", time);
    }

    public void PlaySongNow(int indexSong, bool loop)
    {
        if(downVolumeCoroutine != null)
            StopCoroutine(downVolumeCoroutine);
        if(upVolumeCoroutine != null)
            StopCoroutine(upVolumeCoroutine);
        
        finallyPlayEnabled = false;
        audioSource.clip = Clips[indexSong];
        audioSource.loop = loop;
        audioSource.volume = originalVolume;
        audioSource.Play();
    }

    void FinallyPlay()
    {
        if(!finallyPlayEnabled)
            return;
        audioSource.clip = Clips[localIndexSong];
        audioSource.volume = 0;
        audioSource.Play();
        upVolumeCoroutine = StartCoroutine(AudioHelper.StartFade(this.audioSource, 5, originalVolume));
    }

    void CheckAudioSettings()
    {
        int sound = PlayerPrefs.GetInt(EffectsPrefsVariableName, 1);
        float aux = (sound == 0) ? EffectsMinVolume : EffectsMaxVolume;
        GlobalMixer.SetFloat(EffectsExposedVolumeName, aux);

        int music = PlayerPrefs.GetInt(MusicPrefsVariableName, 1);
        float aux2 = (music == 0) ? MusicMinVolume : MusicMaxVolume;
        GlobalMixer.SetFloat(MusicExposedVolumeName, aux2);
    }

    public bool IsBusy()
    {
        return audioSource.isPlaying;
    }

    public void StopMusic()
    {
        StartCoroutine(AudioHelper.StartFade(this.audioSource, 5f, 0f));
    }

    void SwitchVolume(string variableName, float maxVolume, float minVolume, bool value)
    {
        if(value)
            GlobalMixer.SetFloat(variableName, maxVolume);
        else
            GlobalMixer.SetFloat(variableName, minVolume);
    }

    public void SwitchChannelVolume(string prefVariableName, bool enabled)
    {
        if(prefVariableName == MusicPrefsVariableName)
            SwitchVolume(MusicExposedVolumeName, MusicMaxVolume, MusicMinVolume, enabled);
        else if(prefVariableName == EffectsPrefsVariableName)
            SwitchVolume(EffectsExposedVolumeName, EffectsMaxVolume, EffectsMinVolume, enabled);
    }
}
