using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IFreezable
{
    [Header("Movement Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private bool AllowedToMove;
    private bool AllowedToJump;
    Vector3 movement;
    Vector3 IsoMovement;

    [Header("Health Attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float immunityTime;
    private bool hittable;
    private float health;

    [Header("Dash Attributes")]
    [SerializeField] private float dashTime;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashCooldown;
    private bool canDash;

    [Header("Attack Attributes")]
    [SerializeField] private float maxDamage;
    public float damage;
    private bool AllowedToAttack;

    [Header("Focus Attributes")]
    [SerializeField] protected float maxFocus;
    [SerializeField] protected float focusDecreaseRate;
    [SerializeField] protected float focusRefreshRate;
    protected float focus;

    [Header("Components")]
    [SerializeField] private Bar focusBar;
    public Rigidbody rb;
    [SerializeField] private Bar healthBar;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Bar dashBar;
    [SerializeField] GameObject DashBar;
    public LayerMask groundLayers;
    public Transform feet;
    private Canvas canvas;
    [SerializeField] private GameObject sword = null;
    private CharacterAnimations _characterAnimations;
    private SwordTrail swordTrail;



    [Header("Debug")]
    [SerializeField] Vector3 look;

    void Start()
    {
        CheckComponents();

        ///Setup Health System
        health = maxHealth;
        healthBar.Setup(maxHealth);
        AllowedToMove = true;

        focusBar.Setup(maxFocus);
        focusBar.SetValue(0);

        ///Setup dash bar
        dashBar.transform.SetParent(canvas.transform);
        canDash = true;
        dashBar.Setup(100);
        DashBar.SetActive(false);

        AllowedToJump = true;
        AllowedToAttack = true;

        hittable = true;

        damage = 0;

        focus = 0;
    }

    void CheckComponents()
    {
        if (healthBar == null)
        {
            healthBar = ItemManager.singleton.healthBar;
        }
        if (focusBar == null)
        {
            focusBar = ItemManager.singleton.focusBar;
        }
        if (swordTrail == null)
        {
            swordTrail = GetComponentInChildren<SwordTrail>();
        }
        if(canvas == null)
        {
            canvas = ItemManager.singleton.enemyCanvas;
        }
        if(_characterAnimations == null)
        {
            _characterAnimations = GetComponent<CharacterAnimations>();
        }
    }

    void Update()
    {
        if (AllowedToMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal") * -1f;
            movement.z = Input.GetAxisRaw("Vertical") * -2f;

            if (Input.GetButtonDown("Jump") & AllowedToJump)
            {
                Jump();
                AllowedToJump = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) & canDash)
            {
                StartCoroutine(Dodge());
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) & AllowedToAttack)
            {
                StartCoroutine(Attack());
            }

            ///Set rotation vectors for isometric movement
            if (movement.x == 1 & movement.z == 2)
            {
                look = new Vector3(0, 90, 0);
            }
            else if (movement.x == 1 & movement.z == 0)
            {
                look = new Vector3(0, 135, 0);
            }
            else if (movement.x == 1 & movement.z == -2)
            {
                look = new Vector3(0, 180, 0);
            }
            else if (movement.x == 0 & movement.z == -2)
            {
                look = new Vector3(0, 225, 0);
            }
            else if (movement.x == -1 & movement.z == -2)
            {
                look = new Vector3(0, 270, 0);
            }
            else if (movement.x == -1 & movement.z == 0)
            {
                look = new Vector3(0, 315, 0);
            }
            else if (movement.x == -1 & movement.z == 2)
            {
                look = new Vector3(0, 360, 0);
            }
            else if (movement.x == 0 & movement.z == 2)
            {
                look = new Vector3(0, 45, 0);
            }

            if (_characterAnimations != null)
            {
                _characterAnimations.SetSpeed(movement.magnitude);
            }

            ChangeFocus(focusRefreshRate * Time.deltaTime);
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
        var swordRenderer = sword.GetComponentInChildren<Renderer>();
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

    public float GetFocus()
    {
        return focus;
    }

    public float GetMaxFocus()
    {
        return maxFocus;
    }

    public float GetFocusDecreaseRate()
    {
        return focusDecreaseRate;
    }

    public void ChangeFocus(float change)
    {
        focus = Mathf.Clamp(focus + change, 0, maxFocus);
        focusBar.SetValue(focus);
    }

    public bool IsAllowedToMove()
    {
        return AllowedToMove;
    }   

    public void Freeze()
    {
        AllowedToMove = false;
    }

    public void Unfreeze()
    {
        AllowedToMove = true;
    }

}
