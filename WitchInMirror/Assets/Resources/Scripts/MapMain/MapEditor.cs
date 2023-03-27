using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct PatternElement
{
    public int idx;
    public Vector3 Spawner_tr;
    public Sprite sprite;
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

    public void SetElement(int newIdx, Vector3 newPos, Sprite newSprite)
    {
        //idx = newIdx;
        //pos = newPos;
        //sprite = newSprite;
    }
}
[System.Serializable]
public struct PatternBase
{
    public int idx;
    [SerializeField]
    public List<PatternElement> mypattern ;
    //public List<>
}


public class MapEditor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
