using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Skills/Potion")]
public class Potion : ScriptableSkill
{
    public int healthRegained;

    public override void Effect()
    {
        Greenie.instance.CurrentHealth += healthRegained;
    }

    public override void EndEffect() { }
}
