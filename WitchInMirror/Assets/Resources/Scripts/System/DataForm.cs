using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum E_SCENE { TITLE,PLAY,GAMEOVER, MAX }


[System.Serializable]//����ȭ : ��ũ��Ʈ�� ���� �������� �ʾƵ� ���ķ� ����Ǿ� �ֱ� ������ Public���� �����ϸ� �о�� �� �ִ�.
public class CharData// �ӽ� ĳ���� Ŭ����
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


[System.Serializable]
public struct Obstacle
{
    public GameObject obj;
    public int tr_idx;
    public Vector3 Spawner_tr;

    public Vector3 SetTr(int _tr_idx)
    {
        switch (_tr_idx)
        {
            case 0:
                Spawner_tr.y = 0.9f;
                Spawner_tr.x = 3.0f;
                break;
            case 1:
                Spawner_tr.y = 0.2f;
                Spawner_tr.x = 3.0f;
                break;
            case 2:
                Spawner_tr.y = -0.2f;
                Spawner_tr.x = 3.0f;
                break;
            case 3:
                Spawner_tr.y = -0.9f;
                Spawner_tr.x = 3.0f;
                break;
        }
        return Spawner_tr;
    }
}


