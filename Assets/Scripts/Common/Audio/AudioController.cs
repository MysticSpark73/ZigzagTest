using UnityEngine;
using UnityEngine.Audio;

namespace Zagzag.Common.Audio
{
    public class AudioController : MonoBehaviour
    {
        public bool IsMuted { get; private set; }

        [SerializeField] private AudioSource menuSource;
        [SerializeField] private AudioSource gameOverSource;
        [SerializeField] private AudioSource gemSource;
        [SerializeField] private AudioSource ballSource;
        [SerializeField] private AudioSource ambientsource;
        [SerializeField] private AudioMixer mixer;

        private string masterVolumeKey = "masterVolume";
        private float pitchShift = .25f;


        public static AudioController Instance;
        private void Awake()
        {
            Instance = this;
            
        }

        public void PlaySound(Sounds sound) 
        {
            switch (sound)
            {
                case Sounds.Menu:
                    menuSource.Play();
                    break;
                case Sounds.GameOver:
                    gameOverSource.Play();
                    break;
                case Sounds.GemPickup:
                    ShiftPitch(gemSource);
                    gemSource.Play();
                    break;
                case Sounds.Ball:
                    ShiftPitch(ballSource);
                    ballSource.Play();
                    break;
                case Sounds.Ambient:
                    ambientsource.Play();
                    break;
                default:
                    break;
            }
        }

        public void MuteSounds() 
        {
            IsMuted = true;
            mixer.SetFloat(masterVolumeKey, -80);
        }

        public void UnmuteSounds() 
        {
            IsMuted = false;
            mixer.SetFloat(masterVolumeKey, 0);
        }

        private void ShiftPitch(AudioSource source) 
        {
            source.pitch = Random.Range(1-pitchShift, 1+pitchShift);
        }
    }
}
