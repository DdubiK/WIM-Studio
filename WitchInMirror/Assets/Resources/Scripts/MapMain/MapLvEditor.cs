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
    public List<ObstacleBase> obstacleobjlist = new List<ObstacleBase>(); // 첫번째 장애물의 리스트
    [SerializeField]
    public static Queue<ObstacleBase> queActivePool = new Queue<ObstacleBase>(); // 첫번째 장애물의 풀링작업                                                                          //[SerializeField]
    public static Queue<ObstacleBase> queInActivePool = new Queue<ObstacleBase>();

    public List<ObstacleData> obstaclesDatalist = new List<ObstacleData>(); //각 장애물의 데이터 관리 [1],[2],[3] 일단데이터베이스

    public ObstacleBase before;
    public int beforeidx;
    public int[] array = new int[18];
    public int[] array1 = new int[18];
    int resourceidx = 0;
    public string[] resourcePaths = new string[]
{
    null,
    "Map/Texture/Projectile01",
    "Map/Texture/Projectile02",
    "Map/Texture/Projectile03",
};

    // Start is called before the first frame update
    void Start()
    {
        array = new int[18] { 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 };
        array1 = new int[18] { 3, 0, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2 };
        SetPosList(24, 6, ref Initposlist);
        SetPosList(6, 6, ref Poolposlist, 2.5f);
        Initialize();
        createobj();
    }



    #region 패턴 생성 풀링

    public static Queue<RuningObject> queActive = new Queue<RuningObject>(); // 첫번째 장애물의 풀링작업                                                                          //[SerializeField]
    public static Queue<RuningObject> queInActive = new Queue<RuningObject>();

    public List<GameObject> runobj = new List<GameObject>();

    public float objmoveSpeed;

    public GameObject Player1;
    public GameObject Player2;
    public int obstaclePercent;
    public int itemPercent;
    public Vector3[] Initposlist = new Vector3[144];
    public Vector3[] Poolposlist = new Vector3[36];

    public int posidx = 0;
    public int poolposidx = 0;

    public RuningObject last_obj;
    public void SetPosList(int _row, int _col, ref Vector3[] _array, float _Setx = 0)
    {
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _col; j++)
            {

                int index = i * _col + j;
                if (index < _array.Length)
                {
                    if (j < 3)
                    {
                        _array[index] = new Vector3(_Setx + (float)i * 0.3f, 0.9f - (float)j * 0.35f);
                    }
                    else
                    {
                        _array[index] = new Vector3(_Setx + (float)i * 0.3f, -0.2f - (float)(j - 3) * 0.35f);
                    }
                }
            }
        }
        //for (int k = 0; k < _array.Length; k++)
        //{
        //    Debug.Log("array" + "[" + k + "]:" + "" + _array[k].x + "," + _array[k].y);
        //}
    }
    public void createobj()
    {

        for (int i = 0; i < 24; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                GameObject obj = new GameObject();
                obj.name = "obj" + (i + 1) + "," + (j + 1);
                obj.AddComponent<SpriteRenderer>();
                //obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Texture/Projectile01");
                obj.transform.position = Initposlist[posidx];
                //obj.transform.position = new Vector3(2.5f + (i / 3f), 1 - (j / 3f), 0);
                runobj.Add(obj);
                RuningObject a = new RuningObject();
                a.ID = array[resourceidx];
                //Debug.Log("arrayrint:" + array[resourceidx] + ",ID:" + a.ID);
                a.Obj = obj;
                if (a.ID == 0) //없음
                {
                    a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                }
                else if (a.ID == 1) //장애물
                {
                    int percent = Random.Range(0, 10);
                    string resourcePath = resourcePaths[a.ID];
                    if (percent <= obstaclePercent)
                    {
                        if (resourcePath != null)
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                        }
                    }
                    else
                    {
                        a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                        a.ID = 0;
                    }
                    //a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                }
                else if (a.ID == 2) //마력
                {
                    string resourcePath = resourcePaths[a.ID];
                    if (resourcePath != null)
                    {
                        a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                    }
                    //a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Texture/Projectile02");
                }
                else if (a.ID == 3) //아이템
                {
                    int percent = Random.Range(0, 10);
                    string resourcePath = resourcePaths[a.ID];
                    if (percent <= itemPercent)
                    {
                        if (resourcePath != null)
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                        }
                    }
                    else
                    {
                        a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                        a.ID = 0;
                    }
                    //a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Texture/Projectile03");
                }
                a.Obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                queActive.Enqueue(a);
                resourceidx++;
                posidx++;
                if (i == 23 && j == 5)
                {
                    last_obj = a;
                }

                if (resourceidx >= array.Length)
                { // 인덱스가 배열 범위를 벗어나면 0으로 초기화
                    resourceidx = 0;
                }
            }
        }
        posidx = 0;
        resourceidx = 0;
    }


    public void pulling()
    {

        if (queInActive.Count > 36 && (2.5f - last_obj.Obj.transform.position.x) >= 0.3f)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    RuningObject a = queInActive.Dequeue();
                    a.ID = array1[resourceidx];
                    a.colcheck = false;
                    a.Obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    //Debug.Log("arrayrint:" + array1[resourceidx] + ",ID:" + b.ID+"resourceidx"+resourceidx);
                    if (a.ID == 0) //없음
                    {
                        a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    else if (a.ID == 1) //장애물
                    {
                        int percent = Random.Range(0, 10);
                        string resourcePath = resourcePaths[a.ID];
                        if (percent <= obstaclePercent)
                        {
                            if (resourcePath != null)
                            {
                                a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                            }
                        }
                        else
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                            a.ID = 0;
                        }
                    }
                    else if (a.ID == 2) //마력
                    {
                        string resourcePath = resourcePaths[a.ID];
                        if (resourcePath != null)
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                        }
                        //a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Texture/Projectile02");
                    }
                    else if (a.ID == 3) //아이템
                    {
                        int percent = Random.Range(0, 10);
                        string resourcePath = resourcePaths[a.ID];
                        if (percent <= itemPercent)
                        {
                            if (resourcePath != null)
                            {
                                a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                            }
                        }
                        else
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                            a.ID = 0;
                        }
                        //a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Map/Texture/Projectile03");
                    }
                    //a.Obj.transform.position = new Vector3(2.5f + (i / 3f), 1 - (j / 3f), 0);
                    Debug.Log("poolposidx" + a.Obj.transform.position);
                    a.Obj.transform.position = Poolposlist[poolposidx];
                    resourceidx++;
                    poolposidx++;
                    if (resourceidx >= array1.Length)
                    { // 인덱스가 배열 범위를 벗어나면 0으로 초기화
                        resourceidx = 0;
                    }
                    queActive.Enqueue(a);
                    if(i==5&&j==5)
                    {
                        last_obj = a;
                    }
                }
            }
            poolposidx = 0;
            resourceidx = 0;

        }

    }
    #region 이펙트 풀링
    public List<GameObject> Effectobj = new List<GameObject>();
    public List<Animator> Effectani = new List<Animator>();
    public static Queue<Animator> EffectActive = new Queue<Animator>();
    public static Queue<Animator> EffectInActive = new Queue<Animator>();
    public List<Animator> aniList;
    public void InitEffect()
    {
        for(int i=0;i<5;i++)
        {
            Effectani[i] = new Animator();
            EffectInActive.Enqueue(Effectani[i]);
        }
    }

    #endregion
    public void moveojb()
    {
        if (queActive.Count > 0)
        {
            int sample = 0;
            foreach (RuningObject element in queActive)
            {
                element.Obj.transform.Translate(Vector3.left * Time.deltaTime * objmoveSpeed);

                if (element.Obj.transform.position.x < -3.0f)
                {
                    sample++;
                }

                if ((element.Obj.transform.position - Player1.transform.position).magnitude < 0.15f || (element.Obj.transform.position - Player2.transform.position).magnitude < 0.15f)
                {
                    if (!element.colcheck)
                    {
                        Debug.Log("충돌");
                        element.Obj.GetComponent<SpriteRenderer>().sprite = null;

                        switch (element.ID)
                        {
                            case 0:
                                //Method 실행
                                //이펙트 생성 및 실행
                                if (element.effect)
                                {
                                    element.effect.AddComponent<Animator>();
                                    Animator ani = element.effect.GetComponent<Animator>();
                                    ani.runtimeAnimatorController = aniList[0].runtimeAnimatorController;
                                }
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                        }

                        element.colcheck = true;
                    }//Debug.Log("닿음");
                }
                //element.Obj.transform.position
            }
            for (int i = 0; i < sample; i++)
            {
                RuningObject b = queActive.Dequeue();
                if (!b.Obj.GetComponent<Animator>())
                {
                    
                }
                queInActive.Enqueue(b);
            }
        }
        //int count = queActive.Count; // for 루프에서 큐의 크기를 미리 저장해둡니다.
        //for (int i = 0; i < count; i++)
        //{
        //    RuningObject element = queActive.Dequeue(); // 큐에서 요소를 꺼냅니다.
        //    element.Obj.transform.Translate(Vector3.left * Time.deltaTime);

        //    if (element.Obj.transform.position.x < -3.0f)
        //    {
        //        queInActive.Enqueue(element);
        //    }
        //    //else
        //    //{
        //    //    queActive.Enqueue(element);
        //    //}
        //}

        //for (int i = 0; i < queActive.Count; i++)
        //{
        //    queActive[i].transform.Translate(Vector3.left*Time.deltaTime);

        //}
    }

    #endregion
    public void CheckCol()
    {

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

    // 거리를 재서 간격으로 생성시킨다.
    // 생성자체에 확률을 추가한다.
    // 2개의 오브젝트를 생성하도록 한다.
    // 이 2개의 오브젝트를 시간순서별로 관리할 수 있는 제어 시스템을 만든다.
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
            if (before.transform.position.x < 2.0f)
            {
                int percent = Random.Range(0, 10);
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
        //SpawnerObstacle1();
        moveojb();
        pulling();
    }
}
