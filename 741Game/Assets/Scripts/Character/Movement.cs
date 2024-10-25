using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private Bar healthBar;
    [SerializeField] private float immunityTime;
    private bool hittable;

    [SerializeField] private float dashTime;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashCooldown;
    private bool canDash;

    [SerializeField] private Bar focusBar;

    private Canvas canvas;
    Vector3 movement;
    Vector3 IsoMovement;

    [SerializeField] Vector3 look;

    [SerializeField] private bool AllowedToMove;

    [SerializeField] private TrailRenderer trail;

    [SerializeField] private Bar dashBar;
    [SerializeField] GameObject DashBar;

    public Transform feet;
    private bool AllowedToJump;
    public LayerMask groundLayers;

    [SerializeField] private float maxDamage;
    public float damage;
    private bool AllowedToAttack;

    [SerializeField] private GameObject sword;
    
    private CharacterAnimations _characterAnimations;

    [SerializeField] private SwordTrail swordTrail;

    void Start()
    {
        ///Setup Health System
        health = maxHealth;
        healthBar.Setup(maxHealth);

        focusBar.Setup(10000);

        AllowedToMove = true;

        canvas = ItemManager.singleton.enemyCanvas;

        ///Setup dash bar
        dashBar.transform.SetParent(canvas.transform);
        canDash = true;
        dashBar.Setup(100);
        DashBar.SetActive(false);

        AllowedToJump = true;
        AllowedToAttack = true;

        hittable = true;

        damage = 0;
        _characterAnimations = GetComponent<CharacterAnimations>();
    }

    void Update()
    {
        ///Set Movement
        movement.x = Input.GetAxisRaw("Horizontal")*-1f;
        movement.z = Input.GetAxisRaw("Vertical")*-2f;
        
        if (Input.GetButtonDown("Jump") & AllowedToJump & AllowedToMove)
        {
            Jump();
            AllowedToJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) & canDash & AllowedToMove)
        {
            StartCoroutine(Dodge());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) & AllowedToMove & AllowedToAttack)
        {
            StartCoroutine(Attack());
        }

        ///Set rotation vectors for isometric movement
        if (movement.x == 1 & movement.z == 2)
        {
            look = new Vector3(0,90,0);
        }
        else if (movement.x == 1 & movement.z == 0)
        {
            look = new Vector3(0,135,0);
        }
        else if (movement.x == 1 & movement.z == -2)
        {
            look = new Vector3(0,180,0);
        }
        else if (movement.x == 0 & movement.z == -2)
        {
            look = new Vector3(0,225,0);
        }
        else if (movement.x == -1 & movement.z == -2)
        {
            look = new Vector3(0,270,0);
        }
        else if (movement.x == -1 & movement.z == 0)
        {
            look = new Vector3(0,315,0);
        }
        else if (movement.x == -1 & movement.z == 2)
        {
            look = new Vector3(0,360,0);
        }
        else if (movement.x == 0 & movement.z == 2)
        {
            look = new Vector3(0,45,0);
        }

        if(_characterAnimations != null && AllowedToMove)
        {
            _characterAnimations.SetSpeed(movement.magnitude);
        }
    }

    void FixedUpdate()
    {
        if (AllowedToMove == true)
        {
            ///Setting up isometric movement
            transform.rotation = Quaternion.Euler(look);
            IsoMovement = IsomentricConverter(movement);
            rb.MovePosition(transform.position + IsoMovement * speed * Time.fixedDeltaTime);
        }
    }
    
    private Vector3 IsomentricConverter(Vector3 vector)
    {
        //Setting up isometric movement pt 2, the sequel
        Quaternion rotation = Quaternion.Euler(0f,45f,0f);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }

    private void Jump()
    {
        Vector3 movement = new Vector2(rb.velocity.y, jumpForce);

        rb.velocity = movement;
    }

    private IEnumerator Dodge()
    {
        canDash = false;
        DashBar.SetActive(true);
        dashBar.SetValue(0);
        IsoMovement = IsomentricConverter(movement);
        rb.velocity = new Vector3(IsoMovement.x * dashPower,0,IsoMovement.z * dashPower);
        trail.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trail.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        DashBar.SetActive(false);
        canDash = true;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Ground")
        {
            AllowedToJump = true;
        }

        if (collider.tag == "Enemy" & hittable)
        {
            hittable = false;
            StartCoroutine(Hit(20));
        }

        if (collider.tag == "Arrow" & hittable)
        {
            hittable = false;
            StartCoroutine(Hit(10));
        }
    }

    private IEnumerator Attack()
    {
        AllowedToAttack = false;
        AllowedToMove = false;
        if(_characterAnimations != null)
        {
            _characterAnimations.Attack();
        }
        damage = maxDamage;
        var swordRenderer = sword.GetComponent<Renderer>();
        swordTrail.StartTrail();
        Color red = new Color(1f, 0f, 0f, 1f);
        swordRenderer.material.SetColor("_Color", red);
        yield return new WaitForSeconds(1f);
        Color white = new Color(1f, 1f, 1f, 1f);
        swordRenderer.material.SetColor("_Color", white);
        swordTrail.StopTrail();
        damage = 0;
        AllowedToAttack = true;
        AllowedToMove = true;
    }

    private IEnumerator Hit(float damage)
    {
        health = health - damage;
        healthBar.Decrease(damage);
        yield return new WaitForSeconds(immunityTime);
        hittable = true;
    }

}
