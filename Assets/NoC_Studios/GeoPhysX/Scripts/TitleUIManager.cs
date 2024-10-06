using UnityEngine;
using UnityEngine.UI;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// The TitleUIManager class manages the user interface elements related to the title scene in the game.
    /// It handles the initialization, updates, and event responses for various UI components presented
    /// in the title screen, such as buttons, labels, and other interactive elements.
    /// </summary>
    /// <remarks>
    /// This class should be attached to a GameObject in the Unity Editor to function properly.
    /// Make sure all required UI elements are assigned via the Unity Inspector if needed.
    /// </remarks>
    public class TitleUIManager : MonoBehaviour
    {
        /// <summary>
        /// Initializes the TitleUIManager by setting up button listeners and refreshing the version text.
        /// </summary>
        /// <remarks>
        /// This method is called automatically by Unity when the script instance is being loaded.
        /// It finds the settings and quit buttons in the UI and attaches the appropriate event listeners
        /// to them. It also refreshes the version text displayed in the UI.
        /// </remarks>
        void Start()
        {
            Button playGameButton = GameObject.Find(GameManager.k_playGameButton).GetComponent<Button>();
            playGameButton.onClick.AddListener(GameManager.Instance.StartGame);
            
            Button settingsMenuButton = GameObject.Find(GameManager.k_settingsButtonName).GetComponent<Button>();
            settingsMenuButton.onClick.AddListener(GameManager.Instance.LoadSettingsScreen);

            Button quitMenuButton = GameObject.Find(GameManager.k_quitButtonName).GetComponent<Button>();
            quitMenuButton.onClick.AddListener(GameManager.Instance.QuitGame);
            
            GameManager.Instance.RefreshVersionText();
        }
    }
}
