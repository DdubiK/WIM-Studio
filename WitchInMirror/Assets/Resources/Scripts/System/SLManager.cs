using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.IO;

public static class SLManager 
{

    private static string m_sSaveFileDirectory;  // 저장할 폴더 경로
    private static string m_sSaveFileName = "/HighScore.json"; // 파일 이름

    //public List<int[]> data = new List<int[]>();
    public static int Score;

    // Start is called before the first frame update

    #region 시작 원격 설정
    public static void RemoteStart()
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
    public static void _reset()
    {

        //string jdata = JsonConvert.SerializeObject(gData, Formatting.Indented);
        //File.WriteAllText(Application.persistentDataPath + "/czSaveData.json", jdata);
        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;
        File.Delete(filecheck);
    }


    public static void _save()
    {

        //List 못읽음
        string jdata = JsonConvert.SerializeObject(Score);

        File.WriteAllText(m_sSaveFileDirectory + m_sSaveFileName, jdata);

    }

    public static void _load()
    {

        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;


        //JObject
        //Debug.Log(File.Exists(filecheck) +"   " + jdata);

        if (File.Exists(filecheck))
        {
            string jdata = File.ReadAllText(m_sSaveFileDirectory + m_sSaveFileName);



            Score = JsonConvert.DeserializeObject<int>(jdata);

            //GameManager.instance.getSaveLoad().gData = gData;
            //Debug.Log("파일 불러오기");



        }
        else
        {

            Score = 0;

            //Debug.Log("파일 새로 생성");

        }

    }

    #endregion
}
