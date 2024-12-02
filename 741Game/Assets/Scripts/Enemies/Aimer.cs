using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    private PlayerController player;

    [SerializeField] private Arrow arrow;

    private void Start()
    {
        player = ItemManager.singleton.Player;
    }


    private void Update()
    {
        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z));
    }

    public void Shoot()
    {
        //Create arrow prefab, and set direction
        Arrow arrows = Instantiate(arrow, transform.position, transform.rotation);
        arrows.Project(transform.forward);
    }
}
