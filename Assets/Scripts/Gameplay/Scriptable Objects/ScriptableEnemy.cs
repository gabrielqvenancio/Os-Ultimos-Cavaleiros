using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "Scriptable Objects/Enemy")]
public class ScriptableEnemy : ScriptableObject
{
    public int rarity, mapId, scoreYield;
    public AudioClip[] deathSounds;
}
