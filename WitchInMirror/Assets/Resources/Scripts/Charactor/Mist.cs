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
    public void mistInit() //ó�� �Ȱ� ����
    {
        GameObject[] mist = GameObject.FindGameObjectsWithTag("mist");
        for (int i = 0; i < mist.Length; i++)
        {
            mistPosition.Add(mist[i]);
            mist[i].gameObject.SetActive(false);
        }
    }
    public void mistRestart() //�ʱ�ȭ ��, �����Ǿ� �ִ� �Ȱ� ����
    {
        StopCoroutine("RemoveMist");
        for (int i = 0; i < makePosition.Count; i++)
        {
            makePosition[i].gameObject.SetActive(false);
            makePosition.RemoveAt(i);
        }
        count = 0;
    }
 
    public void RandomPos() //�Ȱ� ���� ���� 12�� �̻� �� ���� ������.
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
    IEnumerator RemoveMist(GameObject addmist) // �Ȱ� ���� �ڷ�ƾ, �ð����� ���� ���� , ���� �ð� ���� �� active false�� , ����Ʈ ����
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
