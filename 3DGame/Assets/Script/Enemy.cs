using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif
//public enum EnemyState { Find,Attack,Dead}
public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget;
    public ParticleSystem hitEffect;
    public float damage = 20.0f;
    public float timeBetAttack = 0.5f;

    public Transform attackRoot;
    public Transform eyeTransform;

    public float attackRadius = 2f;
    public float speed;
    public float fieldOfView = 50f;

    bool isAttack;

    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;
    private Animator enemyAni;
    private float lastAttackTime;
    private bool isDead = false;

#if UNITY_EDITOR

   //private void OnDrawGizmosSelected()
   //{
   //    if (attackRoot != null)
   //    {
   //        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
   //        Gizmos.DrawSphere(attackRoot.position, attackRadius);
   //    }
   //
   //    var leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
   //    var leftRayDirection = leftRayRotation * transform.forward;
   //    Handles.color = new Color(1f, 1f, 1f, 0.2f);
   //    Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
   //}

#endif

    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead) 
            {
                return true;
            }
            return false;
        }
    }
    public void SetUp(float Hp,float damage, float speed, float attackSpeed) 
    {
        base.MaxHp = Hp;
        this.damage = damage;
        pathFinder.speed = speed;
        this.timeBetAttack = attackSpeed;
    }
    private void Awake()
    {
        enemyAni = GetComponent<Animator>();
        pathFinder = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        StartCoroutine(UpdatePath());
    }
    private void Update()
    {
        if (!dead) 
        {
            enemyAni.SetBool("Attack", isAttack);
            enemyAni.SetBool("HasTarget", hasTarget);
            enemyAni.SetFloat("Distance", pathFinder.remainingDistance);
        }
    }
    IEnumerator UpdatePath() 
    {
        while (!isDead) 
        {
            if (hasTarget&& !isAttack && pathFinder.enabled)
            {
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else 
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                for (int i = 0; i < colliders.Length; i++) 
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    if (livingEntity != null && !livingEntity.dead) 
                    {
                        targetEntity = livingEntity;

                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }

    }

    IEnumerator hitCoroutine() 
    {
        enemyAni.SetTrigger("hit");
        pathFinder.isStopped = true;
         pathFinder.velocity = Vector3.zero;
         yield return new WaitForSeconds(0.2f);
         if(!dead)
            pathFinder.isStopped = false;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead) 
        {
            //hitEffect.transform.position = hitPoint;
            //hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            //hitEffect.Play();
            StartCoroutine(hitCoroutine());
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public override void Die()
    {
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();

        for (int i = 0; i < enemyColliders.Length; i++) 
        {
            enemyColliders[i].enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAni.SetTrigger("Die");
    }
    private void OnTriggerStay(Collider other)
    {
        pathFinder.velocity = Vector3.zero;
        if (!isDead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            if (attackTarget != null && attackTarget == targetEntity && hasTarget)
            {
                isAttack = true;
                lastAttackTime = Time.time;

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                //attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
        else
            isAttack = false;
    }
}
