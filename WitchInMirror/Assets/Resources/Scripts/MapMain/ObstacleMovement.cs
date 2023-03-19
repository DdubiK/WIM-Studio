using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float move_speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Movement()
    {
        transform.position += Vector3.left * Time.deltaTime * move_speed;
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
    }
}
