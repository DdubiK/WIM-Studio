using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackGround : MonoBehaviour
{
    public MeshRenderer _renderer;
    public float renderer_offset;
    public Vector3 offset;
    public float scrollSpeed;
    public Texture materialTexture;
    // Start is called before the first frame update
    //void Start()
    //{
    //    _renderer = GetComponent<MeshRenderer>();
    //    Debug.Log("BackGround:" + gameObject.name);
    //    //_renderer = GetComponent<MeshRenderer>();
    //    //targetcamera = GetComponent<Transform>();
    //    //rect = GetComponent<RectTransform>();
    //    //rect.transform.SetAsFirstSibling();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //    //if (_renderer.material)
    //    //{
    //    //    _renderer.material.mainTextureOffset = new Vector2(scrollSpeed *Time.deltaTime, 0);
    //    //}
    //    //Debug.Log(_renderer.material.name);

    //    float newOffSetX = _renderer.material.mainTextureOffset.x + scrollSpeed * Time.deltaTime;
    //    Vector2 newOffset = new Vector2(newOffSetX, 0);

    //    _renderer.material.mainTextureOffset = newOffset;

    //    //this.transform.position = new Vector3(targetcamera.transform.position.x, 0, 0);
    //}
    //private void LateUpdate()
    //{



    //    //renderer_offset = targetcamera.position.x + offset.x;
    //    //_renderer.material.mainTextureOffset = new Vector2(1, 0);
    //}

    private float textureUnitSizeX;

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        textureUnitSizeX = _renderer.material.mainTexture.width / _renderer.material.mainTextureScale.x;
    }

    void Update()
    {
        float newOffSetX = _renderer.material.mainTextureOffset.x + scrollSpeed * Time.deltaTime;
        Vector2 newOffset = new Vector2(newOffSetX, 0);

        _renderer.material.mainTextureOffset = newOffset;

        if (newOffSetX > textureUnitSizeX)
        {
            _renderer.material.mainTextureOffset = new Vector2(newOffSetX - textureUnitSizeX, 0);
        }
    }
}
