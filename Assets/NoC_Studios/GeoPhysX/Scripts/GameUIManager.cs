using UnityEngine;
using UnityEngine.UI;

namespace NoC.Studios.GeoPhysX
{
    /// <summary>
    /// Manages the game's user interface components.
    /// </summary>
    public class GameUIManager : MonoBehaviour
    {
        /// <summary>
        /// Initializes the game UI by setting up the return to title button listener
        /// and refreshing the displayed version text.
        /// </summary>
        void Start()
        {
            Button returnToTitleButton = GameObject.Find(GameManager.k_returnToTitleButtonName).GetComponent<Button>();
            returnToTitleButton.onClick.AddListener(GameManager.Instance.LoadTitleScreen);
            
            GameManager.Instance.RefreshVersionText();
        }
    }
}
