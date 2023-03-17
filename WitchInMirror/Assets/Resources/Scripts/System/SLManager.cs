using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.IO;

public class SLManager : MonoBehaviour
{

    private string m_sSaveFileDirectory;  // 저장할 폴더 경로
    private string m_sSaveFileName = "/InventoryData.json"; // 파일 이름

    public CharData data;


    // Start is called before the first frame update
    void Start()
    {
        RemoteStart();
    }

    #region 시작 원격 설정
    public void RemoteStart()
    {
        m_sSaveFileDirectory = Application.dataPath + "/Save/";
        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;

        if (!Directory.Exists(m_sSaveFileDirectory)) // 해당 경로가 존재하지 않는다면
            Directory.CreateDirectory(m_sSaveFileDirectory); // 폴더 생성(경로 생성)


        _load();
        _save();

    }

    #endregion

    #region 리셋 세이브 로드
    public void _reset()
    {

        //string jdata = JsonConvert.SerializeObject(gData, Formatting.Indented);
        //File.WriteAllText(Application.persistentDataPath + "/czSaveData.json", jdata);
        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;
        File.Delete(filecheck);
    }


    public void _save()
    {

        //List 못읽음
        string jdata = JsonUtility.ToJson(data);

        File.WriteAllText(m_sSaveFileDirectory + m_sSaveFileName, jdata);

    }

    public void _load()
    {



        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;


        //JObject
        //Debug.Log(File.Exists(filecheck) +"   " + jdata);

        if (File.Exists(filecheck))
        {
            string jdata = File.ReadAllText(m_sSaveFileDirectory + m_sSaveFileName);



            data = JsonConvert.DeserializeObject<CharData>(jdata);
            //GameManager.instance.getSaveLoad().gData = gData;
            //Debug.Log("파일 불러오기");



        }
        else
        {

            data = new CharData();

            //Debug.Log("파일 새로 생성");

        }

    }

    #endregion
}
