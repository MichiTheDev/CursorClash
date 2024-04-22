using System.Collections;
using UnityEngine;

namespace MichiTheDev
{
   public class AudioSourceObject : MonoBehaviour
   {
      private GameObject _owner;
      private AudioSource _audioSource;
      private AudioSource _oneShotAudioSource;
      private Coroutine _fadingRoutine;

      private void Awake()
      {
         _audioSource = gameObject.AddComponent<AudioSource>();
         _oneShotAudioSource = gameObject.AddComponent<AudioSource>();
      }

      public void ChangeVolume(float newVolume)
      {
         _audioSource.volume = newVolume;
      }

      public void ChangePitch(float pitch)
      {
         _audioSource.pitch = pitch;
      }
      
      public void PlayOneShot(AudioClipInfo audioClipInfo, bool randomPitch = false, Vector2 pitchRange = new ())
      {
         _oneShotAudioSource.outputAudioMixerGroup = audioClipInfo.AudioMixerGroup;
         _oneShotAudioSource.volume = audioClipInfo.Volume;
         if(randomPitch) _oneShotAudioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
         else _oneShotAudioSource.pitch = audioClipInfo.Pitch;
         _oneShotAudioSource.PlayOneShot(audioClipInfo.AudioClip);
      }

      public void DestroySelf(float seconds)
      {
         Destroy(gameObject, seconds);
      }
      
      public void Play(AudioClipInfo audioClipInfo)
      {
         _audioSource.outputAudioMixerGroup = audioClipInfo.AudioMixerGroup;
         _audioSource.clip = audioClipInfo.AudioClip;
         _audioSource.volume = audioClipInfo.Volume;
         _audioSource.pitch = audioClipInfo.Pitch;
         _audioSource.loop = audioClipInfo.Loop;
         _audioSource.Play();
      }

      public void StartFade(float targetVolume, float transitionTime)
      {
         if(_fadingRoutine != null)
         {
            StopCoroutine(_fadingRoutine);
         }
         
         _fadingRoutine = StartCoroutine(Fade(_audioSource.volume, targetVolume, transitionTime));
      }

      private IEnumerator Fade(float startVolume, float targetVolume, float transitionTime)
      {
         float fadeTimer = 0;
         while (fadeTimer < transitionTime)
         {
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, fadeTimer / transitionTime);
            fadeTimer += Time.deltaTime;
            yield return null;
         }
         _fadingRoutine = null;
      }
   }
}