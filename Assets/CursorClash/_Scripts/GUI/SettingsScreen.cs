using UnityEngine;
using UnityEngine.UI;

namespace MichiTheDev
{
    public class SettingsScreen : MonoBehaviour
    {
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Start()
        {
            _masterSlider.value = AudioManager.GetLinearVolume("Master_Volume");
            _musicSlider.value = AudioManager.GetLinearVolume("Music_Volume");
            _sfxSlider.value = AudioManager.GetLinearVolume("SFX_Volume");
        }

        public void MasterVolumeChanged(float volume)
        {
            AudioManager.ChangeFloatParamter("Master_Volume", volume);
        }

        public void MusicVolumeChanged(float volume)
        {
            AudioManager.ChangeFloatParamter("Music_Volume", volume);
        }

        public void SFXVolumeChanged(float volume)
        {
            AudioManager.ChangeFloatParamter("SFX_Volume", volume);
        }
    }
}