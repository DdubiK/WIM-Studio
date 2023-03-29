using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void make(Vector3 _transform)
    {
        this.transform.position = _transform;
        Invoke("DestroyEffect", 1f);
        
    }
    public void DestroyEffect()
    {
        GameManager3.ReturnObject(this);
    }

}
