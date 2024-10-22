using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeeleMovement : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float speed;
    [SerializeField] private float movementFrequency;
    private bool playerInRange;

    private Movement player;

    [SerializeField] private EnemyBase Base;

    [SerializeField] private PlayerDetector pd;

    private Vector3 playerPosition;

    private void Start()
    {
        ///Setup objects from item manager
        player = ItemManager.singleton.Player;
        playerPosition = transform.position;

        ///Setup movement loop
        StartCoroutine(MoveToPlayer());
    }

    private void Update()
    {
        ///check if player is in range
        playerInRange = pd.playerInRange;

        ///move to player, if in range
        if (playerInRange)
        {
            Base.transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, speed);
        }
    }

    private IEnumerator MoveToPlayer()
    {
        ///If in range, set target position to player position, and then wait
        if (playerInRange)
        {
            playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            yield return new WaitForSeconds(movementFrequency);
        }
        ///or if not in range, repeat unitl in range
        yield return new WaitForSeconds(0.01f);
        ///and keep looping
        StartCoroutine(MoveToPlayer());
    }
}
