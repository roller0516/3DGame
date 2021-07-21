using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : LivingEntity , IDamageable
{
    [SerializeField]
    GameObject turret;
    Animation ani;

    public LayerMask whatIsTarget;
    public ParticleSystem hitEffect;
    public float damage = 50.0f;
    public float timeBetAttack = 0.5f;

    public Transform barrelR;
    public Transform barrelL;

    public float attackRadius = 2f;
    public float speed;
    public float fieldOfView = 50f;

    public ParticleSystem bullet;

    bool isAttack;

    private int attackCount = 0;
    private LivingEntity targetEntity;
    private Animator enemyAni;
    private float lastAttackTime;
    private bool isDead = false;

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
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animation>();
        StartCoroutine(FindEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEntity) 
        {
            turret.transform.LookAt(targetEntity.transform);
            if (!isDead && Time.time >= lastAttackTime + timeBetAttack && !targetEntity.dead)
            {
                lastAttackTime = Time.time;
                if (attackCount == 0)
                {
                    Instantiate(bullet, barrelR.position, turret.transform.rotation);
                    attackCount = 1;
                }
                else if (attackCount == 1)
                {
                    Instantiate(bullet, barrelL.position, turret.transform.rotation);
                    attackCount = 0;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Environment")
            ani.Play("Turret_v1_activation");
    }
    IEnumerator FindEnemy() 
    {
        while (!dead) 
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, whatIsTarget);
            for (int i = 0; i < colliders.Length; i++)
            {
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                if (livingEntity != null && !livingEntity.dead)
                {
                    targetEntity = livingEntity;

                    break;
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
