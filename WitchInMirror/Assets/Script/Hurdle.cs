using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    void Start()
    {
        //speed = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveSystem();
    }
    public void MoveSystem()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
