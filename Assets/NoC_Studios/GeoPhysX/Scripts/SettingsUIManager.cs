using System;
using UnityEngine;
using UnityEngine.UI;

namespace NoC.Studios.GeoPhysX
{
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
        /// Sets the volume level for background music (BGM) using the value from the BGM volume slider.
        /// </summary>
        public void SetVolume_BGM()
        {
            GameManager.Instance.SetVolume_BGM(GetVolumeLevel(SoundOptions.BGM));
        }

        /// <summary>
        /// Sets the volume level for sound effects (SFX) using the value from the SFX volume slider.
        /// </summary>
        public void SetVolume_SFX()
        {
            GameManager.Instance.SetVolume_SFX(GetVolumeLevel(SoundOptions.SFX));
        }

        /// <summary>
        /// Retrieves the current volume level based on the specified sound option.
        /// </summary>
        /// <param name="soundOption">The sound option to retrieve the volume level for, either BGM or SFX.</param>
        /// <returns>The current volume level as a float value, typically ranging from 0.0 (muted) to 1.0 (full volume).</returns>
        float GetVolumeLevel(SoundOptions soundOption)
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
        /// Loads the title screen by switching to the Title scene.
        /// </summary>
        public void LoadTitleScreen()
        {
            GameManager.Instance.LoadTitleScreen();
        }
    }
}