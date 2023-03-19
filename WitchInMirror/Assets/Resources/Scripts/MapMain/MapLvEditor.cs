using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class MapLvEditor : MonoBehaviour
{
    public float Timer;
    public List<ObstacleBase> obstacleobjlist = new List<ObstacleBase>(); // ù��° ��ֹ��� ����Ʈ
    [SerializeField]
    public static Queue<ObstacleBase> queActivePool = new Queue<ObstacleBase>(); // ù��° ��ֹ��� Ǯ���۾�                                                                          //[SerializeField]
    public static Queue<ObstacleBase> queInActivePool = new Queue<ObstacleBase>();

    public List<ObstacleData> obstaclesDatalist = new List<ObstacleData>(); //�� ��ֹ��� ������ ���� [1],[2],[3] �ϴܵ����ͺ��̽�

    public ObstacleBase before;
    public int beforeidx;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        int count = 10;
        for (int i = 0; i < count; i++)
        {
            GameObject sample = Instantiate(obstaclesDatalist[0].data.obj);
            sample.transform.position = new Vector3(3f, 0);
            ObstacleBase Osample = sample.GetComponent<ObstacleBase>();
            Osample.InActive();
            obstacleobjlist.Add(Osample);
            queInActivePool.Enqueue(Osample);
        }
        Debug.Log("" + queActivePool.Count);
        Debug.Log("" + queInActivePool.Count);
    }

    public void SpawnerObstacle()
    {
        Timer += Time.deltaTime;
        if (obstaclesDatalist.Count == 0)
        {
            Debug.LogError("List is Empty!");
            return;
        }
        if (Timer > 3f)
        {
            ObstacleBase obstacleBase = queInActivePool.Dequeue();
            queActivePool.Enqueue(obstacleBase);
            obstacleBase.Active();
            //obstacleBase.transform.position = new Vector3(3f,0);

            int randomidx = Random.Range(0, 4);
            obstaclesDatalist[0].data.SetTr(randomidx);
            obstacleBase.transform.position = obstaclesDatalist[0].data.Spawner_tr;
            Timer = 0;
        }
    }
    public void SpawnerObstacle1()
    {
        Timer += Time.deltaTime;
        if (obstaclesDatalist.Count == 0)
        {
            Debug.LogError("List is Empty!");
            return;
        }
        if (before)
        {
            if (before.transform.position.x < 1f)
            {
                ObstacleBase obstacleBase = queInActivePool.Dequeue();
                queActivePool.Enqueue(obstacleBase);
                obstacleBase.Active();
                while (true)
                {
                    int randomidx = Random.Range(0, 4);
                    if (beforeidx != randomidx)
                    {
                        obstaclesDatalist[0].data.SetTr(randomidx);
                        obstacleBase.transform.position = obstaclesDatalist[0].data.Spawner_tr;
                        before = obstacleBase;
                        beforeidx = randomidx;
                        Timer = 0;
                        break;
                    }
                }
            }
        }
        else if (Timer > 3f)
        {
            ObstacleBase obstacleBase = queInActivePool.Dequeue();
            queActivePool.Enqueue(obstacleBase);
            obstacleBase.Active();
            //obstacleBase.transform.position = new Vector3(3f,0);
            int randomidx = Random.Range(0, 4);
            obstaclesDatalist[0].data.SetTr(randomidx);
            obstacleBase.transform.position = obstaclesDatalist[0].data.Spawner_tr;
            Timer = 0;

            before = obstacleBase;
            beforeidx = randomidx;
            Debug.Log("hh");
        }
        else return;
    }
    // Update is called once per frame
    void Update()
    {
        //SpawnerObstacle();
        SpawnerObstacle1();
    }
}