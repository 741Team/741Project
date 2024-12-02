using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeMovement : EnemyBase
{
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float speed;
    [SerializeField] private float movementFrequency;



    public override void Start()
    {
        base.Start();
        ///Setup objects from item manager
        playerPosition = transform.position;

        ///Setup movement loop
        StartCoroutine(MoveToPlayer());
    }

    public override void Update()
    {
        base.Update();
        ///move to player, if in range
        if (playerInRange && allowedToMove)
        {
            transform.LookAt(playerPosition);
            transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, speed);
            animator.SetFloat("Movement", 1);
        }
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
