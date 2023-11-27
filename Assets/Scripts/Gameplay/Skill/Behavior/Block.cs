using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    internal static Block instance;

    internal Skill skillInstance;
    internal ScriptableSkill Attributes;
    [SerializeField] private Vector3 additionalForce;
    [SerializeField] private int baseMaxHitCount;
    [SerializeField] private int additionalMaxHitCountPerLevel;
    [SerializeField] internal float additionalDurationPerLevel;
    private int invulnerabilityCount;

    private void Start()
    {
        instance = this;
        Attributes = Inventory.instance.allSkills[(int)SkillType.Block];
        invulnerabilityCount = 0;

    }

    public void Effect()
    {
        invulnerabilityCount = 0;
        Greenie.instance.Animator.SetTrigger("startBlock");
        Greenie.instance.Animator.SetBool("isBlocking", true);
        Greenie.instance.AdditionalForce += additionalForce;
        Greenie.instance.ReducedDamagePercentage += 100;
        Greenie.instance.Pushable += 1;
    }

    public void EndEffect()
    {
        Greenie.instance.Animator.SetTrigger("endBlock");
        Greenie.instance.Animator.SetBool("isBlocking", false);
        Greenie.instance.AdditionalForce -= additionalForce;
        Greenie.instance.ReducedDamagePercentage -= 100;
        Greenie.instance.Pushable -= 1;
    }

    internal void CheckInvulnerabilityMaxThreshold()
    {
        if (skillInstance.State != SkillState.active)
        {
            return;
        }

        invulnerabilityCount++;
        if (invulnerabilityCount == baseMaxHitCount + ((Inventory.instance.SkillsLevel[(int)Attributes.type] - 1) * additionalMaxHitCountPerLevel))
        {
            skillInstance.Timer = 0;
        }
    }
}
