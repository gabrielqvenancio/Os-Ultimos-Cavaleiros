using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "character", menuName = "Scriptable Objects/Character")]
public class ScriptableCharacter : ScriptableObject
{
    public string characterName;
    public int damage, health, armor;
    public Vector3 baseVelocity;
    public Vector3 pushAcceleration, pushRecovery;
    public GameObject prefab;
}