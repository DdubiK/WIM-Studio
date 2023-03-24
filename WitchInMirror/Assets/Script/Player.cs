using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float time;
    public float playtime;
    public float magic;
    public float jump = 3f;
    public float itemreverseTime;
    public bool isGround;
    public bool isDamaged;
    public bool isShield;
    public bool coroutineStart;

    // Start is called before the first frame update
    void Start()
    {
        isGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer();
    }
    public void Jump1()
    {
        if (isGround)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, jump);
        }
    }
    public void Jump2()
    {
        if (isGround)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -jump);
        }
    }
    public void timer()
    {
        time += Time.deltaTime;
        playtime += Time.deltaTime;
        itemreverseTime += Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGround = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "hurdle")
        {
            if(isShield)
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
        if (other.gameObject.tag == "shield")
        {
            gameObject.transform.Find("Shield").gameObject.SetActive(true);
            isShield = true;
        }
        if (other.gameObject.tag == "magicreverse")
        {
            if (GameManager3.GetInstance().magicReverse == false)
            {
                Debug.Log("Reverse!!!");
                GameManager3.GetInstance().magicReverse = true;
            }
            else
            {
                Debug.Log("Return!!!");
                GameManager3.GetInstance().magicReverse = false;
            }
        }
        if(other.gameObject.tag == "itemreverse")
        {
            if (GameManager3.GetInstance().itemReverse == false)
            {
                if (coroutineStart == false)
                {
                    StartCoroutine("ItemReverse");
                    Debug.Log("ItemReverse!!!");
                }
                else if(coroutineStart == true)
                {
                    StopCoroutine("ItemReverse");
                    itemreverseTime = 0;
                    coroutineStart = false;
                    StartCoroutine("ItemReverse");
                    Debug.Log("StopCoroutine");
                }
            }
            else if (GameManager3.GetInstance().itemReverse == true)
            {
                if (coroutineStart == false)
                {
                    StartCoroutine("ItemReverse");
                    Debug.Log("ItemReverse!!!");
                }
                else if (coroutineStart == true)
                {
                    StopCoroutine("ItemReverse");
                    itemreverseTime = 0;
                    coroutineStart = false;
                    StartCoroutine("ItemReverse");
                    Debug.Log("StopCoroutine");
                }
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGround = false;
        }
    }

    public void Damage()
    {
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
                //yield return new WaitForSeconds(0.1f);
                
                if(time >= 2.5f)
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
        coroutineStart = true;

        if (itemreverseTime < 4f)
        {
            GameManager3.GetInstance().itemReverse = true;
            yield return new WaitForSeconds(4f);
            GameManager3.GetInstance().itemReverse = false;
            itemreverseTime = 0;
            coroutineStart = false;
            Debug.Log("内风凭场!!!!!!!!!!!!");
        }

        //else if (itemreverseTime >= 3f)
        //{
        //    Debug.Log("内风凭场!!!!!!!!!!!!");
        //    itemreverseTime = 0;
        //    GameManager3.GetInstance().itemReverse = false;
        //}


    }
}