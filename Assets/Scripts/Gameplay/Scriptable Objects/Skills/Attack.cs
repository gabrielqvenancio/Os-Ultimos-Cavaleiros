using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Skills/Attack")]
public class Attack : ScriptableSkill
{
    public int additionalDamage;
    public Vector3 additionalVelocity, additionalForce;
    public AudioClip[] greenieVoices;

    public override void Effect()
    {
        SoundHandler.instance.PlaySoundEffect(Greenie.instance.gameObject.GetComponent<AudioSource>(), greenieVoices[Random.Range(0, greenieVoices.Length)]);

        PhysicsHandler.instance.ResetGlobalVelocity();
        PhysicsHandler.instance.AddGlobalVelocity(- additionalVelocity, - additionalVelocity / duration);
        Greenie.instance.LocalHitVelocity = Vector3.zero;

        Greenie.instance.Animator.SetTrigger("startAttack");
        Greenie.instance.Animator.SetBool("isAttacking", true);
        Greenie.instance.transform.Find("Attack Effect").GetComponent<Animator>().SetTrigger("Attack");
        Greenie.instance.Pushable += 1;
        Greenie.instance.AdditionalDamage += additionalDamage;
        Greenie.instance.AdditionalForce += additionalForce;
        
    }

    public override void EndEffect()
    {
        Greenie.instance.Animator.SetTrigger("endAttack");
        Greenie.instance.Animator.SetBool("isAttacking", false);
        Greenie.instance.Pushable -= 1;
        Greenie.instance.AdditionalDamage -= additionalDamage;
        Greenie.instance.AdditionalForce -= additionalForce;
    }
}
