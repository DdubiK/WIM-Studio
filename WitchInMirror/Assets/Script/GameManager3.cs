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
    public bool magicReverse;
    public bool itemReverse;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!instance) instance = this;
    }
    void Start()
    {
        magic = 200f;
    }
   

    // Update is called once per frame
    void Update()
    {
        Magic();
    }
    public void Magic()
    {
        if(magicReverse)
        {
            magic += Time.deltaTime * 3f;
        }
        else magic -= Time.deltaTime * 3f;
    }
}
