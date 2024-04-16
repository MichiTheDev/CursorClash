using UnityEngine;

namespace MichiTheDev
{
   public class AudioSourceObject : MonoBehaviour
   {
      private GameObject _owner;
      private AudioSource _audioSource;
      private AudioSource _oneShotAudioSource;

      private void Awake()
      {
         _audioSource = gameObject.AddComponent<AudioSource>();
         _oneShotAudioSource = gameObject.AddComponent<AudioSource>();
      }
      
      public void PlayOneShot(AudioClipInfo audioClipInfo)
      {
         _oneShotAudioSource.outputAudioMixerGroup = audioClipInfo.AudioMixerGroup;
         _oneShotAudioSource.volume = audioClipInfo.Volume;
         _oneShotAudioSource.pitch = audioClipInfo.Pitch;
         _oneShotAudioSource.PlayOneShot(audioClipInfo.AudioClip);
      }

      public void Play(AudioClipInfo audioClipInfo)
      {
         _audioSource.outputAudioMixerGroup = audioClipInfo.AudioMixerGroup;
         _audioSource.clip = audioClipInfo.AudioClip;
         _audioSource.volume = audioClipInfo.Volume;
         _audioSource.pitch = audioClipInfo.Pitch;
         _audioSource.loop = audioClipInfo.Loop;
         _audioSource.PlayOneShot(audioClipInfo.AudioClip);
      }
   }
}