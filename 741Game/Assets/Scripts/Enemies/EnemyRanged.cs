using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private Movement player;
    
    private Vector3 playerPosition;
    [SerializeField] private PlayerDetector pd;
    private bool playerInRange;

    [SerializeField] private float reloadTime;

    [SerializeField] private Arrow arrow;

    private void Start()
    {
        ///Setup objects from item manager
        player = ItemManager.singleton.Player;

        ///Start aiming loop
        StartCoroutine(Aiming());
    }

    private void Update()
    {
        ///Set target position to ignore height (Looks good in isometric, may cause issues if we toy with height in level design?)
        playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        ///Check if player in range
        playerInRange = pd.playerInRange;

        ///If in range, look at player
        if (playerInRange)
        {
            transform.forward = playerPosition - transform.position;
        }
    }

    private IEnumerator Aiming()
    {
        ///If in range, wait, and then shoot
        if (playerInRange)
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