using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour
{
    public List<GameObject> mistPosition = new List<GameObject>();
    public List<GameObject> makePosition = new List<GameObject>();
    public int min = 0;
    public int max = 12;
    public float count = 0;
    //public float mistTime;
    public bool addList;
    //public bool coroutineStart;


    // Start is called before the first frame update
    void Start()
    {
        mistInit();
    }
    public void mistInit() //처음 안개 생성
    {
        GameObject[] mist = GameObject.FindGameObjectsWithTag("mist");
        for (int i = 0; i < mist.Length; i++)
        {
            mistPosition.Add(mist[i]);
            mist[i].gameObject.SetActive(false);
        }
    }
    public void mistRestart() //초기화 시, 생성되어 있는 안개 리셋
    {
        StopCoroutine("RemoveMist");
        for (int i = 0; i < makePosition.Count; i++)
        {
            makePosition[i].gameObject.SetActive(false);
            makePosition.RemoveAt(i);
        }
        count = 0;
    }
 
    public void RandomPos() //안개 랜덤 생성 12개 이상 시 빠져 나오기.
    {
        if (count > max)
        {
            return;
        }
        else
        {
            int currentNumber = Random.Range(min, max);
            while (count < max)
            {
                //currentNumber = Random.Range(min, max);
                if (makePosition.Contains(mistPosition[currentNumber]))
                {
                    currentNumber = Random.Range(min, max);
                }
                else
                {
                    makePosition.Add(mistPosition[currentNumber]);
                    GameObject addMist = mistPosition[currentNumber];
                    addMist.SetActive(true);
                    StartCoroutine("RemoveMist", addMist);
                    count++;
                    break;
                }
            }
        }
    }
    IEnumerator RemoveMist(GameObject addmist) // 안개 생성 코루틴, 시간마다 투명도 감소 , 일정 시간 지날 시 active false와 , 리스트 제거
    {
        float mistTime = 0;
        addmist.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        while (true)
        {
            if (mistTime <= 8f)
            {               
                yield return new WaitForSeconds(1f);
                mistTime++;
                addmist.gameObject.GetComponent<SpriteRenderer>().color -= new Color(0f, 0f, 0f, 0.1f);
            }
            else
            {
                addmist.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                int idx = makePosition.IndexOf(addmist);
                makePosition.RemoveAt(idx);
                addmist.SetActive(false);
                count--;
                break;
            }
        }
    }

}
