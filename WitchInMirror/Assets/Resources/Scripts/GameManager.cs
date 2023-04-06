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
        SetResolutions();
        SLManager.RemoteStart();
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

    public float InGameTimeScale = 0;

    public void EventSenceChange(int idx)
    {
        //Debug.Log("check");
        SetGUIState((E_SCENE)idx);
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
                isPause = false;
                Time.timeScale = 0;
                pausePanel.gameObject.SetActive(false);
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
                SLManager._load();
                if (Score > SLManager.Score)
                {
                   
                    SLManager.Score = Score;
                    SLManager._save();
                    
                }
                textHighScore.text = "" + SLManager.Score;
                textScore.text = "" + (Score+Mathf.Round(distance) *100f);
                //SceneUpdate -= UIUpdate;
                SceneUpdate -= CharUpdate;
                SceneUpdate -= MapUpdate;
                if (magic == 0)
                    gameoverchar.gameObject.SetActive(true);
                else
                    gameoverchar.gameObject.SetActive(false);
                if (magic == 800)
                    gameoverchar2.gameObject.SetActive(true);
                else
                    gameoverchar2.gameObject.SetActive(false);
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


    public void SetResolutions()
    {
        int setWidth = 3200; // 사용자 설정 너비
        int setHeight = 1440; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);
        // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }



    #endregion


    #region UI
    [Header("UI")]
    public List<GameObject> listGUIScenes;
    private bool isPause = false;
    public GameObject HelpPanel;
    public GameObject HelpPlayPanel;
    public GameObject pausePanel;
    public GameObject objCanvas;
    public GameObject danger;
    public GameObject mapEffect1;
    public GameObject mapEffect4;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textHighScore;
    public TextMeshProUGUI dis_text;
    public GameObject gameoverchar;
    public GameObject gameoverchar2;

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
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/GetMagic0"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/GetMagic1"));
        audioClip.Add(Resources.Load<AudioClip>("Audio/Sources/GetMagic2"));
        audioSource.clip = audioClip[0];

        audioSource.clip = bgmSource.clip;
        //UIratio();

        SFXSlider.value = 0.5f;
        BGMSlider.value = 0.5f;

    }

    void UIUpdate()
    {
        MagicUI();
        //UpdateGUIState();
        MagicLow();
        distanceUI();
    }

    public void SetHelp()
    {
        HelpPanel.gameObject.SetActive(true);
    }

    public void HelpBack()
    {
        HelpPanel.gameObject.SetActive(false);
    }

    public void SetPlayHelp()
    {
        HelpPlayPanel.gameObject.SetActive(true);
    }

    public void HelpPlayBack()
    {
        HelpPlayPanel.gameObject.SetActive(false);
    }

    public void SoundPlay(int soundnumber)
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



    public void MagicLow()
    {
        if (magic <= 120 || magic >= 680)
        {
            danger.SetActive(true);
            if (magic <= 120) mapEffect4.SetActive(true);
            if (magic >= 680) mapEffect1.SetActive(true);
        }
        else
        {
            danger.SetActive(false);
            mapEffect1.SetActive(false);
            mapEffect4.SetActive(false);
        }
    }

    public void SetPause()
    {
        if (!isPause)
        {
            isPause = true;
            InGameTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
            pausePanel.gameObject.SetActive(true);
        }
    }

    public void PauseContinue()
    {
        if (isPause)
        {
            isPause = false;
            Time.timeScale = InGameTimeScale;
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

    public void distanceUI()
    {
        distance += Time.deltaTime*Time.timeScale *2f;
        dis_text.text = Mathf.Round(distance) + "m";
    }


    public void reStart()
    {
        SceneUpdate -= CharUpdate;
        SceneUpdate -= MapUpdate;
    }



    #endregion


    #region 캐릭터, 마력, 시스템
    [Header("캐릭터")]
    public GameObject[] player = new GameObject[2];
    public float jump = 3f;
    public int Score = 0;
    public bool isGround;
    public bool jumpUp;
    public Vector3 disToGround;
    public Vector3 disToJumpPos;
    public Action GroundCheck = null;
    public int jumpCount = 0;
    public int HighScore = 0;

    [Header("마력")]
    public float Maxmagic = 400;
    public float Minmagic = 0;
    public float magic;
    public float upMagic = 10f;
    //public float magicstopTime;
    public bool magicReverse;
    public bool magicStop;
    public float magicDecreasePer = 15f;

    [Header("시스템")]
    //public float time;
    public float playtime;
    public bool itemReverse;
    //public float itemreverseTime;
    public float distance;

    //스테이지 레벨 증가
    public float StageLvTimer = 0;
    public float StageLvTime;
    public int StageLv = 0;


    void CharStart()
    {
        magic = 400f;
        Maxmagic = 800;
        Minmagic = 0;
        Score = 0;
        StageLvTime = 30;

        jumpCount = 0;

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
        LvUPTimer();
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
        //Debug.Log(Vector2.Distance(player[0].transform.position, disToGround));
        //Debug.Log(Vector2.Distance(player[0].transform.position, disToJumpPos));

        float disGround = 0.25f;
        if (isGiant)
        {
            disGround = 0.5f;
        }

        if (!isGround && jumpCount > 0 && Vector2.Distance(player[0].transform.position, disToJumpPos) < 0.2f)
        {
            jumpUp = false;
            //Debug.Log(Vector2.Distance(player[0].transform.position, disToJumpPos));
        }
        if (!isGround && !jumpUp &&jumpCount>0&& Vector2.Distance(player[0].transform.position, disToGround) <= disGround)
        {

            isGround = true;
            jumpCount = 0;
            GroundCheck -= GroundChecking;
            return;
        }
    }


    public void Jump()
    {
        if (isGround)
        {

            player[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, jump );
            player[1].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -jump);
            isGround = false;
            jumpUp = true;
            GroundCheck += GroundChecking;
            jumpCount++;
            SoundPlay(2);
        }
        if (!isGround && !jumpUp && jumpCount==1 && !isGiant && GroundCheck == GroundChecking)
        {

            player[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, jump * 0.7f);
            player[1].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -jump *0.7f);
            jumpCount++;

            SoundPlay(2);
        }
    }

    public void timer()
    {
        //time += Time.deltaTime;
        playtime += Time.deltaTime / Time.timeScale;
        //itemreverseTime += Time.deltaTime;
        //magicstopTime += Time.deltaTime;
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
        UpScore(20);
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
        UpScore(20);
    }



    //마력 감소율 시간에 따라 증가
    public void Magic()
    {
        if (magicStop == false)
        {
            if (magicReverse)
            {
                magic += Time.deltaTime * magicDecreasePer * (1 + StageLv * 0.1f);
            }
            else magic -= Time.deltaTime * magicDecreasePer * (1 + StageLv * 0.1f);
        }
        MagicCheck();
    }

    //시간에따른 스테이지 레벨 증가
    public void LvUPTimer()
    {
        if (StageLvTimer >= StageLvTime)
        {
            if(StageLv >=10)
            {
                StageLvTimer = 0;
                StageLv = 10;
                magicDecreasePer += 5;
            }
            else
            {
                StageLvTimer = 0;
                StageLv++;
            }
            Time.timeScale = 1 + (StageLv * 0.04f); // 이것보다 작아야된다.
            InGameTimeScale = Time.timeScale;
        }
        else
            StageLvTimer += Time.deltaTime;
    }


    public void resetCharState()
    {
        magic = 400f;
        Maxmagic = 800;
        Minmagic = 0;
        isGround = true;
        jumpUp = false;
        isGiant = false;
        GroundCheck = null;
        magicDecreasePer = 15f;
        InGameTimeScale = 1;
        Score = 0;
        playtime = 0;
        StageLvTimer = 0;
        StageLv = 0;
        distance = 0;
        resetInit();
    }


    #endregion

    #region 아이템 효과
    [Header("아이템 획득 효과")]
    public Mist mist;
    public bool isDamaged;
    public bool isShield;
    public bool isGiant = false;
    //public bool coroutineStart1;//ItemReverseCoroutineStart
    public bool coroutineStart2;//MagicStopCoroutineStart

    public void resetInit()
    {

        //기존 장애물 충돌로 안개 생성 시 없애기
        mist.mistInit();

        //무적 상태 시 없애기
        isDamaged = false;

        //쉴드 적용 시 없애기
        isShield = false;
        player[0].gameObject.transform.Find("Shield").gameObject.SetActive(false);
        player[1].gameObject.transform.Find("Shield").gameObject.SetActive(false);

        //체질 변화 원래대로
        magicReverse = false;
        StopCoroutine("MagicReverse");

        //아이템 성질 변화 원래대로
        itemReverse = false;
        StopCoroutine("ItemReverse");

        //거대화 원래대로
        magicStop = false;
        player[0].gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        player[1].gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        player[0].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        player[1].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        StopCoroutine("MagicStop");
        //player[0].gameObject.transform.Find("SuperMode2").gameObject.SetActive(false);
        //player[1].gameObject.transform.Find("SuperMode2").gameObject.SetActive(false);


    }
    public void BroomstickItem()
    {
        magic = 400;
    }
    public void ShieldItem()
    {
        if (magicStop == false)
        {
            player[0].transform.GetChild(0).gameObject.SetActive(true);
            player[1].transform.GetChild(0).gameObject.SetActive(true);
            isShield = true;            
        }
    }

    public void Damaged()
    {
        if (magicStop == false)
        {
            if (isShield)
            {
                player[0].transform.GetChild(0).gameObject.SetActive(false);
                player[1].transform.GetChild(0).gameObject.SetActive(false);
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
        StopCoroutine("MagicReverse");
        StartCoroutine("MagicReverse");
        //if (magicReverse == false)
        //{
        //    Debug.Log("Reverse!!!");
        //    magicReverse = true;
        //}
        //else
        //{
        //    Debug.Log("Return!!!");
        //    magicReverse = false;
        //}
    }

    public void ItemReverseItem()
    {
        StopCoroutine("ItemReverse");
        StartCoroutine("ItemReverse");
        //if (itemReverse == false)
        //{
        //    if (coroutineStart1 == false)
        //    {
        //        StartCoroutine("ItemReverse");
        //        Debug.Log("ItemReverse!!!");
        //    }
        //    else if (coroutineStart1 == true)
        //    {
        //        StopCoroutine("ItemReverse");
        //        //itemreverseTime = 0;
        //        coroutineStart1 = false;
        //        StartCoroutine("ItemReverse");
        //        //Debug.Log("StopCoroutine");
        //    }
        //}
        //else if (GetInstance().itemReverse == true)
        //{
        //    if (coroutineStart1 == false)
        //    {
        //        StartCoroutine("ItemReverse");
        //        //Debug.Log("ItemReverse!!!");
        //    }
        //    else if (coroutineStart1 == true)
        //    {
        //        StopCoroutine("ItemReverse");
        //        //itemreverseTime = 0;
        //        coroutineStart1 = false;
        //        StartCoroutine("ItemReverse");
        //        //Debug.Log("StopCoroutine");
        //    }
        //}
    }

    public void MagicStopItem()
    {
        StopCoroutine("MagicStop");
        StartCoroutine("MagicStop");
        //if (magicStop == false)
        //{
        //    if (coroutineStart2 == false)
        //    {
        //        StartCoroutine("MagicStop");
        //        Debug.Log("magicstop!!!");
        //    }
        //    else if (coroutineStart2 == true)
        //    {
        //        StopCoroutine("MagicStop");
        //        //itemreverseTime = 0;
        //        coroutineStart2 = false;
        //        StartCoroutine("MagicStop");
        //        //Debug.Log("StopCoroutine");
        //    }
        //}
        //else if (magicStop == true)
        //{
        //    if (coroutineStart2 == false)
        //    {
        //        StartCoroutine("MagicStop");
        //        //Debug.Log("ItemReverse!!!");
        //    }
        //    else if (coroutineStart2 == true)
        //    {
        //        StopCoroutine("MagicStop");
        //        magicstopTime = 0;
        //        coroutineStart2 = false;
        //        StartCoroutine("MagicStop");
        //        //Debug.Log("StopCoroutine");
        //    }
        //}
    }

    public void MagicItem(int _idx)
    {
        switch (_idx)
        {
            case 1:
                magic += 100f;
                break;
            case 2:
                magic -= 100f;
                break;
        }
    }

    public void Damage()
    {
        mist.RandomPos();
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
        isDamaged = true;
        StartCoroutine("DamageEffect");
        //Invoke("DamageOff", 4f);
        //time += Time.deltaTime;
    }
    public void DamageOff()
    {
        isDamaged = false;
        player[0].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        player[1].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    IEnumerator DamageEffect()
    {
        Debug.Log("start~!!!!!!");
        float timer = 0;
        while (isDamaged)
        {
            timer += Time.deltaTime;
            if (timer < 2.5f)
            {
                timer += 0.1f;
                player[0].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
                player[1].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
                yield return new WaitForSeconds(0.1f / Time.timeScale);
                timer += 0.1f;
                player[0].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                player[1].gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                yield return new WaitForSeconds(0.1f / Time.timeScale);
            }
            else
            {
                DamageOff();
                break;
            }
        }
    }

    IEnumerator ItemReverse()
    {
        itemReverse = true;
        yield return new WaitForSeconds(10f / Time.timeScale);
        itemReverse = false;
        //Debug.Log("코루틴끝!!!!!!!!!!!!");

    }
    IEnumerator MagicReverse()
    {
        Debug.Log("코루틴 시작");
        switch (magicReverse)
        {
            case true:
                magicReverse = false;
                break;
            case false:
                magicReverse = true;
                break;
        }
        yield return new WaitForSeconds(10f / Time.timeScale);

        magicReverse = false;
        Debug.Log("코루틴 끝");

    }

    IEnumerator MagicStop()
    {
        Debug.Log("코루틴 시작");
        magicStop = true;
        isGiant = true;
        player[0].gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        player[1].gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        player[0].gameObject.transform.GetChild(1).gameObject.SetActive(true);
        player[1].gameObject.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(8f /Time.timeScale);
        magicStop = false;
        isGiant = false;
        player[0].gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        player[1].gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        player[0].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        player[1].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        Debug.Log("코루틴 끝");
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
