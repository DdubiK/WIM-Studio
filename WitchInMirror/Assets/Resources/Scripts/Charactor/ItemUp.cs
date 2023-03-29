using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUp : MonoBehaviour
{
    public float time;
    public float speed;
    public float getSpeed;
    public float upMagic = 10f;
    public GameObject player;
    public bool isGet;
    public bool isBack;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        GetSystem();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<GameObject>();
        if(collision.gameObject.tag == "Player")
        {
            Get();
            Pooling();
        }
        if(collision.gameObject.tag == "Player" && isBack == true)
        {
            //<<<<<<< HEAD
            MagicUpDown();

//=======
//GameManager.GetInstance().magic += upMagic;
//Destroy(gameObject);
//>>>>>>> origin/main
        }
    }
    public void Pooling()
    {
        var effect = GameManager3.GetEffect();
        var transform = this.transform.position;
        effect.make(transform);
    }
    public void MagicUpDown()
    {
        if (GameManager.GetInstance().itemReverse == false)
        {
            if (GameManager.GetInstance().magicStop == false)
            {
                //GameManager.GetInstance().magic += upMagic;
                Debug.Log("+magic");
            }
            Destroy(gameObject);
        }
        else
        {
            if (GameManager.GetInstance().magicStop == false)
            {
                //GameManager.GetInstance().magic -= upMagic;
                Debug.Log("-magic");
            }
            Destroy(gameObject);
        }
    }

    public void Get()
    {
        getSpeed = 3f;
        speed = 0f;
        time = 0f;
        isGet = true;
    }
    public void Back()
    {
        isGet = false;
        isBack = true;
        time = 0f;
    }
    public void GetSystem()
    {
        getSpeed -= Time.deltaTime;
        time += Time.deltaTime;
        transform.position += Vector3.left * speed * Time.deltaTime;
        if (isGet)
        {
            if (time < 0.1f) transform.position += Vector3.right * getSpeed * Time.deltaTime;
            else
            {
                Back();
            }
        }
        if (isBack)
        {
            if (time < 0.1f) transform.position += new Vector3(1f, 0.5f, 0) * getSpeed * 1.5f * Time.deltaTime;
            else
            {
                player = GameManager.GetInstance().player[0];
                Vector3 dist = player.transform.position - this.transform.position;
                Vector3 dir = dist.normalized;
                float fdist = dist.magnitude;
                transform.position += dir * getSpeed * Time.deltaTime;
            }
        }
        //if (GameManager.GetInstance().magicStop == false) upMagic = 10f;
        //else upMagic = 0f;

    }
}
