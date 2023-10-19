using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "character", menuName = "Scriptable Objects/Character")]
public class ScriptableCharacter : ScriptableObject
{
    public string characterName;
    public int damage, health, armor;
    public float resistance;
    public Vector3 baseVelocity;
    public Vector3 pushForce;
    public GameObject prefab;
}