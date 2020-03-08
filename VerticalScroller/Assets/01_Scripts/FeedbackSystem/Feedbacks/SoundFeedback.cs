using UnityEngine;
using System.Collections;
using BaseSystems.Audio;
using UnityEngine.Audio;
using BaseSystems.EventSystem;

namespace BaseSystems.Feedback {

    /// <summary>
    /// Triggers an Audio event
    /// </summary>
    public class SoundFeedback : Feedback
    {
        public AudioClip Clip;
        public AudioMixerGroup Mixer;

        protected override void CustomPlay()
        {
            AudioSFXEvent.Trigger(Clip, Mixer);
        }
    }
}