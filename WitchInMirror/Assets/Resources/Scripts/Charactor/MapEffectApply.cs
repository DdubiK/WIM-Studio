using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEffectApply : MonoBehaviour
{
    public GameObject overMagic;
    public GameObject lackMagic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.GetInstance().magic >= 300f)
        {
            overMagic.gameObject.SetActive(true);
        }
        if(100f < GameManager.GetInstance().magic && GameManager.GetInstance().magic < 300f)
        {
            overMagic.gameObject.SetActive(false);
            lackMagic.gameObject.SetActive(false);
        }
        
        if (GameManager.GetInstance().magic <= 100f)
        {
            lackMagic.gameObject.SetActive(true);
        }

    }
}
