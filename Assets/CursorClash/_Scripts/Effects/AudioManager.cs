using System;
using System.Collections.Generic;
using UnityEngine;

namespace MichiTheDev
{
    public class AudioManager : MonoBehaviour
    {
        private static Dictionary<string, AudioClip> _SFXAudioClips = new();
        private static Dictionary<string, AudioClip> _musicAudioClips = new();

        private static float _masterVolume = 1f;
        private static float _SFXVolume = 0.33f;
        private static float _musicVolume = 0.25f;
        
        private static GameObject _SFXParent;
        private static GameObject _musicParent;
        
        private void Awake()
        {
            AudioClip[] SFXClips = Resources.LoadAll<AudioClip>("Audio/SFX");
            foreach(AudioClip audioClip in SFXClips)
            {
                _SFXAudioClips.Add(audioClip.name, audioClip);
            }
            
            AudioClip[] musicClips = Resources.LoadAll<AudioClip>("Audio/Music");
            foreach(AudioClip audioClip in musicClips)
            {
                _musicAudioClips.Add(audioClip.name, audioClip);
            }

            _SFXParent = new GameObject("SFX");
            _SFXParent.transform.SetParent(transform);

            _musicParent = new GameObject("Music");
            _musicParent.transform.SetParent(transform);

            DontDestroyOnLoad(gameObject);
        }

        public static void Play(AudioClipInfo audioClipInfo)
        {
            AudioSource audioSource = new GameObject(audioClipInfo.Name).AddComponent<AudioSource>();

            switch(audioClipInfo.Category)
            {
                case "SFX":
                    audioSource.transform.SetParent(_SFXParent.transform);
                    audioSource.clip = _SFXAudioClips[audioClipInfo.Name];
                    audioSource.volume = audioClipInfo.Volume * _SFXVolume * _masterVolume;
                    break;
                case "Music":
                    audioSource.transform.SetParent(_musicParent.transform);
                    audioSource.clip = _musicAudioClips[audioClipInfo.Name];
                    audioSource.volume = audioClipInfo.Volume * _musicVolume * _masterVolume;
                    break;
            }
            
            audioSource.playOnAwake = false;
            audioSource.loop = audioClipInfo.Loop;
            audioSource.pitch = audioClipInfo.Pitch;
            audioSource.Play();

            if(!audioSource.loop)
            {
                Destroy(audioSource.gameObject, audioSource.clip.length);
            }
        }
    }

    [Serializable]
    public struct AudioClipInfo
    {
        public string Name;
        public string Category;
        public float Volume;
        public float Pitch;
        public bool Loop;
    }
}
