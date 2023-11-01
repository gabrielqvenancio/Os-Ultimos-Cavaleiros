using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "Scriptable Objects/Level")]
public class ScriptableLevel : ScriptableObject
{
    public string levelName;
    public GameObject[] enemiesToSpawn;
    public int amountOfLayers;
    public AudioClip music;
    public GameObject mapPrefab;
}
