using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    internal static Potion instance;

    internal Skill skillInstance;
    internal ScriptableSkill Attributes;
    [SerializeField] private int baseHealthRestored, additionalHealthRestoredPerLevel;

    private void Start()
    {
        instance = this;
        Attributes = Inventory.instance.allSkills[(int)SkillType.Potion];
    }

    internal void Effect()
    {
        Greenie.instance.CurrentHealth += baseHealthRestored + (Inventory.instance.SkillsLevel[(int)Attributes.type] - 1) * additionalHealthRestoredPerLevel;
        Greenie.instance.transform.Find("Potion Effect").GetComponent<Animator>().SetTrigger("UsePotion");
    }

    internal void EndEffect()
    {

    }
}
