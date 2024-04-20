using UnityEngine;
using UnityEngine.Audio;

namespace MichiTheDev
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioMixer _audioMixer;

        private void Awake()
        {
            _audioMixer = Resources.Load<AudioMixer>("Audio/MainMixer");
            
            DontDestroyOnLoad(gameObject);
        }

        public static void ChangeFloatParamter(string name, float linearVolume)
        {
            _audioMixer.SetFloat(name, LinearToDecibel(linearVolume));
        }

        public static float GetLinearVolume(string name)
        {
            if(_audioMixer.GetFloat(name, out float volume))
            {
                return DecibelToLinear(volume);
            }
            return 0f;
        }

        public static float GetDecibelVolume(string name)
        {
            if(_audioMixer.GetFloat(name, out float volume))
            {
                return volume;
            }
            return -144.0f;
        }

        public static AudioMixerGroup FindAudioMixerGroup(string group)
        {
            AudioMixerGroup audioMixerGroup = _audioMixer.FindMatchingGroups(group)[0];
            return audioMixerGroup == null ? null : audioMixerGroup;
        }
        
        public static float LinearToDecibel(float linearVolume)
        {
            return linearVolume != 0 ? 20.0f * Mathf.Log10(linearVolume) : -144.0f;
        }

        public static float DecibelToLinear(float decibel)
        {
            if(decibel == 0) return 1f;
            return Mathf.Pow(10.0f, decibel / 20.0f);
        }
    }
}