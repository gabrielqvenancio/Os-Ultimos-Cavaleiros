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
        PhysicsHandler.instance.AddGlobalVelocity(-additionalVelocity * Character.basePushForceFactor, - additionalVelocity * (Character.basePushForceFactor / duration));
        Greenie.instance.LocalHitVelocity = Vector3.zero;

        Greenie.instance.Animator.SetTrigger("startAttack");
        Greenie.instance.Pushable = false;
        Greenie.instance.AdditionalDamage += additionalDamage;
        Greenie.instance.AdditionalForce += additionalForce;
        
    }

    public override void EndEffect()
    {
        Greenie.instance.Animator.SetTrigger("endAttack");
        Greenie.instance.Pushable = true;
        Greenie.instance.AdditionalDamage -= additionalDamage;
        Greenie.instance.AdditionalForce -= additionalForce;
    }
}
