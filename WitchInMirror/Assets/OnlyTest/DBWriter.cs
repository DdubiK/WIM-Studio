using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DBWriter : MonoBehaviour
{
    public TextMeshProUGUI TextMesh;

    public int DBNUM=0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextMesh.text = DBLoader.CharDataList[DBNUM].Name;
    }


    private void OnGUI()
    {
       
    }
}
