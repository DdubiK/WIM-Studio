using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.IO;

public static class SLManager 
{

    private static string m_sSaveFileDirectory;  // ������ ���� ���
    private static string m_sSaveFileName = "/HighScore.json"; // ���� �̸�

    //public List<int[]> data = new List<int[]>();
    public static int Score;

    // Start is called before the first frame update

    #region ���� ���� ����
    public static void RemoteStart()
    {
        m_sSaveFileDirectory = Application.dataPath + "/Save/";
        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;

        if (!Directory.Exists(m_sSaveFileDirectory)) // �ش� ��ΰ� �������� �ʴ´ٸ�
            Directory.CreateDirectory(m_sSaveFileDirectory); // ���� ����(��� ����)


        _load();
        _save();

    }

    #endregion

    #region ���� ���̺� �ε�
    public static void _reset()
    {

        //string jdata = JsonConvert.SerializeObject(gData, Formatting.Indented);
        //File.WriteAllText(Application.persistentDataPath + "/czSaveData.json", jdata);
        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;
        File.Delete(filecheck);
    }


    public static void _save()
    {

        //List ������
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
            //Debug.Log("���� �ҷ�����");



        }
        else
        {

            Score = 0;

            //Debug.Log("���� ���� ����");

        }

    }

    #endregion
}
