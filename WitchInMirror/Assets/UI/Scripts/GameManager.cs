using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public List<GameObject> listGUIScenes;
    public enum E_SCENE { PLAY, TITLE, GAMEOVER, MAX }
    public E_SCENE curScene;

    public GameObject pausePanel;
    private bool isPause = false;

    public AudioMixer audioMixer;
    public Slider BgmSlider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUIState();
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

    public void UpdateGUIState()
    {
        switch (curScene)
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

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }
}
