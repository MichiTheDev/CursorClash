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
        }

        public static void ChangeVolume(string group, float linearVolume)
        {
            _audioMixer.SetFloat(group, LinearToDecibel(linearVolume));
        }

        public static float GetLinearVolume(string group)
        {
            if(_audioMixer.GetFloat(group, out float volume))
            {
                return DecibelToLinear(volume);
            }
            return 0f;
        }

        public static float GetDecibelVolume(string group)
        {
            if(_audioMixer.GetFloat(group, out float volume))
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