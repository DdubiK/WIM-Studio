using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour
{
    public List<GameObject> mistPosition = new List<GameObject>();
    public List<GameObject> makePosition = new List<GameObject>();
    public int min = 0;
    public int max = 4;
    public float count = 0;
    public float mistTime;
    public bool addList;
    public bool coroutineStart;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] mist = GameObject.FindGameObjectsWithTag("mist");
        for(int i = 0; i < mist.Length; i++)
        {
            mistPosition.Add(mist[i]);
            Debug.Log(mist[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MistTimer();
    }

    public void RandomPos()
    {
        int currentNumber = Random.Range(min, max);
        while (count == 0f)
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
                StartCoroutine("RemoveMist", addMist);
                Debug.Log("AddList");
                count++;
            }
        }
        count = 0;
    }
    public void MistTimer()
    {
        mistTime += Time.deltaTime;
    }
    IEnumerator RemoveMist(GameObject addmist)
    {
        mistTime = 0;
        coroutineStart = true;

        if (mistTime < 4f)
        {
            addmist.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            //addmist.gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            mistTime = 0;
            coroutineStart = false;
            addmist.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            int idx = makePosition.IndexOf(addmist);
            makePosition.RemoveAt(idx);
            //addmist.gameObject.SetActive(false);
            Debug.Log("***mistcoroutimeEnd***");
        }
    }

}
