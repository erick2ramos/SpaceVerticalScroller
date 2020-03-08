using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace BaseSystems.EventSystem
{
    /// <summary>
    /// Event trigger to reproduce a SFX through (optionally) a mixer group
    /// </summary>
    public struct AudioSFXEvent
    {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;

        static AudioSFXEvent e;

        public static void Trigger(AudioClip clip, AudioMixerGroup group)
        {
            e.Clip = clip;
            e.MixerGroup = group;

            EventManager.Trigger(e);
        }
    }
}