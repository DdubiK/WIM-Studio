using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.IO;


//DAO(Data Access Object) DataBase�� �����ϱ� ���� ������ ���� ��ü
public class DBLoader : MonoBehaviour
{
    //DTO(Data Transfer Object) ������ �����͸� ��ȯ�ϱ� ���� �߰� ��ü (DB -> CharDataList -> GameCharactor)
    public static List<CharData> CharDataList = new List<CharData>();

    public static MapPatternData MapPatternArray = new MapPatternData();



    private void Awake()
    {
        ReadData();
    }



    #region ��ī��, ���� ID �˻�
    // �ڱ��ڽ��� ����ȭ�� ���� ī�Ǹ� �Ͽ� ������ �����͸� �����ϰ� �о��.
    static T DeepCopy<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var tmp = JsonConvert.DeserializeObject<T>(json);

        return tmp;
    }

    // ���� ���� ����Ͽ� List�� ������ Class ���� �� ID���� ��ġ�ϴ� ������ ��ī�Ƿ� �����Ͽ� �����ϵ��� �Ѵ�.
    public static CharData GetCharStatusByIdx(int idx)
    {
        return DeepCopy(CharDataList.Find(x => x.ID == idx));
    }
    #endregion

    #region ������ �о����
    public void ReadData()
    {
        if (Resources.Load<TextAsset>("DataBase/CharDataTable") != null)
        {
            string jdata = Resources.Load<TextAsset>("DataBase/CharDataTable").text;

            CharDataList = JsonConvert.DeserializeObject<List<CharData>>(jdata);
        }
        else
        {
            Debug.LogError("Scan Failed");

        }

        if (Resources.Load<TextAsset>("DataBase/MapDataTable") != null)
        {
            string jdata = Resources.Load<TextAsset>("DataBase/MapDataTable").text;


            MapPatternArray = JsonConvert.DeserializeObject<MapPatternData>(jdata);
           
        }
        else
        {
            Debug.LogError("Scan Failed");

        }




    }

    #endregion
}
