using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager3 : MonoBehaviour
{
    public static GameManager3 GetInstance() { return instance; }
    public static GameManager3 instance = null;

    public GameObject player1;
    public GameObject player2;
    public float magic;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!instance) instance = this;
    }
    void Start()
    {
        
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
