using System;
using UnityEngine;
using UnityEngine.UI;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// The SettingsUIManager class handles user interface interactions related to game settings.
    /// This includes adjusting volume levels for background music (BGM) and sound effects (SFX),
    /// as well as managing navigation to the title screen.
    /// </summary>
    public class SettingsUIManager : MonoBehaviour
    {
        /// <summary>
        /// Specifies options for controlling sound settings in the game.
        /// </summary>
        enum SoundOptions
        {
            BGM,
            SFX
        }

        /// <summary>
        /// Button that navigates the user back to the main menu when clicked.
        /// </summary>
        [SerializeField] Button m_backToMenuButton;

        /// <summary>
        /// Slider that controls the volume level of the background music (BGM).
        /// </summary>
        [SerializeField] Slider m_volumeSlider_BGM;

        /// <summary>
        /// Slider component for adjusting the sound effects (SFX) volume in the game settings.
        /// </summary>
        [SerializeField] Slider m_volumeSlider_SFX;

        /// <summary>
        /// Initializes the volume sliders for background music (BGM) and sound effects (SFX)
        /// to their respective current volume levels and refreshes the version text in the UI.
        /// </summary>
        void Start()
        {
            RefreshUI();
            GameManager.Instance.RefreshVersionText();
        }

        /// <summary>
        /// Sets the volume level for background music (BGM) using the value from the BGM volume slider.
        /// </summary>
        public void SetVolume_BGM()
        {
            GameManager.Instance.SetVolume_BGM(GetSliderVolmeLevel(SoundOptions.BGM));
        }

        /// <summary>
        /// Sets the volume level for sound effects (SFX) using the value from the SFX volume slider.
        /// </summary>
        public void SetVolume_SFX()
        {
            GameManager.Instance.SetVolume_SFX(GetSliderVolmeLevel(SoundOptions.SFX));
        }

        /// <summary>
        /// Retrieves the volume level for the specified sound option.
        /// </summary>
        /// <param name="soundOption">The sound option (BGM or SFX) for which the volume level is requested.</param>
        /// <returns>The volume level corresponding to the provided sound option.</returns>
        float GetVolumeLevel(SoundOptions soundOption)
        {
            switch (soundOption)
            {
                case SoundOptions.BGM:
                    return GameManager.Instance.Volume_BGM;
                case SoundOptions.SFX:
                    return GameManager.Instance.Volume_SFX;
                default:
                    throw new ArgumentOutOfRangeException(nameof(soundOption), soundOption, null);
            }
        }

        /// <summary>
        /// Retrieves the current volume level from the specified volume slider.
        /// </summary>
        /// <param name="soundOption">The type of sound (BGM or SFX) for which the volume level is being retrieved.</param>
        /// <returns>The current value of the specified volume slider.</returns>
        float GetSliderVolmeLevel(SoundOptions soundOption)
        {
            switch (soundOption)
            {
                case SoundOptions.BGM:
                    return m_volumeSlider_BGM.value;
                case SoundOptions.SFX:
                    return m_volumeSlider_SFX.value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(soundOption), soundOption, null);
            }
        }

        /// <summary>
        /// Updates the UI sliders to reflect the current volume levels for both background music (BGM) and sound effects (SFX).
        /// </summary>
        void RefreshUI()
        {
                m_volumeSlider_BGM.value = GetVolumeLevel(SoundOptions.BGM);
                m_volumeSlider_SFX.value = GetVolumeLevel(SoundOptions.SFX);
        }

        /// <summary>
        /// Loads the title screen by switching to the Title scene.
        /// </summary>
        public void LoadTitleScreen()
        {
            GameManager.Instance.PlayClickSound();
            GameManager.Instance.LoadTitleScreen();
        }
    }
}