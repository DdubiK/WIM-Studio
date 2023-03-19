using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    public float moveSpeed;
    public float curDistance;
    public bool isMove;
    public float reachDistance;

    public void Active()
    {
        gameObject.SetActive(true);
        isMove = true;
        curDistance = 0;

    }
    public void InActive()
    {
        isMove = false;
        curDistance = 0;
        gameObject.SetActive(false);
    }
    public void Move()
    {
        float fmove = Time.deltaTime * moveSpeed;
        transform.position += fmove * Vector3.left;
        curDistance += fmove;
    }
    public void Stop()
    {

        InActive();
        //MapLvEditor.queUsePool.Dequeue

    }
    //public abstract 
    // Start is called before the first frame update
    void Start()
    {
        //InActive();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            Move();
        }
        if (curDistance >= reachDistance)
        {
            Stop();
            if (MapLvEditor.queActivePool.Count > 0)
            {
                ObstacleBase obstaclebase = MapLvEditor.queActivePool.Dequeue();
                MapLvEditor.queInActivePool.Enqueue(obstaclebase);
            }
        }
    }
}
