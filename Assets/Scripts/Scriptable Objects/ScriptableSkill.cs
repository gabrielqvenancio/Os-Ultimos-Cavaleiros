using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AdditionalAttributes
{
    public string name;
    public float value;
}

[CreateAssetMenu(fileName = "skill", menuName = "Scriptable Objects/Skill")]
public class ScriptableSkill : ScriptableObject
{
    public List<AdditionalAttributes> additionalParameters;
    public int id;
    public float cooldown;
    public float duration;
    public string skillName;
    public Sprite icon;
}
