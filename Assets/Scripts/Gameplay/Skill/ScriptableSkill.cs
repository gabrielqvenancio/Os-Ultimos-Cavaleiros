using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "Scriptable Objects/Skill")]
public class ScriptableSkill : ScriptableObject
{
    public SkillType type;
    public int basePrice;
    public float cooldown;
    public float duration;
    public AudioClip sound;
    public Sprite icon;
}
