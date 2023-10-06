using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillEffects
{
    internal static SkillEffect AssignSkillEffect(int id)
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

    internal static void Attack()
    {

    }

    internal static void Block()
    {

    }

    internal static void Potion()
    {

    }
}
