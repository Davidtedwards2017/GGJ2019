using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController> {

    private float MaxVolume = 1f;
    private float m_MasterSfxVolume = 1f;
    public float MusicVolumeLevelScale = 0.5f;
    private float m_MasterMusicVolume = 1f;

    public float StartingSfxVolume = 1;
    public float StartingMusicVolume = 1;

    public delegate void MusicVolumeChangedEventHandler(float newVol);

    public event MusicVolumeChangedEventHandler OnMusicVolumneChanged;

    public float MasterSfxVolume
    {
        get { return m_MasterSfxVolume; }
        set
        {
            m_MasterSfxVolume = Mathf.Min(Mathf.Max(0, value), MaxVolume);
        }
    }
    
    public float MasterMusicVolume
    {
        get { return m_MasterMusicVolume; }
        set
        {
            m_MasterMusicVolume = Mathf.Min(Mathf.Max(0, value), MaxVolume) * MusicVolumeLevelScale;
            if(OnMusicVolumneChanged != null)
            {
                OnMusicVolumneChanged(m_MasterMusicVolume);
            }
        }
    }

    public void Start()
    {
        MasterMusicVolume = StartingMusicVolume;
        MasterSfxVolume = StartingSfxVolume;
    }
}
