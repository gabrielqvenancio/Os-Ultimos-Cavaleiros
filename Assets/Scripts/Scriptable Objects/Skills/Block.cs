using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Objects/Skills/Block")]
public class Block : ScriptableSkill
{
    public Vector3 additionalForce;

    public override void Effect()
    {
        Greenie.instance.transform.Find("Shield").gameObject.SetActive(true);
        Greenie.instance.AdditionalForce += additionalForce;
        Greenie.instance.ReducedDamagePercentage += 100;
        Greenie.instance.isPushable = false;
    }

    public override void EndEffect()
    {
        Greenie.instance.transform.Find("Shield").gameObject.SetActive(false);
        Greenie.instance.AdditionalForce -= additionalForce;
        Greenie.instance.ReducedDamagePercentage -= 100;
        Greenie.instance.isPushable = true;
    }
}
