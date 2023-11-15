using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenie : Character
{
    internal static Greenie instance;

    [SerializeField] private Skill[] skills;
    [SerializeField] private AudioClip[] hurtSounds;

    private float reducedDamagePercentage;
    internal float ReducedDamagePercentage
    {
        get { return Mathf.Clamp(reducedDamagePercentage, 0, 100); }
        set { reducedDamagePercentage = value; }
    }
    internal int Pushable { get; set; }
    internal int AdditionalDamage { get; set; }
    internal Vector3 AdditionalForce { get; set; }

    private void Awake()
    {
        instance = this;

        Pushable = 0;
        AdditionalDamage = 0;
        AdditionalForce = Vector3.zero;
        reducedDamagePercentage = 0;

        Initialize();
    }

    private void Start()
    {
        healthBar.ApplyHealthRange(0, Attributes.health);

        for(int i = 0; i < skills.Length; i++)
        {
            skills[i].Id = i;
            skills[i].Source = SoundHandler.instance.skillSources[i];
            skills[i].Source.clip = skills[i].Attributes.sound;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Deceleration();
    }

    private void LateUpdate()
    {
        healthBar.UpdateHealth(CurrentHealth);

        foreach(Skill skill in skills)
        {
            if (skill.Timer > 0)
            {
                skill.Timer -= Time.deltaTime;
            }
            skill.CheckState();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();

        TakeDamage((int)((100f - reducedDamagePercentage) / 100f) * enemyHit.Attributes.damage);
        if(CurrentHealth <= 0)
        {
            return;
        }
        enemyHit.TakeDamage(Attributes.damage + AdditionalDamage);

        if(enemyHit.CurrentHealth > 0)
        {
            PhysicsHandler.instance.PushCharacter(Attributes.pushForce + AdditionalForce, enemyHit);
            if(Pushable == 0)
            {
                SoundHandler.instance.PlaySoundEffect(GetComponent<AudioSource>(), hurtSounds[Random.Range(0, hurtSounds.Length)]);
                PhysicsHandler.instance.PushCharacter(enemyHit.Attributes.pushForce, this);
            }
        }

        SoundHandler.instance.PlayHitSound();
    }

    protected override void Move()
    {
        LevelHandler.instance.Move(LevelHandler.instance.EnvironmentObjectsParent);
    }

    protected override void OnElimination()
    {
        IOHandler.SaveHighScore();
        StartCoroutine(GameoverScript.instance.GameOver());
    }

    internal override void OnPush()
    {
        recovery = new Vector3(Attributes.resistance, 0, 0);
        Animator.SetBool("isPushed", true);
        Animator.SetTrigger("gotHit");
    }

    internal override void OnRecover()
    {
        Animator.SetBool("isPushed", false);
    }

    public void ClickOnSkill(int id)
    {
        if(SceneHandler.instance.State != GameState.gameplay)
        {
            return;
        }

        foreach(Skill skill in skills)
        {
            if(skill.Id == id && skill.State == SkillState.ready)
            {
                skill.Use();
            }
        }
    }
}