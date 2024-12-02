using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyBase
{
    [SerializeField] private float reloadTime;

    [SerializeField] private Arrow arrow;

    [SerializeField] private Aimer aimer;

    public override void Start()
    {
        base.Start();
        //Start aiming loop
        StartCoroutine(Aiming());
    }

    public override void Update()
    {
        base.Update();
        if (hittable)
        {
            //If in range, look at player
            if (playerInRange && allowedToMove)
            {
                animator.SetBool("PlayerInRange", true);
                base.SetPlayerPosition();
                transform.forward = playerPosition - transform.position;
            }
            else
            {
                animator.SetBool("PlayerInRange", false);
                transform.forward = transform.forward;
            }
        }
    }

    private IEnumerator Aiming()
    {
        //If in range, wait, and then shoot
        if (playerInRange && allowedToMove)
        {
            yield return new WaitForSeconds(reloadTime);
            if (hittable && allowedToMove)
            {
                aimer.Shoot();
                animator.SetTrigger("Attack");
            }
        }
        //or if not in range, repeat unitl in range
        yield return new WaitForSeconds(0.01f);
        //and keep looping
        StartCoroutine(Aiming());
    }


}