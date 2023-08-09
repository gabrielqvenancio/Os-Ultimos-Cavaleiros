using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "Scriptable Objects/Enemy")]
public class ScriptableEnemy : ScriptableObject
{
    public string enemyName;
    public int rarity;
    public int damage;
    public int health;
}
