using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyBase
{
    [SerializeField] private float reloadTime;

    [SerializeField] private Arrow arrow;

    public override void Start()
    {
        base.Start();
        ///Start aiming loop
        StartCoroutine(Aiming());
    }

    public override void Update()
    {
        base.Update();

        ///If in range, look at player
        if (playerInRange && allowedToMove)
        {
            transform.forward = playerPosition - transform.position;
        }
    }

    private IEnumerator Aiming()
    {
        ///If in range, wait, and then shoot
        if (playerInRange && allowedToMove)
        {
            yield return new WaitForSeconds(reloadTime);
            Shoot();
        }
        ///or if not in range, repeat unitl in range
        yield return new WaitForSeconds(0.01f);
        ///and keep looping
        StartCoroutine(Aiming());
    }

    private void Shoot()
    {
        ///Create arrow prefab, and set direction
        Arrow arrows = Instantiate(arrow, transform.position, transform.rotation);
        arrows.Project(transform.forward);
    }
}