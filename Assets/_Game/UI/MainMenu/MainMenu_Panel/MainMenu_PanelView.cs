using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_PanelView : MonoBehaviour
{
    public List<GameObject> mainMenuPanelList = new List<GameObject>();
    public List<PlayableDirector> playableDirectorList = new List<PlayableDirector>();

    public Button homeButton;
    public Button selectLevelButton;
    public Button creditButton;
    public Button settingButton;
    public Button quitButton;

    public PlayableDirector lastPlayableDirector;
    public void LoadLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowPanel(GameObject panel)
    {
        foreach (var item in mainMenuPanelList)
        {
            item.SetActive(false);
        }
        panel.SetActive(true);
    }

    public void HideAllPanel()
    {
        foreach (var item in mainMenuPanelList)
        {
            item.SetActive(false);
        }
    }

    public void PlayDirector(PlayableDirector director)
    {
        foreach (var item in playableDirectorList)
        {
            item.Stop();
            //item.RebuildGraph();
        }

        PlayableDirector directorToPlay = director;

        if (lastPlayableDirector != null)
        {
            //check last director index in list
            int index = playableDirectorList.IndexOf(director);
            int lastIndex = playableDirectorList.IndexOf(lastPlayableDirector);
            //if current index is less than last Index, get director from list contains name of current director
            if (index < lastIndex)
            {
                directorToPlay = playableDirectorList.FirstOrDefault(x=> x.name.Contains(director.name));
            }
        }

        directorToPlay.Play();
    }
}
