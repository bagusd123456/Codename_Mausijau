using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinCondition_PanelView : MonoBehaviour
{
    public Button nextLevelButton;
    public Button backToMenuButton;

    private void OnEnable()
    {
        nextLevelButton.interactable = SceneManager.sceneCountInBuildSettings != SceneManager.GetActiveScene().buildIndex + 1;

        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        backToMenuButton.onClick.AddListener(OnToMenuButtonClicked);
    }

    private void OnDisable()
    {
        nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
        backToMenuButton.onClick.RemoveListener(OnToMenuButtonClicked);
    }

    public void OnNextLevelButtonClicked()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void OnToMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
