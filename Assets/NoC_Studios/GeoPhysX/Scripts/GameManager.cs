#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// The GameManager class is responsible for managing the overall state and
    /// flow of the game, including transitioning between scenes, setting volume levels,
    /// and handling game startup and quitting.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Represents the name of the UI element that displays the game's version text.
        /// </summary>
        const string k_versionTextName = "VersionText";

        /// <summary>
        /// Represents the name of the UI element that triggers starting the game.
        /// </summary>
        public const string k_playGameButton = "PlayButton";

        /// <summary>
        /// Represents the name of the UI element that serves as the settings button.
        /// </summary>
        public const string k_settingsButtonName = "SettingsButton";

        /// <summary>
        /// Represents the name of the UI element that serves as the quit button.
        /// </summary>
        public const string k_quitButtonName = "QuitButton";

        /// <summary>
        /// Represents the minimum volume level allowed in the game, typically ranging from 0.0 (muted).
        /// </summary>
        const float k_minVolume = 0.0f;

        /// <summary>
        /// Represents the maximum allowable volume level for audio settings in the game.
        /// </summary>
        const float k_maxVolume = 1.0f;

        /// <summary>
        /// Represents the default volume level for background music (BGM) in the game.
        /// This is typically a float value between 0.0 (muted) and 1.0 (full volume).
        /// </summary>
        const float k_defaultVolume_BGM = 0.5f;

        /// <summary>
        /// Represents the default volume level for sound effects (SFX) in the game.
        /// </summary>
        const float k_defaultValue_SFX = k_maxVolume;

        /// <summary>
        /// Enumerates the different scenes within the game.
        /// Each scene represents a distinct user interface or game state managed by the GameManager.
        /// </summary>
        enum GameScenes
        {
            Title = 0,
            SettingsMenu = 1,
            Game = 2
        }

        /// <summary>
        /// Defines the types of release builds for the game.
        /// Detailed to separate between development builds and production releases.
        /// </summary>
        enum ReleaseTypes { Development, Release }

        /// <summary>
        /// Specifies the current type of game release, distinguishing between development and production builds.
        /// </summary>
        const ReleaseTypes m_releaseType = ReleaseTypes.Development;

        /// <summary>
        /// Holds the current volume level for the background music (BGM) in the game.
        /// Typically a float value between 0.0 (muted) and 1.0 (full volume).
        /// </summary>
        float m_volumeLevel_BGM = k_defaultVolume_BGM;

        /// <summary>
        /// Holds the current volume level for sound effects (SFX) in the game.
        /// </summary>
        float m_volumeLevel_SFX = k_defaultValue_SFX;

        /// <summary>
        /// Stores the name of the player.
        /// </summary>
        string m_playerName;
        
        /// <summary>
        /// The audio source that handles the background music (BGM) in the game.
        /// </summary>
        [SerializeField] AudioSource m_audioSourceBGM;

        /// <summary>
        /// Represents the audio source component responsible for playing sound effects in the game.
        /// </summary>
        [SerializeField] AudioSource m_audioSourceSFX;

        /// <summary>
        /// Represents the audio clip that plays when a click event is triggered.
        /// </summary>
        [SerializeField] AudioClip m_clickSound;

        /// <summary>
        /// Represents the sound effect played when a game piece is dropped.
        /// </summary>
        [SerializeField] AudioClip m_dropPieceSound;

        /// <summary>
        /// A list of audio clips that are played when pieces collide during the game.
        /// </summary>
        [SerializeField] List<AudioClip> m_pieceCollisionSounds;

        /// <summary>
        /// Represents the audio clip that plays when a game piece is removed.
        /// </summary>
        [SerializeField] AudioClip m_removePieceSound;

        /// <summary>
        /// Provides a singleton instance of the GameManager.
        /// Ensures only one instance of GameManager exists during the application lifecycle.
        /// </summary>
        public static GameManager Instance { get; private set; }
        
        /// <summary>
        /// Retrieves the current volume level for the background music (BGM) in the game.
        /// The volume level is typically a float value between 0.0 (muted) and 1.0 (full volume).
        /// </summary>
        public float Volume_BGM => m_volumeLevel_BGM;

        /// <summary>
        /// Gets the current volume level for sound effects (SFX) in the game.
        /// </summary>
        public float Volume_SFX => m_volumeLevel_SFX;

        /// <summary>
        /// Gets the player's name in the game.
        /// </summary>
        public string PlayerName => m_playerName;

        /// <summary>
        /// Provides access to the audio source component used for playing sound effects.
        /// </summary>
        AudioSource AudioSource_SFX => m_audioSourceSFX;

        /// <summary>
        /// Initializes the GameManager instance if it does not already exist.
        /// Ensures that the GameManager object persists across scene loads.
        /// </summary>
        void Awake() 
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Sets the version text field in the UI to display the current application version and release type.
        /// </summary>
        void Start()
        {
            SetVolume_BGM(m_volumeLevel_BGM);
            SetVolume_SFX(m_volumeLevel_SFX);
            RefreshVersionText();
        }

        /// <summary>
        /// Initiates the game start sequence by performing necessary setup.
        /// </summary>
        public static void StartGame()
        {
            SceneManager.LoadScene((int)GameScenes.Game);
        }

        /// <summary>
        /// Loads the title screen by switching to the Title scene.
        /// Updates the version text displayed in the user interface.
        /// </summary>
        public static void LoadTitleScreen()
        {
            SceneManager.LoadScene((int)GameScenes.Title);
        }

        /// <summary>
        /// Loads the settings screen by switching to the SettingsMenu scene.
        /// Updates the version text displayed in the user interface.
        /// </summary>
        public static void LoadSettingsScreen()
        {
            SceneManager.LoadScene((int)GameScenes.SettingsMenu);
        }

        /// <summary>
        /// Sets the background music (BGM) volume level in the game.
        /// </summary>
        /// <param name="volumeLevel">The desired volume level for BGM, typically ranging from 0.0 (muted) to 1.0 (full volume).</param>
        public void SetVolume_BGM(float volumeLevel)
        {
            m_volumeLevel_BGM = volumeLevel;
            SetVolume(m_audioSourceBGM, m_volumeLevel_BGM);
        }

        /// <summary>
        /// Sets the sound effects (SFX) volume level in the game.
        /// </summary>
        /// <param name="volumeLevel">The desired volume level for SFX, typically ranging from 0.0 (muted) to 1.0 (full volume).</param>
        public void SetVolume_SFX(float volumeLevel)
        {
            m_volumeLevel_SFX = volumeLevel;
            SetVolume(m_audioSourceSFX, m_volumeLevel_SFX);
        }

        /// <summary>
        /// Sets the volume level for the specified audio source.
        /// </summary>
        /// <param name="source">The audio source to adjust the volume of.</param>
        /// <param name="volumeLevel">The desired volume level, typically between 0 (muted) and 1 (full volume).</param>
        static void SetVolume(AudioSource source, float volumeLevel)
        {
            source.volume = volumeLevel;
        }

        /// <summary>
        /// Plays the click sound effect using the configured settings for sound effects volume.
        /// </summary>
        public void PlayClickSound()
        {
            AudioSource_SFX.PlayOneShot(m_clickSound, Volume_SFX);
        }

        /// <summary>
        /// Plays the sound effect associated with dropping a game piece.
        /// </summary>
        public void PlayDropSound()
        {
            AudioSource_SFX.PlayOneShot(m_dropPieceSound, Volume_SFX);
        }

        /// <summary>
        /// Plays a random collision sound effect from the list of predefined audio clips.
        /// The sound effect is played at the current SFX volume level.
        /// </summary>
        public void PlayCollisionSound()
        {
            var clipIndex = Random.Range(0, m_pieceCollisionSounds.Count);
            AudioSource_SFX.PlayOneShot(m_pieceCollisionSounds[clipIndex], Volume_SFX);
        }

        /// <summary>
        /// Plays the sound effect for when a game piece is removed.
        /// Uses the configured volume level for sound effects.
        /// </summary>
        public void PlayRemoveSound()
        {
            AudioSource_SFX.PlayOneShot(m_removePieceSound, Volume_SFX);
        }

        /// <summary>
        /// Sets the name of the player.
        /// </summary>
        /// <param name="playerName">The name to be assigned to the player.</param>
        public void SetPlayerName(string playerName)
        {
            m_playerName = playerName;
        }

        /// <summary>
        /// Quits the currently running game.
        /// In the Unity Editor, this will exit Playmode.
        /// In a built application, this will close the application.
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Updates the version text in the UI to reflect the current application version and release type.
        /// </summary>
        public static void RefreshVersionText()
        {
            var versionLabel = GameObject.Find(k_versionTextName);

            if (!versionLabel) return;
            
            var versionText = versionLabel.GetComponent<TextMeshProUGUI>();
            versionText.text = $"Version {Application.version} [{m_releaseType.ToString()}]";
        }
    }
}