#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using TMPro;

namespace NoC.Studios.GeoPhysX
{
    public class GameManager : MonoBehaviour
    {
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
        /// Represents the UI text field that displays the current version of the game.
        /// </summary>
        [SerializeField] TextMeshProUGUI m_versionTextField;

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
            m_versionTextField.text = $"Version {Application.version} [{m_releaseType.ToString()}]";
        }
        public void StartGame()
        {
            //TODO: Implement
        }

        public void OpenSettingsScreen()
        {
            //TODO: Implement
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
    }
}