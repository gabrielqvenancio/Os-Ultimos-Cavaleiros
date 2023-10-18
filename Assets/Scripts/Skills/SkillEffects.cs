using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkillEffects : MonoBehaviour
{
    internal static SkillEffects instance;

    private void Awake()
    {
        instance = this;
    }

    internal SkillEffect AssignSkillEffect(int id)
    {
        SkillEffect effect = null;
        switch(id) 
        {
            case 0:
                effect = Attack;
                break;
            case 1:
                effect = Block;
                break;
            case 2:
                effect = Potion;
                break;
        }

        return effect;
    }

    private bool IsSkillUsable(Skill skill)
    {
        if(skill.OnCooldown) return false;

        return true;
    }

    private IEnumerator PutOnCooldown(Skill skill)
    {
        yield return new WaitForSeconds(skill.attributes.cooldown);
        skill.OnCooldown = false;
    }

    private IEnumerator WaitForSkillCompletion(Skill skill)
    {
        yield return new WaitForSeconds(skill.attributes.duration);
        StartCoroutine(PutOnCooldown(skill));
    }

    internal void Attack(Skill skill)
    {
        if (!IsSkillUsable(skill)) return;
        skill.OnCooldown = true;

        Vector3 additionalVelocity = new Vector3(-skill.attributes.additionalParameters[1].value, 0, 0);
        int additionalDamage = (int)skill.attributes.additionalParameters[0].value;
        Vector3 recover = additionalVelocity / skill.attributes.duration;

        GameHandler.instance.ResetGlobalVelocity();
        GameHandler.instance.AddGlobalVelocity(additionalVelocity, recover);
        Greenie.instance.LocalHitVelocity = Vector3.zero;

        IEnumerator coroutine = HandleTimedAttackEffects(skill.attributes.duration, additionalDamage);
        StartCoroutine(coroutine);
        StartCoroutine(WaitForSkillCompletion(skill));
    }

    internal void Block(Skill skill)
    {

    }

    internal void Potion(Skill skill)
    {

    }

    private IEnumerator HandleTimedAttackEffects(float damageIncrementDuration, int additionalDamage)
    {
        Greenie.instance.Animator.SetTrigger("startAttack");
        Greenie.instance.isPushable = false;
        Greenie.instance.AdditionalDamage += additionalDamage;
        Greenie.instance.AdditionalForce += Greenie.instance.Attributes.pushForce;

        yield return new WaitForSeconds(damageIncrementDuration);

        Greenie.instance.Animator.SetTrigger("endAttack");
        Greenie.instance.isPushable = true;
        Greenie.instance.AdditionalDamage -= additionalDamage;
        Greenie.instance.AdditionalForce -= Greenie.instance.Attributes.pushForce;
    }
}
