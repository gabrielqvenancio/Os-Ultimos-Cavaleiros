using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "Scriptable Objects/Enemy")]
public class ScriptableEnemy : ScriptableCharacter
{
    public int rarity, mapId, scoreYield;
    public ScriptableDrop drop;
}
