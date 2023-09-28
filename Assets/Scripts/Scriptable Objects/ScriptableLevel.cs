using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "Scriptable Objects/Level")]
public class ScriptableLevel : ScriptableObject
{
    public string levelName;
    public GameObject[] enemiesToSpawn;
    public Sprite ground, horizon;
    public int amountOfLayers;
}
