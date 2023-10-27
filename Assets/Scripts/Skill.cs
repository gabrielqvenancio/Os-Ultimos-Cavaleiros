using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [SerializeField] private ScriptableSkill attributes;
    internal ScriptableSkill Attributes { get { return attributes; } }
    internal SkillState State { get; set; }
    internal float Timer { get; set; }
    internal int Id { get; set; }

    public Skill()
    {
        State = SkillState.ready;
        Timer = 0;
    }

    internal void CheckState()
    {
        switch (State)
        {
            case SkillState.ready:
            {
                break;
            }
            case SkillState.active:
            {
                if(Timer <= 0)
                {
                    State = SkillState.cooldown;
                    Timer = Attributes.cooldown;
                    Attributes.EndEffect();
                }
                break;
            }
            case SkillState.cooldown:
            {
                UIHandler.instance.ReduceSkillCooldownUI(Id + 1, Timer / Attributes.cooldown);

                if (Timer <= 0)
                {
                    State = SkillState.ready;
                    Timer = 0;
                }
                break;
            }
        }
    }

    public void Use()
    {
        Attributes.Effect();
        State = SkillState.active;
        Timer = Attributes.duration;
    }
}
