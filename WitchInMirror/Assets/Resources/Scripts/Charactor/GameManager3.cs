using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager3 : MonoBehaviour
{
    public static GameManager3 GetInstance() { return instance; }
    public static GameManager3 instance = null;

    public GameObject player1;
    public GameObject player2;
    public GameObject effectpoolingPrefeb;
    public Mist mist;
    public float magic;
    public bool magicReverse;
    public bool itemReverse;
    public bool magicStop;

    public Queue<Effect> effectpoolingQueue = new Queue<Effect>();
    public List<GameObject> objList = new List<GameObject>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (!instance) instance = this;
        Initialize(20);
    }
    void Start()
    {
        magic = GameManager.GetInstance().magic;
    }
    public void Initialize(int _Count)
    {
        for(int i = 0; i < _Count; i++)
        {
            effectpoolingQueue.Enqueue(CreateNewObject());
        }
    }
    public Effect CreateNewObject()
    {
        var newObj = Instantiate(effectpoolingPrefeb).GetComponent<Effect>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static Effect GetEffect()
    {
        if(instance.effectpoolingQueue.Count > 0f)
        {
            var obj = instance.effectpoolingQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = instance.CreateNewObject();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public static void ReturnObject(Effect obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance.effectpoolingQueue.Enqueue(obj);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
