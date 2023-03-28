using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]//직렬화 : 스크립트가 씬에 존재하지 않아도 직렬로 연결되어 있기 때문에 Public으로 정의하면 읽어올 수 있다.
public class CharData// 임시 캐릭터 클래스
{
    public int ID;
    public string Name;
    public string PFL;
    public int TYP;
    public int HP;
    public int MP;
    public int SPD;
    public int ATK;
    public int DEF;


    public CharData()
    {
        ID = 0;
        Name = "";
        PFL = "";
        TYP = 0;
        HP = 0;
        MP = 0;
        SPD = 0;
        ATK = 0;
        DEF = 0;
    }

}

[System.Serializable]
public class MapPatternData
{
    public List<int[]> Pattern { get; set; }
}


[System.Serializable]
public class RuningObject{
    public int ID;
    public GameObject Obj;
    public bool colcheck = false;
    public GameObject effect;
    //int[,] ID =new int [6,6];

    //List<int[,]> a = new List<int[,]>();


}

