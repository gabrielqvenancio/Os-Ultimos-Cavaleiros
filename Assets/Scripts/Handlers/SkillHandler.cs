using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    internal static SkillHandler instance;

    private const int numberOfActiveSkills = 3;
    private Skill[] activeSkills;

    private void Start()
    {
        instance = this;

        activeSkills = new Skill[numberOfActiveSkills];
        for (int i = 0; i < numberOfActiveSkills; i++)
        {
            activeSkills[i] = new Skill(Inventory.instance.allSkills[i].type, i);
        }
    }

    private void LateUpdate()
    {
        foreach (Skill skill in activeSkills)
        {
            if (skill.Timer > 0)
            {
                skill.Timer -= Time.deltaTime;
            }
            skill.CheckState();
        }
    }

    public void ClickOnSkill(int id)
    {
        if (SceneHandler.instance.State != GameState.gameplay)
        {
            return;
        }

        foreach (Skill skill in activeSkills)
        {
            if (skill.ButtonId == id && skill.State == SkillState.ready)
            {
                skill.Use();
            }
        }
    }
}
