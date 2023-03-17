using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.IO;

public class SLManager : MonoBehaviour
{

    private string m_sSaveFileDirectory;  // ������ ���� ���
    private string m_sSaveFileName = "/InventoryData.json"; // ���� �̸�

    public CharData data;


    // Start is called before the first frame update
    void Start()
    {
        RemoteStart();
    }

    #region ���� ���� ����
    public void RemoteStart()
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
    public void _reset()
    {

        //string jdata = JsonConvert.SerializeObject(gData, Formatting.Indented);
        //File.WriteAllText(Application.persistentDataPath + "/czSaveData.json", jdata);
        string filecheck = m_sSaveFileDirectory + m_sSaveFileName;
        File.Delete(filecheck);
    }


    public void _save()
    {

        //List ������
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
            //Debug.Log("���� �ҷ�����");



        }
        else
        {

            data = new CharData();

            //Debug.Log("���� ���� ����");

        }

    }

    #endregion
}
