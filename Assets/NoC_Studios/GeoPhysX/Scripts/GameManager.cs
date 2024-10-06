#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NoC.Studios.GeoPhysX
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Represents the name of the UI element that displays the game's version text.
        /// </summary>
        const string k_versionTextName = "VersionText";

        /// <summary>
        /// Represents the name of the UI element that serves as the settings button.
        /// </summary>
        public const string k_settingsButtonName = "SettingsButton";

        /// <summary>
        /// Represents the name of the UI element that serves as the quit button.
        /// </summary>
        public const string k_quitButtonName = "QuitButton";
        
        /// <summary>
        /// Enumerates the different scenes within the game.
        /// Each scene represents a distinct user interface or game state managed by the GameManager.
        /// </summary>
        enum GameScenes { Title = 0, SettingsMenu = 1 }
        
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
        /// Provides a singleton instance of the GameManager.
        /// Ensures only one instance of GameManager exists during the application lifecycle.
        /// </summary>
        public static GameManager Instance { get; private set; }

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
            RefreshVersionText();
        }

        /// <summary>
        /// Initiates the game start sequence by performing necessary setup.
        /// </summary>
        public void StartGame()
        {
            //TODO: Implement
            RefreshVersionText();
        }

        /// <summary>
        /// Loads the title screen by switching to the Title scene.
        /// Updates the version text displayed in the user interface.
        /// </summary>
        public void LoadTitleScreen()
        {
            SceneManager.LoadScene((int)GameScenes.Title);
            RefreshVersionText();
        }

        /// <summary>
        /// Loads the settings screen by switching to the SettingsMenu scene.
        /// Updates the version text displayed in the user interface.
        /// </summary>
        public void LoadSettingsScreen()
        {
            SceneManager.LoadScene((int)GameScenes.SettingsMenu);
            RefreshVersionText();
        }

        /// <summary>
        /// Sets the background music (BGM) volume level in the game.
        /// </summary>
        /// <param name="volumeLevel">The desired volume level for BGM, typically ranging from 0.0 (muted) to 1.0 (full volume).</param>
        public void SetVolume_BGM(float volumeLevel)
        {
            //TODO: Implement
        }

        /// <summary>
        /// Sets the sound effects (SFX) volume level in the game.
        /// </summary>
        /// <param name="volumeLevel">The desired volume level for SFX, typically ranging from 0.0 (muted) to 1.0 (full volume).</param>
        public void SetVolume_SFX(float volumeLevel)
        {
            //TODO: Implement
        }

        /// <summary>
        /// Sets the volume level for the specified audio source.
        /// </summary>
        /// <param name="source">The audio source to adjust the volume of.</param>
        /// <param name="volumeLevel">The desired volume level, typically between 0 (muted) and 1 (full volume).</param>
        void SetVolume(AudioSource source, float volumeLevel)
        {
            source.volume = volumeLevel;
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
        void RefreshVersionText()
        {
            GameObject versionLabel = GameObject.Find(k_versionTextName);

            if (!versionLabel) return;
            
            TextMeshProUGUI versionText = versionLabel.GetComponent<TextMeshProUGUI>();
            versionText.text = $"Version {Application.version} [{m_releaseType.ToString()}]";
        }
    }
}