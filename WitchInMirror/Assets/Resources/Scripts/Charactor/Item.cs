using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float speed;
    public GameObject IdleEffect;
    public GameObject getItem;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.3f;
        GameObject effect = Instantiate(IdleEffect);
        effect.transform.position = this.gameObject.transform.position;
        effect.transform.parent = this.gameObject.transform;
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
            GameObject effect = Instantiate(getItem);
            effect.transform.position = this.gameObject.transform.position;
            effect.transform.parent = this.gameObject.transform;
            Destroy(gameObject, 0.2f);
        }
    }
}
