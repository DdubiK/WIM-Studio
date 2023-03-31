using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
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
        SetGUIState(E_SCENE.TITLE);
        CharStart();
        UIStart();
        MapStart();
        SceneUpdate += UIUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        SceneUpdate?.Invoke();
    }


    #region 씬
    [Header("씬")]
    public Action SceneUpdate = null;
    public E_SCENE curScene;


    public void EventSenceChange(int idx)
    {
        Debug.Log("check");
        SetGUIState((E_SCENE)idx);
    }

    #endregion


    #region UI
    [Header("UI")]
    public List<GameObject> listGUIScenes;
    private bool isPause = false;
    public GameObject pausePanel;
    public GameObject objCanvas;
    public TextMeshProUGUI textScore;

    [Header("사운드")]
    public AudioMixer audioMixer;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public AudioSource audioSource;
    public AudioSource bgmSource;
    public List<AudioClip> audioClip;

    public GameObject bar;
    public GameObject magicbar;


    void UIStart()
    {
        objCanvas = GameObject.FindGameObjectWithTag("Canvas");
        audioSource = this.GetComponent<AudioSource>();
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump1"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump2"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump3"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump4"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump5"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump7"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump8"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump9"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump12"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/jump13"));
        audioSource.clip = audioClip[0];

        audioSource.clip = bgmSource.clip;
        UIratio();

        SFXSlider.value = 0.5f;
        BGMSlider.value = 0.5f;

    }

    void UIUpdate()
    {
        MagicUI();
        //UpdateGUIState();
    }


    void SoundPlay(int soundnumber)
    {
        if (soundnumber > audioClip.Count) { return; }
        audioSource.clip = audioClip[soundnumber];

        audioSource.Play();
    }



    public void UIratio()
    {
        RectTransform rtCanvas = objCanvas.GetComponent<RectTransform>();
        listGUIScenes[0].GetComponent<RectTransform>().sizeDelta = rtCanvas.sizeDelta;
        listGUIScenes[1].GetComponent<RectTransform>().sizeDelta = rtCanvas.sizeDelta;
        listGUIScenes[2].GetComponent<RectTransform>().sizeDelta = rtCanvas.sizeDelta;
    }

    public void ShowGUIState(E_SCENE scene)
    {
        for (int idx = 0; idx < (int)E_SCENE.MAX; idx++)
        {
            if ((E_SCENE)idx == scene)
                listGUIScenes[(int)idx].SetActive(true);
            else
                listGUIScenes[(int)idx].SetActive(false);
        }
    }


    //public void Initialize(GameManager gameManager)
    //{
    //    gameObject.SetActive(true);
    //    SetGUIState(curScene);
    //}

    public void SetGUIState(E_SCENE scene)
    {
        switch (scene)
        {
            case E_SCENE.TITLE:
                Time.timeScale = 0;
                break;
            case E_SCENE.PLAY:
                Time.timeScale = 1;
                mapEditor.ResetPooling();
                resetCharState();
                SceneUpdate += CharUpdate;
                SceneUpdate += MapUpdate;
                break;
            case E_SCENE.GAMEOVER:
                Time.timeScale = 0;
                //textScore.text = "Score : " + Score;
                //SceneUpdate -= UIUpdate;
                SceneUpdate -= CharUpdate;
                SceneUpdate -= MapUpdate;
                break;
        }
        ShowGUIState(scene);
        curScene = scene;
    }

    //public void UpdateGUIState()
    //{
    //    switch (curScene)
    //    {
    //        case E_SCENE.PLAY:
    //            Time.timeScale = 1;
    //            break;
    //        case E_SCENE.TITLE:
    //            Time.timeScale = 0;
    //            break;
    //        case E_SCENE.GAMEOVER:
    //            Time.timeScale = 0;
    //            break;
    //    }
    //}





    public void MagicLow()
    {
        if (magic <= 60)
        {
            Debug.Log("QQ");
        }
        else if (magic >= 360)
        {
            Debug.Log("WW");
        }
    }
    #endregion


    #region UI


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

    public void SetBGMVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BGMSlider.value) * 20);
    }
    
    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXSlider.value) * 20);
    }

    public void MagicUI()
    {
        RectTransform rectTransform = bar.GetComponent<RectTransform>();
        RectTransform rectTransform1 = magicbar.GetComponent<RectTransform>();

        float magic_x = ((magic - Minmagic) / (Maxmagic - Minmagic)) * rectTransform.rect.width + (rectTransform.anchoredPosition.x - rectTransform.rect.width / 2);
        //Debug.Log(magic_x);
        rectTransform1.anchoredPosition = new Vector2(magic_x, rectTransform1.anchoredPosition.y);

    }

    public void MagicCheck()
    {
        if (magic <= Minmagic)
        {
            magic = Minmagic;
            SetGUIState(E_SCENE.GAMEOVER);
            //Debug.Log("GameOver");
            return;
        }
        if (magic >= Maxmagic) 
        {
            magic = Maxmagic;
            SetGUIState(E_SCENE.GAMEOVER);
            return;
        }
    }


    #endregion

    #region 캐릭터, 마력
    [Header("캐릭터")]
    public GameObject[] player = new GameObject[2];
    public float jump = 3f;
    public float Score = 0;
    public bool isGround;
    public bool jumpUp;
    public Vector3 disToGround;
    public Vector3 disToJumpPos;
    public Action GroundCheck = null;

    [Header("마력")]
    public float Maxmagic = 400;
    public float Minmagic = 0;
    public float magic;
    public float upMagic = 10f;
    public float magicstopTime;
    public bool magicReverse;
    public bool magicStop;
    public float magicDecreasePer = 15f;

    [Header("시스템")]
    public float time;
    public float playtime;
    public bool itemReverse;
    public float itemreverseTime;



    void CharStart()
    {
        magic = 200f;
        Maxmagic = 400;
        Minmagic = 0;
        Score = 0;
        isGround = true;
        jumpUp = false;
        disToGround = new Vector3(-1.9f, 0, 0);
        disToJumpPos = new Vector3(-1.9f, 0.7f, 0);
    }

    //
    void CharUpdate()
    {
        Magic();
        MagicUI();
        timer();
        GroundCheck?.Invoke();//액션에 함수가 들어왔을때 실행 아닐땐 넘기기
    }

    public void UpScore(int _point)
    {
        Score += _point;
    }

    //거리로 점프 체크
    public void GroundChecking()
    {
        //distanc로 거리 체크 하여 설정
        //Debug.Log(Vector3.Distance(player[0].transform.position, disToGround));
        //Debug.Log(Vector3.Distance(player[0].transform.position, disToJumpPos));

        if (!isGround && Vector3.Distance(player[0].transform.position, disToJumpPos) < 0.5f)
        {
            jumpUp = false;
        }

        if (!isGround && !jumpUp && Vector3.Distance(player[0].transform.position,disToGround)<=0.5f)
        {
            isGround = true;
            GroundCheck -= GroundChecking;
            return;
        }
        
    }


    public void Jump()
    {
        if (isGround)
        {
            player[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, jump);
            player[1].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -jump);
            isGround = false;
            jumpUp = true;
            GroundCheck += GroundChecking;

            SoundPlay(2);
        }
    }


    public void timer()
    {
        time += Time.deltaTime;
        playtime += Time.deltaTime;
        itemreverseTime += Time.deltaTime;
        magicstopTime += Time.deltaTime;
    }



    public void MagicUp()
    {
        if (itemReverse == false)
        {
            if (magicStop == false)
            {
                magic += upMagic;
                Debug.Log("+magic");
            }
        }
        else
        {
            if (magicStop == false)
            {
                magic -= upMagic;
                Debug.Log("-magic");
            }
        }
        Score += 100;
    }

    public void MagicDown()
    {
        if (itemReverse == false)
        {
            if (magicStop == false)
            {
                magic -= upMagic;
                Debug.Log("+magic");
            }
        }
        else
        {
            if (magicStop == false)
            {
                magic += upMagic;
                Debug.Log("-magic");
            }
        }
        Score += 100;
    }




    public void Magic()
    {
        if (magicStop == false)
        {
            if (magicReverse)
            {
                magic += Time.deltaTime * magicDecreasePer;
            }
            else magic -= Time.deltaTime * magicDecreasePer;
        }
        MagicCheck();
    }


    public void resetCharState()
    {
        magic = 200f;
        Maxmagic = 400;
        Minmagic = 0;
        isGround = true;
        jumpUp = false;
        Score = 0;

    }


    #endregion

    #region 아이템 효과
    [Header("아이템 획득 효과")]
    public Mist mist;
    public bool isDamaged;
    public bool isShield;
    public bool coroutineStart1;//ItemReverseCoroutineStart
    public bool coroutineStart2;//MagicStopCoroutineStart


    public void ShieldItem()
    {
        if (coroutineStart2 == false)
        {
            gameObject.transform.Find("Shield").gameObject.SetActive(true);
            isShield = true;
        }
    }


    public void Damaged()
    {
        if (coroutineStart2 == false)
        {
            if (isShield)
            {
                gameObject.transform.Find("Shield").gameObject.SetActive(false);
                isShield = false;
            }
            else
            {
                Damage();
                Debug.Log("isDamaged!");
            }
        }
    }


    public void MagicReverseItem()
    {
        if (magicReverse == false)
        {
            Debug.Log("Reverse!!!");
            magicReverse = true;
        }
        else
        {
            Debug.Log("Return!!!");
            magicReverse = false;
        }
    }

    public void ItemReverseItem()
    {
        if (itemReverse == false)
        {
            if (coroutineStart1 == false)
            {
                StartCoroutine("ItemReverse");
                Debug.Log("ItemReverse!!!");
            }
            else if (coroutineStart1 == true)
            {
                StopCoroutine("ItemReverse");
                itemreverseTime = 0;
                coroutineStart1 = false;
                StartCoroutine("ItemReverse");
                //Debug.Log("StopCoroutine");
            }
        }
        else if (GetInstance().itemReverse == true)
        {
            if (coroutineStart1 == false)
            {
                StartCoroutine("ItemReverse");
                //Debug.Log("ItemReverse!!!");
            }
            else if (coroutineStart1 == true)
            {
                StopCoroutine("ItemReverse");
                itemreverseTime = 0;
                coroutineStart1 = false;
                StartCoroutine("ItemReverse");
                //Debug.Log("StopCoroutine");
            }
        }
    }

    public void MagicStopItem()
    {
        if (magicStop == false)
        {
            if (coroutineStart2 == false)
            {
                StartCoroutine("MagicStop");
                Debug.Log("magicstop!!!");
            }
            else if (coroutineStart2 == true)
            {
                StopCoroutine("MagicStop");
                itemreverseTime = 0;
                coroutineStart2 = false;
                StartCoroutine("MagicStop");
                //Debug.Log("StopCoroutine");
            }
        }
        else if (magicStop == true)
        {
            if (coroutineStart2 == false)
            {
                StartCoroutine("MagicStop");
                //Debug.Log("ItemReverse!!!");
            }
            else if (coroutineStart2 == true)
            {
                StopCoroutine("MagicStop");
                magicstopTime = 0;
                coroutineStart2 = false;
                StartCoroutine("MagicStop");
                //Debug.Log("StopCoroutine");
            }
        }
    }

    public void MagicItem(int _idx)
    {
        switch (_idx)
        {
            case 1:
                magic += 50f;
                break;
            case 2:
                magic -= 50f;
                break;
        }
    }

    public void Damage()
    {
        mist.RandomPos();
        gameObject.layer = 3;
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
        isDamaged = true;
        StartCoroutine("DamageEffect");
        //Invoke("DamageOff", 4f);
        time += Time.deltaTime;
    }
    public void DamageOff()
    {
        gameObject.layer = 0;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    IEnumerator DamageEffect()
    {
        Debug.Log("start~!!!!!!");
        time = 0;
        if (time < 2.5f)
        {
            while (isDamaged == true)
            {
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                if (time >= 2.5f)
                {
                    time = 0f;
                    isDamaged = false;
                    DamageOff();
                }
            }
        }
    }

    IEnumerator ItemReverse()
    {
        itemreverseTime = 0;
        coroutineStart1 = true;

        if (itemreverseTime < 10f)
        {
            GameManager.GetInstance().itemReverse = true;
            yield return new WaitForSeconds(10f);
            GameManager.GetInstance().itemReverse = false;
            itemreverseTime = 0;
            coroutineStart1 = false;
            Debug.Log("코루틴끝!!!!!!!!!!!!");
        }
    }

    IEnumerator MagicStop()
    {
        gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        magicstopTime = 0;
        coroutineStart2 = true;

        if (magicstopTime < 4f)
        {
            GameManager.GetInstance().magicStop = true;
            yield return new WaitForSeconds(4f);
            GameManager.GetInstance().magicStop = false;
            magicstopTime = 0;
            coroutineStart2 = false;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            Debug.Log("코루틴끝!!!!!!!!!!!!");
        }
    }





    #endregion

    #region 맵 에디터
    public MapEditor mapEditor;

    void MapStart()
    {
        mapEditor.MapEditorInit();
    }

    void MapUpdate()
    {
        mapEditor.moveojb();
        mapEditor.pulling();
    }
   
    void MapReset()
    {
        
    }

    #endregion

    
}
