using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    internal static Inventory instance;

    [SerializeField] internal ScriptableSkill[] allSkills;
    internal int Money { get; set; }
    internal int[] SkillsLevel { get; set; }

    private void Awake()
    {
        instance = this;
        SkillsLevel = new int[allSkills.Length];

        IOHandler.LoadMoney();
        IOHandler.LoadSkillsLevel();
    }

    internal ScriptableSkill GetAttributesBySkillType(SkillType type)
    {
        foreach (ScriptableSkill attributes in allSkills)
        {
            if(attributes.type == type)
            {
                return attributes;
            }
        }
        return null;
    }
}
