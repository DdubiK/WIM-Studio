using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MapEditor : MonoBehaviour
{
    public void MapEditorInit()
    {
        SetPosList(24, 6, ref Initposlist);
        SetPosList(6, 6, ref Poolposlist, 2.5f);
        PathInit();
        createobj();


    }
    public void PathInit()
    {
        resourcePaths = new string[] //아이템 리소스 파일 위치
{
    null,
    "Map/Texture/Enemy01_idle",
    "Map/Texture/Projectile01",
    "Map/Texture/Projectile02",
    "Map/Texture/Projectile03",
    "Map/Texture/Projectile04",
    "Map/Texture/Projectile05",
    "Map/Texture/Projectile06",
    "Map/Texture/Item03_Heart",
    "Map/Texture/Item01_PowerUp"
};
    }


    #region 패턴 생성 풀링

    public static Queue<RuningObject> queActive = new Queue<RuningObject>(); // 첫번째 장애물의 풀링작업                                                                          //[SerializeField]
    public static Queue<RuningObject> queInActive = new Queue<RuningObject>();

    public List<GameObject> runobj = new List<GameObject>();


    public List<GameObject> EffectobjList = new List<GameObject>();



    public float objmoveSpeed;

    public GameObject Player1;
    public GameObject Player2;
    public int obstaclePercent;
    public int itemPercent;
    public Vector3[] Initposlist = new Vector3[144];
    public Vector3[] Poolposlist = new Vector3[36];

    //오브젝트 리소스 접근 idx
    public string[] resourcePaths; //아이템 리소스 파일 위치

    public RuningObject last_obj;
    #region 리스타트 초기화
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

    #region 벡터 위치값 배열 생성
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

    #region 첫 Pattern 생성

    public void createobj()
    {
        int posidx = 0;
        int resourceidx = 0;
        for (int i = 0; i < 24; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                GameObject obj = new GameObject();
                obj.name = "obj" + (i + 1) + "," + (j + 1);
                obj.AddComponent<SpriteRenderer>();
                runobj.Add(obj);
                RuningObject a = new RuningObject();
                a.ID = 0;
                a.Obj = obj;
                //오브젝트 ID 값에 따른 리소스 할당
                if (a.ID == 0) //없음
                {
                    a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                }
                obj.transform.position = Initposlist[posidx];
                a.Obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                queActive.Enqueue(a);
                //오브젝트 리소스 idx 값 증가 , 벡터 idx값 증가
                resourceidx++;
                posidx++;
                if (i == 23 && j == 5) //배열 크기에 따라 마지막 오브젝트를 임시로 받아옴 ( 다음 오브젝트 생성 위치값 체크를 위해 )
                {
                    last_obj = a;
                }

                if (resourceidx >= 18) // 오브젝트 인덱스가 배열 범위를 벗어나면 0으로 초기화
                {
                    resourceidx = 0;
                }
            }
        }

        Initialize(10);
        InitializePool();


    }
    #endregion

    #region 오브젝트 풀링
    public void pulling()
    {
        int poolposidx = 0;
        int resourceidx = 0;
        int RandomPattern = Random.Range(0, DBLoader.MapPatternArray.Pattern.Count);
        //int datacount = 0;
        //if (datacount >= DBLoader.MapPatternArray.Pattern.Count) // 인덱스가 배열 범위를 벗어나면 0으로 초기화
        //{
        //    datacount = 0;
        //}
        if (queInActive.Count > 36 && (2.5f - last_obj.Obj.transform.position.x) >= 0.3f)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    RuningObject a = queInActive.Dequeue();
                    //a.ID = DBLoader.MapPatternArray.Pattern[datacount][resourceidx];
                    a.ID = DBLoader.MapPatternArray.Pattern[RandomPattern][resourceidx];
                    a.colcheck = false;
                    a.Obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    a.Obj.transform.position = Poolposlist[poolposidx];
                    if (a.ID == 0) //없음
                    {
                        //int percent = Random.Range(0, 100);
                        //if (percent >= itemPercent)
                        a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                        //if (percent < itemPercent)
                        //{
                        //    int[] itemProbabilities = { 65, 12, 8, 3, 3, 4, 5 }; //각 4,5,6,7,8,9 인덱스 확률
                        //    int ran = Random.Range(0, 100);
                        //    int cumulativeProbability = 0;
                        //    for (int k = 0; k < itemProbabilities.Length; k++)
                        //    {
                        //        cumulativeProbability += itemProbabilities[k];
                        //        if (ran < cumulativeProbability)
                        //        {
                        //            a.ID = k + 4;
                        //            break;
                        //        }
                        //    }
                        //    if (j >= 3 && j < 6)
                        //    {
                        //        int downitemper = 7;
                        //        int reran = Random.Range(0, 10);
                        //        if(reran <= downitemper)
                        //        {
                        //            a.ID = 0;
                        //            a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                        //        }
                        //    }
                        //}
                    }
                    if (a.ID == 1) //장애물
                    {
                        int percent = Random.Range(0, 10);
                        string resourcePath = resourcePaths[a.ID];
                        //if (percent <= obstaclePercent)
                        //{
                        if (resourcePath != null)
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                        }
                        //}
                        //else
                        //{
                        //    a.Obj.GetComponent<SpriteRenderer>().sprite = null;
                        //    a.ID = 0;
                        //}
                    }
                    if (a.ID == 2) //마력
                    {
                        string resourcePath = resourcePaths[a.ID];
                        if (resourcePath != null)
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                        }
                        if (j >= 3 && j < 6)
                        {
                            a.ID = 3;
                        }
                    }
                    if (a.ID == 3) //마력
                    {
                        string resourcePath = resourcePaths[a.ID];
                        if (resourcePath != null)
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                        }
                    }
                    if (a.ID > 3) //아이템(4 : 마력대폭증가 , 5: 마력대폭감소 , 6: 쉴드 , 7: 체질변화 , 8: 아이템성질변화 , 9: 거대화 )  
                    {
                        //int percent = Random.Range(0, 10);
                        string resourcePath = resourcePaths[a.ID];

                        if (resourcePath != null)
                        {
                            a.Obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resourcePath);
                        }
                        //GameObject pobj = GetObjectEffect();
                        //pobj.AddComponent<ParticleSystem>();
                        //ParticleSystem effectPS = pobj.GetComponent<ParticleSystem>(); //기존 파티클                     
                        //ParticleSystem newObjPS = effectIdx[1].GetComponent<ParticleSystem>(); //참조 파티클
                        GameObject effect = Instantiate(effectIdx[1]);
                        ParticleSystem effectPs = effect.GetComponent<ParticleSystem>();
                        effect.transform.position = a.Obj.transform.position;
                        effect.transform.SetParent(a.Obj.transform);
                        //var ColorOverLifetime = effectPS.colorOverLifetime;
                        //ColorOverLifetime.enabled = true;
                        //var SizeOverLifetime = effectPS.sizeOverLifetime;
                        //SizeOverLifetime.enabled = true;
                        //var shapeModule = effectPS.shape;
                        //shapeModule.enabled = false;
                        //CopyParticleSystem(effectPS, newObjPS);
                        effectPs.Play();
                        // b의 Particle System Component 설정을 a의 Particle System Component로 복사합니다.
                    }
                    resourceidx++;
                    poolposidx++;

                    if (resourceidx >= DBLoader.MapPatternArray.Pattern[RandomPattern].Length) // 인덱스가 배열 범위를 벗어나면 0으로 초기화
                    {
                        resourceidx = 0;
                    }
                    //if (resourceidx >= DBLoader.MapPatternArray.Pattern[datacount].Length) // 인덱스가 배열 범위를 벗어나면 0으로 초기화
                    //{
                    //    resourceidx = 0;
                    //}

                    queActive.Enqueue(a); //마지막 오브젝트 정보 참조
                    if (i == 5 && j == 5)
                    {
                        last_obj = a;
                    }
                    //Debug.Log("obj:" + a.Obj.name + ",ID:" + a.ID);
                }
            }
            poolposidx = 0;
            resourceidx = 0;
            //datacount++;
            //Debug.Log("datacount:" + datacount);
        }

    }
    #endregion

    #region 이펙트 생성
    //public GameObject effectpoolingPrefeb;
    public Queue<Effect> effectpoolingQueue = new Queue<Effect>();

    public Queue<Animator> effectpsQueue = new Queue<Animator>();
    public List<GameObject> effectpsList = new List<GameObject>();
    public int poolSize;
    public List<GameObject> effectIdx = new List<GameObject>();
    public string[] effectPaths; //아이템 리소스 파일 위치


    public void CopyParticleSystem(ParticleSystem destination, ParticleSystem source)
    {
        if (destination.isPlaying)
        {
            // 시스템을 중지하고 모든 파티클이 만료될 때까지 기다림
            destination.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        // 파티클 메인 모듈 속성 복사
        ParticleSystem.MainModule mainModule = source.main;
        ParticleSystem.MainModule destMainModule = destination.main;
        destMainModule.duration = mainModule.duration;
        destMainModule.loop = mainModule.loop;
        destMainModule.startDelay = mainModule.startDelay;
        destMainModule.startLifetime = mainModule.startLifetime;
        destMainModule.startSpeed = mainModule.startSpeed;
        destMainModule.startSize = mainModule.startSize;
        destMainModule.startColor = mainModule.startColor;
        destMainModule.gravityModifier = mainModule.gravityModifier;
        destMainModule.simulationSpace = mainModule.simulationSpace;
        destMainModule.scalingMode = mainModule.scalingMode;
        destMainModule.startRotation = mainModule.startRotation;
        destMainModule.emitterVelocityMode = mainModule.emitterVelocityMode;
        destMainModule.maxParticles = mainModule.maxParticles;
        destMainModule.cullingMode = mainModule.cullingMode;
        // 파티클 에미션 모듈 속성 복사
        ParticleSystem.EmissionModule emissionModule = source.emission;
        ParticleSystem.EmissionModule destEmissionModule = destination.emission;
        destEmissionModule.enabled = emissionModule.enabled;
        destEmissionModule.rateOverTime = emissionModule.rateOverTime;
        destEmissionModule.rateOverDistance = emissionModule.rateOverDistance;
        destEmissionModule.burstCount = emissionModule.burstCount;
        for (int i = 0; i < emissionModule.burstCount; i++)
        {
            destEmissionModule.SetBurst(i, emissionModule.GetBurst(i));
        }

        // 파티클 렌더러 속성 복사
        ParticleSystemRenderer renderer = source.GetComponent<ParticleSystemRenderer>();
        ParticleSystemRenderer destRenderer = destination.GetComponent<ParticleSystemRenderer>();
        if (renderer != null && destRenderer != null)
        {
            //Material myMaterial = Resources.Load<Material>("Prefabs/Effects/CFX4_AuraBubble ADD");
            //Material destMaterial = destRenderer.material != null ? destRenderer.material : new Material(Shader.Find("Particles/Standard Unlit"));
            string materialName = renderer.sharedMaterial.name;
            Material myMaterial = Resources.Load<Material>("Prefabs/Effects/Matarial/" + materialName);
            //// 원본 파티클 시스템의 material 복사
            //destMaterial.CopyPropertiesFromMaterial(renderer.sharedMaterial);
            //destRenderer.material = destMaterial;
            //string shaderName = renderer.GetComponent<Renderer>().sharedMaterial.shader.name;
            //Debug.Log("shader:" + shaderName);
            //Material shader = Resources.Load<Material>(shaderName);
            //Debug.Log("shader:" + shader.name);
            destRenderer.material = myMaterial;
            destRenderer.maxParticleSize = renderer.maxParticleSize;
            destRenderer.minParticleSize = renderer.minParticleSize;
            destRenderer.velocityScale = renderer.velocityScale;
            destRenderer.lengthScale = renderer.lengthScale;
            destRenderer.normalDirection = renderer.normalDirection;
            destRenderer.sortMode = renderer.sortMode;
            destRenderer.sortingFudge = renderer.sortingFudge;
            destRenderer.minParticleSize = renderer.minParticleSize;
            destRenderer.maxParticleSize = renderer.maxParticleSize;
        }

        // 파티클 트랜스폼 속성 복사
        Transform sourceTransform = source.GetComponent<Transform>();
        Transform destTransform = destination.GetComponent<Transform>();
        destTransform.position = sourceTransform.position;
        destTransform.rotation = sourceTransform.rotation;
        destTransform.localScale = sourceTransform.localScale;
    }

    private void InitializePool()
    {
        poolSize = 10;
        // 오브젝트 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            Animator obj = CreateEffectPsObj();
            obj.gameObject.SetActive(false);
            effectpsQueue.Enqueue(obj);
            //effectpsList.Add(obj);
        }
        effectPaths = new string[]
        {
            null,
            "Prefabs/Effects/ItemIdle",
            "Prefabs/Effects/GetItemEffect",
            "Prefabs/Effects/MapEffect1",
            "Prefabs/Effects/MapEffect2",
            "Prefabs/Effects/MapEffect3",
            "Prefabs/Effects/MapEffect4",
            "Prefabs/Effects/MapEffect4",
            "Prefabs/Effects/SuperMode1",
            "Prefabs/Effects/SuperMode2",
        };
        for (int j = 0; j < effectPaths.Length; j++)
        {
            GameObject effectprepab = Resources.Load<GameObject>(effectPaths[j]);
            effectIdx.Add(effectprepab);
        }
    }


    public Animator GetObjectEffect()
    {
        if (effectpsQueue.Count > 0)
        {
            // 큐에서 오브젝트를 꺼내 반환
            Animator obj = effectpsQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            // 풀이 비어있는 경우 새로운 오브젝트 생성
            Animator obj = Instantiate(CreateEffectPsObj());
            //effectpsList.Add(obj);
            return obj;
        }
    }
    public Animator CreateEffectPsObj()
    {
        GameObject PsObj = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/GetMagicEffect"));
        PsObj.name = "MEobj";
        PsObj.transform.position = new Vector2(-20, 0);

        Animator psAni = PsObj.GetComponent<Animator>();

        return psAni;
    }



    public void ReturnObjectEffect(int idx, Animator obj)
    {
        // 오브젝트를 풀에 반환
        //obj.SetActive(false);
        effectpsQueue.Enqueue(obj);
    }

    public void Initialize(int _Count)
    {
        for (int i = 0; i < _Count; i++)
        {
            effectpoolingQueue.Enqueue(CreateNewObject());
        }
    }

    public Effect CreateNewObject()
    {
        GameObject EObj = new GameObject();
        EObj.name = "Eobj";
        EObj.AddComponent<SpriteRenderer>();
        EObj.AddComponent<Effect>();
        //EObj.AddComponent<ParticleSystem>();
        EObj.transform.position = new Vector2(-20, 0);

        var newObj = EObj.GetComponent<Effect>();
        newObj.gameObject.SetActive(false);
        //newObj.transform.SetParent(this.transform);

        return newObj;
    }

    public Effect GetEffect()
    {
        if (effectpoolingQueue.Count > 0f)
        {
            var obj = effectpoolingQueue.Dequeue();
            //obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject();
            newObj.gameObject.SetActive(false);
            //newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public void ReturnObject(Effect obj)
    {
        obj.gameObject.SetActive(false);
        //obj.transform.SetParent(this.transform);
        effectpoolingQueue.Enqueue(obj);
    }



    #endregion

    #region 오브젝트 중재자
    public void moveojb()
    {
        if (queActive.Count > 0)
        {
            int sample = 0;
            foreach (RuningObject element in queActive)
            {
                element.Obj.transform.Translate(Vector3.left * Time.deltaTime * objmoveSpeed * (1 + GameManager.instance.StageLv * 0.15f));//스테이지 레벨에 따른 장애물 속도 증가

                if (element.Obj.transform.position.x < -3.0f)
                {
                    if (element.Obj.transform.childCount > 0)
                    {
                        Destroy(element.Obj.transform.GetChild(0).gameObject);
                    }
                    sample++;
                }

                if (!element.colcheck)
                {
                    float checkDistance = 0;
                    if (!GameManager.instance.magicStop) checkDistance = 0.15f;
                    if (GameManager.instance.magicStop) checkDistance = 0.3f;

                    bool p1 = false;
                    //Debug.Log("충돌네임:" + element.Obj.name+"element ID:"+element.ID);
                    bool p2 = false;
                    if ((element.Obj.transform.position - Player1.transform.position).magnitude < checkDistance)
                    {
                        p1 = true;
                    }
                    if ((element.Obj.transform.position - Player2.transform.position).magnitude < checkDistance)
                    {
                        p2 = true;
                    }
                    //Debug.Log("충돌");

                    if (p1 || p2)
                    {
                        GameObject effectobj;
                        switch (element.ID)
                        {
                            case 0:
                                break;
                            case 1:
                                Debug.Log("장애물 1");
                                GameManager.instance.Damaged();

                                //Debug.Log("충돌"+element.Obj.name);
                                break;
                            case 2:
                            case 3:
                                if (element.ID == 2) GameManager.instance.MagicUp();
                                if (element.ID == 3) GameManager.instance.MagicDown();
                                GameManager.instance.UpScore(10);
                                Effect Eobj = GetEffect();
                                if (p1)
                                {
                                    Eobj.player = GameManager.instance.player[0];
                                }
                                if (p2)
                                {
                                    Eobj.player = GameManager.instance.player[1];
                                    Eobj.downCheck = true;
                                }
                                GameManager.instance.SoundPlay(1);
                                Eobj.transform.position = element.Obj.transform.position;
                                ParticleSystem particle = Eobj.GetComponent<ParticleSystem>();
                                //particle = ;
                                Eobj.Get();
                                Eobj.Pooling();
                                Sprite b = element.Obj.GetComponent<SpriteRenderer>().sprite;
                                Eobj.GetComponent<SpriteRenderer>().sprite = b;
                                element.Obj.GetComponent<SpriteRenderer>().sprite = null;
                                Animator psAni = GetObjectEffect();
                                psAni.transform.position = element.Obj.transform.position;
                                psAni.Play("ItemEffect");
                                break;
                            case 4:
                            case 5:
                                if (element.ID == 4) GameManager.instance.MagicItem(1);
                                if (element.ID == 5) GameManager.instance.MagicItem(2);
                                element.effect = effectIdx[3];
                                Debug.Log("아이템 4,5 == 마력 대폭 증가/감소 아이템");
                                Destroy(element.Obj.transform.GetChild(0).gameObject);
                                effectobj = Instantiate(effectIdx[2]);
                                effectobj.transform.position = element.Obj.transform.position;
                                Destroy(effectobj, 1f);
                                element.Obj.GetComponent<SpriteRenderer>().sprite = null;
                                break;
                            case 6:
                                Debug.Log("아이템 6 == 쉴드");
                                GameManager.instance.ShieldItem();
                                Destroy(element.Obj.transform.GetChild(0).gameObject);
                                effectobj = Instantiate(effectIdx[2]);
                                effectobj.transform.position = element.Obj.transform.position;
                                Destroy(effectobj, 1f);
                                element.Obj.GetComponent<SpriteRenderer>().sprite = null;
                                break;
                            case 7:
                                Debug.Log("아이템 7 == 체질변화(지속감소되는 마력이 증가/감소)");
                                GameManager.instance.MagicReverseItem();
                                Destroy(element.Obj.transform.GetChild(0).gameObject);
                                effectobj = Instantiate(effectIdx[2]);
                                effectobj.transform.position = element.Obj.transform.position;
                                Destroy(effectobj, 1f);

                                element.Obj.GetComponent<SpriteRenderer>().sprite = null;
                                break;
                            case 8:
                                Debug.Log("아이템 8 == 아이템성질변화(+면 -로 반대로 10초 유지)");
                                GameManager.instance.ItemReverseItem();
                                Destroy(element.Obj.transform.GetChild(0).gameObject);
                                effectobj = Instantiate(effectIdx[2]);
                                effectobj.transform.position = element.Obj.transform.position;
                                Destroy(effectobj, 1f);
                                element.Obj.GetComponent<SpriteRenderer>().sprite = null;
                                break;
                            case 9:
                                Debug.Log("아이템 9 == 거대화");
                                GameManager.instance.MagicStopItem();
                                Destroy(element.Obj.transform.GetChild(0).gameObject);
                                effectobj = Instantiate(effectIdx[2]);
                                effectobj.transform.position = element.Obj.transform.position;
                                Destroy(effectobj, 1f);
                                element.Obj.GetComponent<SpriteRenderer>().sprite = null;
                                break;
                        }
                        element.colcheck = true;
                    }

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


