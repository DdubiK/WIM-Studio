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
    public float magicstopTime;
    public bool isGround;
    public bool isDamaged;
    public bool isShield;
    public bool coroutineStart1;//ItemReverseCoroutineStart
    public bool coroutineStart2;//MagicStopCoroutineStart

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
        magicstopTime += Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGround = true;
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
    public void ShieldItem()
    {
        if (coroutineStart2 == false)
        {
            gameObject.transform.Find("Shield").gameObject.SetActive(true);
            isShield = true;
        }
    }
    public void MagicReverseItem()
    {
        if (GameManager.GetInstance().magicReverse == false)
        {
            Debug.Log("Reverse!!!");
            GameManager.GetInstance().magicReverse = true;
        }
        else
        {
            Debug.Log("Return!!!");
            GameManager.GetInstance().magicReverse = false;
        }
    }
    public void ItemReverseItem()
    {
        if (GameManager.GetInstance().itemReverse == false)
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
        else if (GameManager.GetInstance().itemReverse == true)
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
        if (GameManager.GetInstance().magicStop == false)
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
        else if (GameManager.GetInstance().magicStop == true)
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
    private void OnTriggerEnter2D(Collider2D other)
    {

        //if (coroutineStart2 == false)
        //{
            if (other.gameObject.tag == "hurdle")
            {
                Damaged();
            }
        //}
        //if (coroutineStart2 == false)
        //{
            if (other.gameObject.tag == "shield")
            {
                ShieldItem();
            }
        //}

        if (other.gameObject.tag == "magicreverse")
        {
            MagicReverseItem();
        }

        if (other.gameObject.tag == "itemreverse")
        {
            ItemReverseItem();
        }
        if (other.gameObject.tag == "magicstop")
        {
            MagicStopItem();
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
        GameManager3.GetInstance().mist.RandomPos();
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
            Debug.Log("内风凭场!!!!!!!!!!!!");
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
            Debug.Log("内风凭场!!!!!!!!!!!!");
        }
    }
}

