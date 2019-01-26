using System;
using System.Collections.Generic;
using UnityEngine;
using Utilites;

namespace Utilites
{
    public static class AudioHelper
    {

        private static Dictionary<string, float> RecentlyPlayed = new Dictionary<string, float>();
        private const float MinTimeBetweenSfx = 0.05f;

        public static AudioSource PlaySfx(this SoundEffectData data, bool looping = false, float volume = 1)
        {
            if (data.Clip != null)
            {
                return PlaySfx(data.Clip, data.Volume * volume, looping);
            }

            return null;
        }

        public static AudioSource PlaySfx(this List<SoundEffectData> collection, float volume = 1, bool loop = false)
        {
            var picked = collection.PickRandom();
            return picked.PlaySfx(loop, volume);
        }

        public static AudioSource PlaySfx(AudioClip clip, float volumne, bool loop = false)
        {
            Debug.Log("playing sfx: " + clip.name);
            float current = Time.time;
            if (RecentlyPlayed.ContainsKey(clip.name))
            {
                if (RecentlyPlayed[clip.name] + MinTimeBetweenSfx > current)
                {
                    return null;
                }
            }

            volumne *= AudioController.Instance.MasterSfxVolume;

            GameObject obj = new GameObject(clip.name);

            var audioSource = obj.AddComponent<AudioSource>();

            audioSource.loop = loop;
            audioSource.clip = clip;
            audioSource.volume = volumne;

            audioSource.Play();

            if (!loop)
            {
                GameObject.Destroy(obj, clip.length);
            }

            RecentlyPlayed.AddOrUpdate(clip.name, current);

            return audioSource;

        }
    }

}
