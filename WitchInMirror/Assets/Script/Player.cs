using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float time;
    public float playtime;
    public float magic;
    public float jump = 3f;
    public bool isGround;
    public bool isDamaged;

    // Start is called before the first frame update
    void Start()
    {
        isGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        playtime += Time.deltaTime;
        magic -= 3f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGround = true;
        }
        //if (collision.gameObject.tag == "hurdle")
        //{
        //    Damage();
        //    Debug.Log("damage!");
        //}
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "hurdle")
        {
            Damage();
            Debug.Log("damage!");
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
        time = 0;
        if (time < 4f)
        {
            while (isDamaged == true)
            {
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                //yield return new WaitForSeconds(0.1f);
                
                if(time >= 4f)
                {
                    time = 0f;
                    isDamaged = false;
                    DamageOff();
                }
            }
        }
        //time = 0f;
    }
}

//이동거리->점수

//마력수치 지속적 감소 (이동 거리비례)

//루나가 마력 아이템 얻을 시 증가

//로나가 마력 아이템 먹을 시 감소