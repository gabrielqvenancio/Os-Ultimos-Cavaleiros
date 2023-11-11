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
    internal bool Pushable { private get; set; }
    internal int AdditionalDamage { get; set; }
    internal Vector3 AdditionalForce { get; set; }

    private void Awake()
    {
        instance = this;

        Pushable = true;
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

        enemyHit.TakeDamage(Attributes.damage + AdditionalDamage);
        TakeDamage((int)((100f - reducedDamagePercentage) / 100f) * enemyHit.Attributes.damage);

        if(enemyHit.gameObject.activeSelf)
        {
            PhysicsHandler.instance.PushCharacter(Attributes.pushForce + AdditionalForce, enemyHit);
            if(Pushable)
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
        SceneHandler.instance.GameOver();
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
        foreach(Skill skill in skills)
        {
            if(skill.Id == id && skill.State == SkillState.ready)
            {
                skill.Use();
            }
        }
    }
}