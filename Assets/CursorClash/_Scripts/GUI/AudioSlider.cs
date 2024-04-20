using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MichiTheDev
{
   public class AudioSlider : MonoBehaviour
   {
      [SerializeField] private string name;
      [SerializeField] private TMP_Text _volumeText;
      
      private Slider _slider;

      private void Awake()
      {
         _slider = GetComponent<Slider>();
      }

      private void Start()
      {
         VolumeChanged(AudioManager.GetLinearVolume(name));
      }

      public void VolumeChanged(float volume)
      {
         AudioManager.ChangeFloatParamter(name, volume);
         _volumeText.text = Mathf.RoundToInt(_slider.value * 100).ToString();
      }
   }
}