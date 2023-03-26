using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager GetInstance() { return instance; }
    public static GameManager instance = null;

    private void Awake()
    {
        if (!instance) instance = this;
    }
    void Start()
    {
        magic = 200f;
        Maxmagic = 400;
        Minmagic = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUIState();
        Magic();
        MagicUI();
    }

    #region UI
    [Header("Scene")]
    public List<GameObject> listGUIScenes;
    public enum E_SCENE { PLAY, TITLE, GAMEOVER, MAX }
    public E_SCENE curScene;

    public GameObject pausePanel;
    private bool isPause = false;

    [Header("Sound")]
    public AudioMixer audioMixer;
    public Slider BgmSlider;

    GameManager gameManager;

    public GameObject bar;
    public GameObject magicbar;


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

    public void MagicUI()
    {
        RectTransform rectTransform = bar.GetComponent<RectTransform>();
        RectTransform rectTransform1 = magicbar.GetComponent<RectTransform>();

        float magic_x = ((magic - Minmagic)/(Maxmagic - Minmagic)) * rectTransform.rect.width + (rectTransform.anchoredPosition.x - rectTransform.rect.width/2);
        //Debug.Log(magic_x);
        rectTransform1.anchoredPosition = new Vector2(magic_x, rectTransform1.anchoredPosition.y);

    }

    #endregion

    #region 캐릭터
    [Header("캐릭터")]
    public GameObject player1;
    public GameObject player2;
    public float magic;
    public bool magicReverse;
    public bool itemReverse;
    public bool magicStop;

    public float Maxmagic = 400;
    public float Minmagic = 0;
    // Start is called before the first frame update
   

    public void Magic()
    {
        if (magicStop == false)
        {
            if (magicReverse)
            {
                magic += Time.deltaTime * 3f;
            }
            else magic -= Time.deltaTime * 3f;
        }
    }

    #endregion
}
