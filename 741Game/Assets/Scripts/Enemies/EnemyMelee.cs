using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    [Header("Movement Variables")]
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float speed;
    [SerializeField] private float movementFrequency;

    [Header("Melee Variables")]
    [SerializeField] private SphereCollider meleeRangeChecker;
    [SerializeField] private float attackCooldown;
    [SerializeField] public bool inMeleeRange;

    [Header("Sword Variables")]
    [SerializeField] private SwordHit swordHit;
    [SerializeField] private float damage;
    [SerializeField] private float knockbackForce;
    bool attacking;



    public override void Start()
    {
        base.Start();
        meleeRangeChecker = GetComponent<SphereCollider>();
        ///Setup objects from item manager
        playerPosition = transform.position;
        inMeleeRange = false;
        attacking = false;
        if (swordHit == null)
        {
            swordHit = GetComponentInChildren<SwordHit>();
        }
        swordHit.SetSword(damage, knockbackForce, transform);

        ///Setup movement loop
        StartCoroutine(MoveToPlayer());
    }

    public override void Update()
    {
        base.Update();
        if (hittable)
        {
            if (inMeleeRange && !attacking)
            {
                Attack();
                animator.SetFloat("Movement", 0);
            }
            ///move to player, if in range
            if (playerInRange && allowedToMove)
            {
                Vector3 targetPostition = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
                transform.LookAt(targetPostition);
                transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, speed);
                animator.SetFloat("Movement", 1);
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        allowedToMove = false;
        attacking = true;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        allowedToMove = true;
        attacking = false;
    }

    private IEnumerator MoveToPlayer()
    {
        ///If in range, set target position to player position, and then wait
        if (playerInRange)
        {
            base.SetPlayerPosition();
            yield return new WaitForSeconds(movementFrequency);
        }
        ///or if not in range, repeat unitl in range
        yield return new WaitForSeconds(0.01f);
        ///and keep looping
        StartCoroutine(MoveToPlayer());
    }

}
