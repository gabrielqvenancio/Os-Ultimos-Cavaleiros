using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    internal static Attack instance;

    internal ScriptableSkill Attributes;
    internal Skill skillInstance;
    [SerializeField] private int baseAdditionalDamage;
    [SerializeField] private int additionalDamagePerLevel;
    [SerializeField] private Vector3 additionalVelocity, additionalForce;
    [SerializeField] private AudioClip[] greenieVoices;

    private void Start()
    {
        instance = this;
        Attributes = Inventory.instance.allSkills[(int)SkillType.Attack];
    }

    internal void Effect()
    {
        SoundHandler.instance.PlaySoundEffect(Greenie.instance.gameObject.GetComponent<AudioSource>(), greenieVoices[Random.Range(0, greenieVoices.Length)]);

        PhysicsHandler.instance.ResetGlobalVelocity();
        PhysicsHandler.instance.AddGlobalVelocity(-additionalVelocity, -additionalVelocity / Attributes.duration);
        Greenie.instance.LocalHitVelocity = Vector3.zero;

        Greenie.instance.Animator.SetTrigger("startAttack");
        Greenie.instance.Animator.SetBool("isAttacking", true);
        Greenie.instance.transform.Find("Attack Effect").GetComponent<Animator>().SetTrigger("Attack");
        Greenie.instance.Pushable += 1;
        Greenie.instance.AdditionalDamage += baseAdditionalDamage + (Inventory.instance.SkillsLevel[(int)Attributes.type] - 1) * additionalDamagePerLevel;
        Greenie.instance.AdditionalForce += additionalForce;
    }

    internal void EndEffect()
    {
        Greenie.instance.Animator.SetTrigger("endAttack");
        Greenie.instance.Animator.SetBool("isAttacking", false);
        Greenie.instance.Pushable -= 1;
        Greenie.instance.AdditionalDamage -= baseAdditionalDamage + (Inventory.instance.SkillsLevel[(int)Attributes.type] - 1) * additionalDamagePerLevel;
        Greenie.instance.AdditionalForce -= additionalForce;
    }
}
