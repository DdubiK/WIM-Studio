using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public void MapEditorInit()
    {
        SetPosList(24, 6, ref Initposlist);
        SetPosList(6, 6, ref Poolposlist, 2.5f);
        createobj();
    }
    // Start is called before the first frame update

    #region ���� ���� Ǯ��

    public static Queue<RuningObject> queActive = new Queue<RuningObject>(); // ù��° ��ֹ��� Ǯ���۾�                                                                          //[SerializeField]
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
    #region ����ŸƮ �ʱ�ȭ
    public void ResetPooling()
    {
        int count = 0;
        int count1 = 144;
        foreach (RuningObject element in queActive)
        {
            queInActive.Enqueue(element);
            count++;
        }
        for (int i = 0; i < count; i++)
        {
            RuningObject a = queActive.Dequeue();
            a.Obj.transform.position = new Vector3(-10f, 0);
            a.colcheck = false;
        }
        Debug.Log("queActive:" + queActive.Count);
        Debug.Log("queInActive:" + queInActive.Count);

        for (int i = 0; i < count1; i++)
        {
            queActive.Enqueue(queInActive.Dequeue());
        }
        //foreach (RuningObject element in queInActive)
        //{
        //    queActive.Enqueue(queInActive.Dequeue());
        //}
    }

    #endregion

    #region ���� ��ġ�� �迭 ����
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
    #endregion
    #region ù Pattern ����
    public int[] array = new int[18]{ 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 }; //�ӽ� ù ���� �迭
int resourceidx = 0; //������Ʈ ���ҽ� ���� idx
    public string[] resourcePaths = new string[] //������ ���ҽ� ���� ��ġ
{
    null,
    "Map/Texture/Projectile01",
    "Map/Texture/Projectile02",
    "Map/Texture/Projectile03",
};
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
                //������Ʈ ID ���� ���� ���ҽ� �Ҵ�
                if (a.ID == 0) //����
                {
                    a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                }
                else if (a.ID == 1) //��ֹ�
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
                else if (a.ID == 2) //����
                {
                    string resourcePath = resourcePaths[a.ID];
                    if (resourcePath != null)
                    {
                        a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                    }
                }
                else if (a.ID == 3) //������
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
                }
                a.Obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                queActive.Enqueue(a);
                //������Ʈ ���ҽ� idx �� ���� , ���� idx�� ����
                resourceidx++;
                posidx++;
                if (i == 23 && j == 5) //�迭 ũ�⿡ ���� ������ ������Ʈ�� �ӽ÷� �޾ƿ� ( ���� ������Ʈ ���� ��ġ�� üũ�� ���� )
                {
                    last_obj = a;
                }

                if (resourceidx >= array.Length) // ������Ʈ �ε����� �迭 ������ ����� 0���� �ʱ�ȭ
                { 
                    resourceidx = 0;
                }
            }
        }
        posidx = 0;
        resourceidx = 0;
    }
    #endregion
    #region ������Ʈ Ǯ��
    public void pulling()
    {

        if (queInActive.Count > 36 && (2.5f - last_obj.Obj.transform.position.x) >= 0.3f)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    RuningObject a = queInActive.Dequeue();
                    a.ID = DBLoader.MapPatternArray.Pattern[3][resourceidx];
                    a.colcheck = false;
                    a.Obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    if (a.ID == 0) //����
                    {
                        a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    else if (a.ID == 1) //��ֹ�
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
                    else if (a.ID == 2) //����
                    {
                        string resourcePath = resourcePaths[a.ID];
                        if (resourcePath != null)
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                        }
                    }
                    else if (a.ID == 3) //������
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
                    }
                    a.Obj.transform.position = Poolposlist[poolposidx];
                    resourceidx++;
                    poolposidx++;
                    if (resourceidx >= DBLoader.MapPatternArray.Pattern[3].Length) // �ε����� �迭 ������ ����� 0���� �ʱ�ȭ
                    { 
                        resourceidx = 0;
                    }
                    queActive.Enqueue(a); //������ ������Ʈ ���� ����
                    if (i == 5 && j == 5)
                    {
                        last_obj = a;
                    }
                }
            }
            poolposidx = 0;
            resourceidx = 0;

        }

    }
    #endregion
    #region ����Ʈ Ǯ��
    public List<GameObject> Effectobj = new List<GameObject>();
    public List<Animator> Effectani = new List<Animator>();
    public static Queue<Animator> EffectActive = new Queue<Animator>();
    public static Queue<Animator> EffectInActive = new Queue<Animator>();
    public List<Animator> aniList;
    public void InitEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            Effectani[i] = new Animator();
            EffectInActive.Enqueue(Effectani[i]);
        }
    }

    #endregion
    #region ������Ʈ ������
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

                if (!element.colcheck &&
                    ((element.Obj.transform.position - Player1.transform.position).magnitude < 0.15f || (element.Obj.transform.position - Player2.transform.position).magnitude < 0.15f))
                {

                    //Debug.Log("�浹");
                    element.Obj.GetComponent<SpriteRenderer>().sprite = null;

                    switch (element.ID)
                    {
                        case 0:
                            //Method ����
                            //����Ʈ ���� �� ����
                            if (element.effect)
                            {
                                element.effect.AddComponent<Animator>();
                                Animator ani = element.effect.GetComponent<Animator>();
                                ani.runtimeAnimatorController = aniList[0].runtimeAnimatorController;
                            }
                            break;
                        case 1:
                            GameManager.instance.MagicUpDown();
                            //Debug.Log("�浹"+element.Obj.name);
                            break;
                        case 2:
                            break;
                    }

                    element.colcheck = true;
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
    }
    #endregion

    #endregion
    // Update is called once per frame
    void Update()
    {
        //moveojb();
        //pulling();
    }
}


