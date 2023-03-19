using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionScroller : MonoBehaviour
{
    [SerializeField]
    private Transform m_Target;
    [SerializeField]
    private float m_ScrollRange = 9.9f;
    [SerializeField]
    private float m_MoveSpeed = 3.0f;
    [SerializeField]
    private Vector3 m_MoveDirection = Vector3.left;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void MapScollorMethod()
    {
        transform.position += m_MoveSpeed * m_MoveDirection * Time.deltaTime;
        if (transform.position.x <= -m_ScrollRange)
        {
            transform.position = m_Target.position + Vector3.right * m_ScrollRange;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        MapScollorMethod();
    }
}
