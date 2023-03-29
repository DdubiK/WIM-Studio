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
        CharStart();
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();
        CharUpdate();
    }

    #region UI
    [Header("씬")]
    public List<GameObject> listGUIScenes;
    public enum E_SCENE { PLAY, TITLE, GAMEOVER, MAX }
    public E_SCENE curScene;

    public GameObject pausePanel;
    private bool isPause = false;

    [Header("사운드")]
    public AudioMixer audioMixer;
    public Slider BgmSlider;

    public GameObject bar;
    public GameObject magicbar;


    void UIStart()
    {

    }

    void UIUpdate()
    {
        MagicUI();
        UpdateGUIState();
    }

    public void Initialize(GameManager gameManager)
    {
        gameObject.SetActive(true);
        SetGUIState(curScene);
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

        float magic_x = ((magic - Minmagic) / (Maxmagic - Minmagic)) * rectTransform.rect.width + (rectTransform.anchoredPosition.x - rectTransform.rect.width / 2);
        //Debug.Log(magic_x);
        rectTransform1.anchoredPosition = new Vector2(magic_x, rectTransform1.anchoredPosition.y);

    }

    #endregion

    #region 캐릭터, 마력
    [Header("캐릭터")]
    public GameObject[] player = new GameObject[2];
    public float jump = 3f;
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



    //거리로 점프 체크
    public void GroundChecking()
    {
        //distanc로 거리 체크 하여 설정
        //Debug.Log(Vector3.Distance(player[0].transform.position, disToGround));
        //Debug.Log(Vector3.Distance(player[0].transform.position, disToJumpPos));

        if (!isGround && Vector3.Distance(player[0].transform.position, disToJumpPos) < 0.1f)
        {
            jumpUp = false;
        }

        if (!isGround && !jumpUp && Vector3.Distance(player[0].transform.position,disToGround)<=0.2f)
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
        }
    }


    public void timer()
    {
        time += Time.deltaTime;
        playtime += Time.deltaTime;
        itemreverseTime += Time.deltaTime;
        magicstopTime += Time.deltaTime;
    }



    public void MagicUpDown()
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
    }


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

    #region 추가용


    #endregion

}
