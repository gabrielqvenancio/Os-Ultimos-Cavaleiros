using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillState
{
    ready,
    active,
    cooldown
}

public abstract class ScriptableSkill : ScriptableObject
{
    public float cooldown;
    public float duration;
    public string skillName;
    public AudioClip sound;
    public Sprite icon;

    public abstract void Effect();
    public abstract void EndEffect();
}
