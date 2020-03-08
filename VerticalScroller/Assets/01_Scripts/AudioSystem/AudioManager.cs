using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using BaseSystems.EventSystem;
using UnityEngine.Audio;

namespace BaseSystems.Audio
{
    public class AudioManager : Manager, IEventListener<AudioSFXEvent>
    {
        [SerializeField]
        AudioSource Source;

        public override void Initialize()
        {
            base.Initialize();
        }

        private void OnEnable()
        {
            this.EventStartListening<AudioSFXEvent>();
        }

        private void OnDisable()
        {
            this.EventStopListening<AudioSFXEvent>();
        }

        public void OnEvent(AudioSFXEvent eventType)
        {
            Source.clip = eventType.Clip;
            Source.outputAudioMixerGroup = eventType.MixerGroup;
            Source.PlayOneShot(Source.clip);
        }
    }
}