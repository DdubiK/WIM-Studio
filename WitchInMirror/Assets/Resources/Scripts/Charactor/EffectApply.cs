using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectApply : MonoBehaviour
{
    public GameObject supermodeMap;
    public GameObject gameOver;
    public float magicstopTime;
    public bool coroutineStart2;
    public float effectTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "magicstop")
        {
            StartCoroutine("MagicStop");
        }
    }
    IEnumerator MagicStop()
    {
        GameObject effect = Instantiate(supermodeMap);
        effect.transform.position = Camera.main.transform.position;
        gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        gameObject.transform.Find("SuperMode2").gameObject.SetActive(true);
        magicstopTime = 0;
        coroutineStart2 = true;
        GameManager.GetInstance().magicStop = true;

        while (magicstopTime < 10f)
        {
            magicstopTime++;
            yield return new WaitForSeconds(1f);
        }
        GameManager.GetInstance().magicStop = false;
        magicstopTime = 0;
        coroutineStart2 = false;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.transform.Find("SuperMode2").gameObject.SetActive(false);
        Destroy(effect);
        Debug.Log("코루틴끝!!!!!!!!!!!!");
    }
}
