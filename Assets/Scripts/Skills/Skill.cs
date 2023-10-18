using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal delegate void SkillEffect(Skill skill);

public class Skill
{
    internal ScriptableSkill attributes;
    internal bool OnCooldown { get; set; }

    internal SkillEffect SkillEffect { get; }

    public Skill(ScriptableSkill attributes)
    {
        this.attributes = attributes;
        OnCooldown = false;
        SkillEffect = SkillEffects.instance.AssignSkillEffect(attributes.id);
    }
}
