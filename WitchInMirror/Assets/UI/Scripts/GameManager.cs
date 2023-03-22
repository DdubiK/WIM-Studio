using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public List<GameObject> listGUIScenes;
    public enum E_SCENE { PLAY, TITLE, GAMEOVER, MAX }
    public E_SCENE curScene;

    public GameObject pausePanel;
    public GameObject settingPanel;
    private bool isPause = false;
    private bool isSetting = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    GameManager gameManager;

    public void Initialize(GameManager gameManager)
    {
        gameObject.SetActive(true);
        SetGUIState(curScene);
        this.gameManager = gameManager;
    }

    public void ShowGUIState(E_SCENE scene)
    {
        for (E_SCENE idx = E_SCENE.PLAY; idx < E_SCENE.MAX; idx++)
        {
            if (idx == scene)
                listGUIScenes[(int)idx].SetActive(true);
            else
                listGUIScenes[(int)idx].SetActive(false);
        }
    }

    public void SetGUIState(E_SCENE scene)
    {
        switch (scene)
        {
            case E_SCENE.PLAY:
                Time.timeScale = 1;
                break;
            case E_SCENE.TITLE:
                Time.timeScale = 0;
                break;
            case E_SCENE.GAMEOVER:
                Time.timeScale = 0;
                break;
        }
        ShowGUIState(scene);
        curScene = scene;
    }

    public void EventSenceChange(int idx)
    {
        SetGUIState((E_SCENE)idx);
    }

    public void SetPause()
    {
        if (!isPause)
        {
            isPause = true;
            Time.timeScale = 0.0f;
            pausePanel.gameObject.SetActive(true);
        }
    }

    public void PauseContinue()
    {
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1f;
            pausePanel.gameObject.SetActive(false);
        }
    }

    public void SetSetting()
    {
        if (!isSetting)
        {
            isSetting = true;
            Time.timeScale = 0.0f;
            settingPanel.gameObject.SetActive(true);
        }
    }

    public void SettingContinue()
    {
        if (isSetting)
        {
            isSetting = false;
            Time.timeScale = 1f;
            settingPanel.gameObject.SetActive(false);
        }
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
