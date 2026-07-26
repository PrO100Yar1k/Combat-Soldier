using UnityEngine;

namespace Assets.App.Scripts.Core.Audio
{
    [RequireComponent(typeof(AudioSource))]

    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _soundEffectsSource = default;
        [SerializeField] private AudioSource _backgroundSoundtrackSource = default;

        [SerializeField] private AudioClip _backgroundSoundtrack = default;

        public void PlayBackgroundSoundtrack(float volume = 0.5f)
        {
            if (_backgroundSoundtrackSource.isPlaying)
                return;

            _backgroundSoundtrackSource.loop = true;
            _backgroundSoundtrackSource.clip = _backgroundSoundtrack;
            _backgroundSoundtrackSource.volume = Mathf.Clamp01(volume);

            _backgroundSoundtrackSource.Play();
        }

        public void StopMusic()
        {
            _backgroundSoundtrackSource.Stop();
        }

        public void PauseMusic()
        {
            _backgroundSoundtrackSource.Pause();
        }

        public void ResumeMusic()
        {
            _backgroundSoundtrackSource.UnPause();
        }

        public void SetMusicVolume(float volume)
        {
            _backgroundSoundtrackSource.volume = Mathf.Clamp01(volume);
        }


        public void PlaySound(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;

            _soundEffectsSource.PlayOneShot(clip, volume);
        }

        public void SetSFXVolume(float volume)
        {
            _soundEffectsSource.volume = Mathf.Clamp01(volume);
        }

    }
}
