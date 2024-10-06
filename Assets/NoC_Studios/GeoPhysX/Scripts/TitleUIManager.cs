using UnityEngine;
using UnityEngine.UI;

namespace NoC.Studios.GeoPhysX
{
    public class TitleUIManager : MonoBehaviour
    {
        void Start()
        {
            Button settingsMenuButton = GameObject.Find(GameManager.k_settingsButtonName).GetComponent<Button>();
            settingsMenuButton.onClick.AddListener(GameManager.Instance.LoadSettingsScreen);

            Button quitMenuButton = GameObject.Find(GameManager.k_quitButtonName).GetComponent<Button>();
            quitMenuButton.onClick.AddListener(GameManager.Instance.QuitGame);
        }
    }
}
