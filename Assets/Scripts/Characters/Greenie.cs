using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenie : Character
{
    internal static Greenie instance;

    [SerializeField] private ScriptableSkill[] skillsSelected;

    private const int baseSkillAmount = 3;
    private int totalHealth, totalArmor;

    internal bool isPushable { private get; set; }
    internal int AdditionalDamage { get; set; }
    internal Vector3 AdditionalForce { get; set; }
    internal Skill[] Skills { get; private set; }

    private void Awake()
    {
        instance = this;

        totalHealth = attributes.health;
        totalArmor = attributes.armor;
        //CurrentHealth = totalHealth;
        //CurrentArmor = totalArmor;
        isPushable = true;
        AdditionalDamage = 0;
        AdditionalForce = Vector3.zero;

        Initialize();
    }

    private void Start()
    {
        healthBar.ApplyHealthRange(0, totalHealth);

        Skills = new Skill[baseSkillAmount];
        for(int i = 0; i < Skills.Length; i++) 
        {
            Skills[i] = new Skill(skillsSelected[i]);
        }
    }

    private void FixedUpdate()
    {
        Move();
        Deceleration();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();

        enemyHit.TakeDamage(Attributes.damage + AdditionalDamage);
        TakeDamage(enemyHit.Attributes.damage);
        if(enemyHit.gameObject.activeSelf)
        {
            PhysicsHandler.instance.PushCharacter(Attributes.pushForce + AdditionalForce, enemyHit);
            if(isPushable) PhysicsHandler.instance.PushCharacter(new Vector3(enemyHit.Attributes.resistance, 0, 0), this);
        }
    }

    protected override void Move()
    {
        LevelHandler.instance.Move(LevelHandler.instance.EnvironmentObjectsParent);
    }

    protected override void OnElimination()
    {
        Application.Quit();
    }

    internal override void OnPush()
    {
        recovery = new Vector3(baseResistance + Attributes.resistance, 0, 0);
        Animator.SetBool("isPushed", true);
        Animator.SetTrigger("gotHit");
    }

    internal override void OnRecover()
    {
        Animator.SetBool("isPushed", false);
    }

    public void Teste()
    {
        Skills[0].SkillEffect(Skills[0]);
    }
}