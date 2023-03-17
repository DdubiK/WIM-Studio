using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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