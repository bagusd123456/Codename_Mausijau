using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoseCondition_PanelView : MonoBehaviour
{
    public Button restartButton;
    public Button backToMenuButton;

    private void OnEnable()
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        backToMenuButton.onClick.AddListener(OnToMenuButtonClicked);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        backToMenuButton.onClick.RemoveListener(OnToMenuButtonClicked);
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnToMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
