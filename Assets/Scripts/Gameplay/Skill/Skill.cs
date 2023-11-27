using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public enum SkillState
{
    ready,
    active,
    cooldown
}

public enum SkillType
{
    Attack,
    Block,
    Potion
}

internal delegate void Effect();
internal delegate void EndEffect();

public class Skill
{
    private ScriptableSkill Attributes { get; }
    internal SkillState State { get; private set; }
    internal AudioSource Source { get; }
    internal int ButtonId { get; }
    internal float Timer { get; set; }
    protected float Duration { get; set; }

    private Effect effect;
    private EndEffect endEffect;

    public Skill(SkillType type, int buttonId)
    {
        Attributes = Inventory.instance.allSkills[(int)type];
        State = SkillState.ready;

        Source = SoundHandler.instance.skillSources[(int)type];
        Source.clip = Attributes.sound;
        ButtonId = buttonId;
        Attributes = Inventory.instance.allSkills[(int)type];

        Timer = 0;
        Duration = Attributes.duration;

        switch (type)
        {
            case SkillType.Attack:
            {
                effect = Attack.instance.Effect;
                endEffect = Attack.instance.EndEffect;
                Attack.instance.skillInstance = this;
                break;
            }
            case SkillType.Block:
            {
                effect = Block.instance.Effect;
                endEffect = Block.instance.EndEffect;
                Duration += (Inventory.instance.SkillsLevel[(int)Attributes.type] - 1) * Block.instance.additionalDurationPerLevel;
                Block.instance.skillInstance = this;
                Greenie.instance.GreenieHitEvent += Block.instance.CheckInvulnerabilityMaxThreshold;
                break;
            }
            case SkillType.Potion:
            {
                effect = Potion.instance.Effect;
                endEffect = Potion.instance.EndEffect;
                Potion.instance.skillInstance = this;
                break;
            }
        }
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
                    endEffect();
                }
                break;
            }
            case SkillState.cooldown:
            {
                UIHandler.instance.ReduceSkillCooldownUI(ButtonId + 1, Timer / Attributes.cooldown);

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
        effect();
        SoundHandler.instance.PlaySoundEffect(Source, Source.clip);
        State = SkillState.active;
        Timer = Duration;
    }
}
