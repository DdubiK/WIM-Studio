using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.IO;


//DAO(Data Access Object) DataBase에 접근하기 위한 로직을 가진 객체
public class DBLoader : MonoBehaviour
{
    //DTO(Data Transfer Object) 계층간 데이터를 교환하기 위한 중간 객체 (DB -> CharDataList -> GameCharactor)
    public static List<CharData> CharDataList = new List<CharData>();

    public static MapPatternData MapPatternArray = new MapPatternData();



    private void Awake()
    {
        ReadData();
    }



    #region 딥카피, 람다 ID 검색
    // 자기자신을 직렬화를 통한 카피를 하여 기존의 데이터를 안전하게 읽어낸다.
    static T DeepCopy<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var tmp = JsonConvert.DeserializeObject<T>(json);

        return tmp;
    }

    // 람다 식을 사용하여 List에 보관된 Class 정보 중 ID값이 일치하는 정보를 딥카피로 복사하여 리턴하도록 한다.
    public static CharData GetCharStatusByIdx(int idx)
    {
        return DeepCopy(CharDataList.Find(x => x.ID == idx));
    }
    #endregion

    #region 데이터 읽어오기
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
