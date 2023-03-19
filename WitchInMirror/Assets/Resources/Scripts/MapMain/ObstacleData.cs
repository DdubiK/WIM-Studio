using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObject/Data", order = int.MaxValue)]
public class ObstacleData : ScriptableObject
{
    public Obstacle data;
}
