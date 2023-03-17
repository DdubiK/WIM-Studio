using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
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
}
