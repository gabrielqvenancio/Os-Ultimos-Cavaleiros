using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffects : MonoBehaviour
{
    internal static SkillEffects instance;

    private void Start()
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

    internal void Attack(Skill skill)
    {
        Vector3 additionalVelocity = new Vector3(-skill.attributes.additionalParameters[1].value, 0, 0);
        int additionalDamage = (int)skill.attributes.additionalParameters[0].value;
        Vector3 recover = additionalVelocity / skill.attributes.duration;

        GameHandler.instance.ResetGlobalVelocity();
        Greenie.instance.isPushable = false;
        GameHandler.instance.AddGlobalVelocity(additionalVelocity, recover);

        Greenie.instance.AdditionalDamage += additionalDamage;
        IEnumerator coroutine = AttackIncreasedDamageHandler(skill.attributes.duration, additionalDamage);

        StartCoroutine(coroutine);

        Greenie.instance.Animator.SetTrigger("startAttack");
    }

    private IEnumerator AttackIncreasedDamageHandler(float damageIncrementDuration, int additionalDamage)
    {
        yield return new WaitForSeconds(damageIncrementDuration);
        Greenie.instance.AdditionalDamage -= additionalDamage;
        Greenie.instance.isPushable = true;
        Greenie.instance.Animator.SetTrigger("endAttack");
    }

    internal void Block(Skill skill)
    {

    }

    internal void Potion(Skill skill)
    {

    }
}
