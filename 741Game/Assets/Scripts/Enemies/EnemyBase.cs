using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IFreezable
{
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private Bar healthBar;
    [SerializeField] private GameObject HealthBar;
    private Canvas canvas;
    [SerializeField] private float immunityTime;
    private bool hittable;
    private Bar focusBar;
    protected bool allowedToMove;

    protected PlayerController player;
    private bool touchingPlayer;
    protected bool playerInRange;
    [SerializeField] private PlayerDetector pd;
    protected Vector3 playerPosition;
    [SerializeField] private float focusOnHit;

    public virtual void Start()
    {
        if(EnemyManager.singleton != null)
        {
            EnemyManager.singleton.AddEnemy(this);
        }
        ///Setup objects from item manager
        player = ItemManager.singleton.Player;
        canvas = ItemManager.singleton.enemyCanvas;
        focusBar = ItemManager.singleton.focusBar;

        ///Setup health
        health = maxHealth;
        healthBar.Setup(maxHealth);
        healthBar.transform.SetParent(canvas.transform);

        if(pd == null)
        {
            pd = GetComponentInChildren<PlayerDetector>();
        }

        allowedToMove = true;
    }

    public virtual void Update()
    {

        playerInRange = pd.playerInRange;
        playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

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
        health = health - damage;
        player.ChangeFocus(focusOnHit);
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

    public void BoltHit(float damage)
    {
        health = health - damage;
        healthBar.Decrease(damage);
    }

    public void Freeze()
    {
        allowedToMove = false;
    }

    public void Unfreeze()
    {
        allowedToMove = true;
    }
}