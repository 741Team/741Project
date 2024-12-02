using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IFreezable
{
    [Header("Enemy Base")]
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private Bar healthBar;
    [SerializeField] private GameObject HealthBar;
    private Canvas canvas;
    [SerializeField] private float immunityTime;
    protected bool hittable;
    private Bar focusBar;
    protected bool allowedToMove;
    [SerializeField] protected Animator animator;
    protected PlayerController player;
    private bool touchingPlayer;
    protected bool playerInRange;
    [SerializeField] private PlayerDetector pd;
    protected Vector3 playerPosition;
    [SerializeField] private float focusOnHit;
    [SerializeField] private ParticleSystem boltParticle;

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

        animator = GetComponentInChildren<Animator>();
        allowedToMove = true;
        hittable = true;
    }

    public virtual void Update()
    {

        playerInRange = pd.playerInRange;

        if (health <= 0)
        {
            Death();
        }
    }

    private IEnumerator Hit(float damage, float knockback, Transform attacker)
    {
        hittable = false;
        health = health - damage;
        Vector3 direction = transform.position - attacker.position;
        GetComponent<Rigidbody>().AddForce(direction.normalized * knockback, ForceMode.Impulse);
        player.ChangeFocus(focusOnHit);
        healthBar.Decrease(damage);
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(immunityTime);
        hittable = true;
    }

    private void Death()
    {
        animator.SetBool("Dead", true);
        Destroy(HealthBar);
        Destroy(this);
    }

    public void OnPlayerHit(float damage, float knockback, Transform attacker)
    {
        StartCoroutine(Hit(damage, knockback, attacker));
    }

    public void BoltHit(float damage)
    {
        StartCoroutine(Hit(damage, 0, transform));
        boltParticle.Play();
    }

    public void Freeze()
    {
        allowedToMove = false;
    }

    public void Unfreeze()
    {
        allowedToMove = true;
    }

    public void SetPlayerPosition()
    {
        playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }
}