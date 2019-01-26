using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicController : Singleton<MusicController> {

    public AudioSource CurrentAudioSource;
    public AudioSource OldAudioSource;

    public float FadeOutTime = 1f;
    public float FadeInTime = 1f;

    public void Start()
    {
        AudioController.Instance.OnMusicVolumneChanged += OnMusicVolumeChanged;
    }

    private void OnMusicVolumeChanged(float newVol)
    {
        if(CurrentAudioSource != null)
        {
            CurrentAudioSource.volume = newVol;
        }
    }

    public void Play(AudioClip musicClip)
    {
        OldAudioSource = CurrentAudioSource;
        if(OldAudioSource != null)
        {
            OldAudioSource.DOFade(0, FadeOutTime).OnComplete(() => Destroy(OldAudioSource.gameObject));
        }

        CurrentAudioSource = StartNewMusicSource(musicClip);
    }

    private AudioSource StartNewMusicSource(AudioClip clip)
    {
        GameObject obj = new GameObject(clip.name);
        obj.transform.SetParent(transform);

        var audioSource = obj.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.volume = 0;

        audioSource.Play();

        audioSource.DOFade(AudioController.Instance.MasterMusicVolume, FadeInTime);

        return audioSource;
    }

}
