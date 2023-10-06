using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal delegate void SkillEffect();

public class Skill
{
    [SerializeField] internal ScriptableSkill attributes;

    internal SkillEffect SkillEffect { get; }

    public Skill(ScriptableSkill attributes)
    {
        this.attributes = attributes;
        SkillEffect = SkillEffects.AssignSkillEffect(attributes.id);
    }
}
