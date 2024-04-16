using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MichiTheDev
{
   [Serializable]
   public struct AudioClipInfo
   {
      public AudioClip AudioClip;
      public AudioMixerGroup AudioMixerGroup;
      public float Volume;
      public float Pitch;
      public bool Loop;
   }
}