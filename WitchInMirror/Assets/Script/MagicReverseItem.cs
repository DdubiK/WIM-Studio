using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicReverseItem : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.5f;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
