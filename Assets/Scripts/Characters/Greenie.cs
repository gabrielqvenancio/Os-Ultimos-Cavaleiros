using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenie : Character
{
    internal static Greenie instance;

    [SerializeField] internal ScriptableCharacter attributes;
    [SerializeField] private ScriptableSkill[] skillsSelected;
    const int baseSkillAmount = 3;
    private int totalHealth, totalArmor;
    private bool isPushable;

    internal Skill[] Skills { get; private set; }
    internal Vector3 localHitvelocity;


    private void Awake()
    {
        instance = this;
        Skills = new Skill[baseSkillAmount];
        for(int i = 0; i < Skills.Length; i++) 
        {
            Skills[i] = new Skill(skillsSelected[i]);
        }

        BoxCollider = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();

        totalHealth = attributes.health;
        totalArmor = attributes.armor;
        currentHealth = totalHealth;
        currentArmor = totalArmor;
        isPushable = true;
        localHitvelocity = Vector3.zero;
    }

    private void Start()
    {
        healthBar.ApplyHealthRange(0, totalHealth);
    }

    private void Update()
    {
        if(localHitvelocity != Vector3.zero) 
        {
            Animator.SetBool("isPushed", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();

        enemyHit.TakeDamage(attributes.damage);
        TakeDamage(enemyHit.attributes.damage);
        if(enemyHit.gameObject.activeSelf)
        {
            GameHandler.instance.ApplyAccelerationOnHit(enemyHit);
            Animator.SetBool("isPushed", true);
            Animator.SetTrigger("gotHit");
        }
    }

    protected override void OnElimination()
    {
        Application.Quit();
    }
}