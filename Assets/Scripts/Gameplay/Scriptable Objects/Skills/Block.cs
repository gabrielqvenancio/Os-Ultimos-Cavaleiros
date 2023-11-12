using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Objects/Skills/Block")]
public class Block : ScriptableSkill
{
    public Vector3 additionalForce;

    public override void Effect()
    {
        Greenie.instance.Animator.SetTrigger("startBlock");
        Greenie.instance.Animator.SetBool("isBlocking", true);
        Greenie.instance.AdditionalForce += additionalForce;
        Greenie.instance.ReducedDamagePercentage += 100;
        Greenie.instance.Pushable += 1;
    }

    public override void EndEffect()
    {
        Greenie.instance.Animator.SetTrigger("endBlock");
        Greenie.instance.Animator.SetBool("isBlocking", false);
        Greenie.instance.AdditionalForce -= additionalForce;
        Greenie.instance.ReducedDamagePercentage -= 100;
        Greenie.instance.Pushable -= 1;
    }
}
