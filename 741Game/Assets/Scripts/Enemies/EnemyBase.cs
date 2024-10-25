using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private Bar healthBar;
    [SerializeField] private GameObject HealthBar;
    private Canvas canvas;
    private Movement player;
    private bool touchingPlayer;
    [SerializeField] private float immunityTime;
    private bool hittable;
    private Bar focusBar;

    private void Start()
    {
        ///Setup objects from item manager
        player = ItemManager.singleton.Player;
        canvas = ItemManager.singleton.enemyCanvas;
        focusBar = ItemManager.singleton.focusBar;

        ///Setup health
        health = maxHealth;
        healthBar.Setup(maxHealth);
        healthBar.transform.SetParent(canvas.transform);
    }

    private void Update()
    {

        if (health <= 0)
        {
            Death();
        }

        if (hittable & touchingPlayer)
        {
            hittable = false;
            StartCoroutine(Hit(player.damage));
        }
    }

    private IEnumerator Hit(float damage)
    {
        ///deal damage, then wait before enemy can be hit again
        focusBar.Increase(1000);
        health = health - damage;
        healthBar.Decrease(damage);
        yield return new WaitForSeconds(immunityTime);
        hittable = true;
    }

    private void Death()
    {
        Destroy(HealthBar);
        Destroy(gameObject);
    }

    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Weapon")
        {
            touchingPlayer = true;
            hittable = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Weapon")
        {
            touchingPlayer = false;
            hittable = false;
        }
    }
}