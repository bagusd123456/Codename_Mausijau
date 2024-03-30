using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_PanelView : MonoBehaviour
{
    public Button homeButton;
    public Button selectLevelButton;
    public Button creditButton;
    public Button settingButton;
    public Button quitButton;
    public void LoadLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
