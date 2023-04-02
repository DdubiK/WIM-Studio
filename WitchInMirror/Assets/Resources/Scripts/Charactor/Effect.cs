using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    public float time;
    public float getSpeed;
    public bool isGet;
    public bool isBack;
    public Action action = null;
    public GameObject player;
    public GameObject idleeffect;
    public bool downCheck = false;

    void Update()
    {
        action?.Invoke();
    }

    public void make(Vector3 _transform)
    {
        this.transform.position = _transform;
        Invoke("DestroyEffect", 1f);
        
    }
    public void DestroyEffect()
    {
        GameManager.instance.mapEditor.ReturnObject(this);
    }


    public void Get()
    {
        getSpeed = 3f;
        time = 0f;
        isGet = true;
        action += getSystem;
        
    }



    public void Pooling()
    {
        var effect = GameManager.instance.mapEditor.GetEffect();
        var transform = this.transform.position;
        effect.make(transform);
    }
    public void Back()
    {
        isGet = false;
        isBack = true;
        time = 0f;
    }


    void getSystem()
    {
        getSpeed -= Time.deltaTime;
        time += Time.deltaTime;
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
            float d = 0.5f;
            if (downCheck)
                d = -0.5f;
                
            if (time < 0.1f) transform.position += new Vector3(1f, d, 0) * getSpeed * 1.5f * Time.deltaTime;
            else
            {

                Vector3 dist = player.transform.position - this.transform.position;
                Vector3 dir = dist.normalized;
                float fdist = dist.magnitude;
                transform.position += dir * getSpeed * Time.deltaTime;
            }

            if (Vector3.Distance(player.transform.position, this.transform.position) < 0.1f)
            {
                GameManager.instance.mapEditor.effectpoolingQueue.Enqueue(this);
                this.gameObject.SetActive(false);
            }
        }
    }
        
}
