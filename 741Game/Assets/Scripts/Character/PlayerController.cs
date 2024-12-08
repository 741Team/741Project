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
    [SerializeField] KeyCode dashKey;
    private bool canDash;
    private bool dashing;

    [Header("Attack Attributes")]
    [SerializeField] private float damage;
    [SerializeField] private float knockBackForce;
    private bool AllowedToAttack;
    [SerializeField] private SwordHit swordHit;
    [SerializeField] private float attackCooldown;

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

    [Header("Level")]
    [SerializeField] private bool Village;
    [SerializeField] private bool Woods;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footstep;
    [SerializeField] private float footstepRate;
    private float footstepTimer;
    [SerializeField] private AudioClip swordSound;
    [SerializeField] private AudioClip castSound;

    [Header("Debug")]
    [SerializeField] Vector3 look;

    void Start()
    {
        CheckComponents();

        swordHit.SetSword(damage, knockBackForce, transform);

        ///Setup Health System
        health = maxHealth;
        healthBar.Setup(maxHealth);
        AllowedToMove = true;

        focusBar.Setup(maxFocus);
        focusBar.SetValue(0);

        ///Setup dash bar
        if (Woods == true)
        {
            dashBar.transform.Rotate(0f, 270f, 0f);
        }
        dashBar.transform.SetParent(canvas.transform);
        canDash = true;
        dashBar.Setup(100);
        DashBar.SetActive(false);
        dashing = false;

        AllowedToJump = true;
        AllowedToAttack = true;

        _characterAnimations.SetAttackTime(attackCooldown + 0.5f);

        hittable = true;

        focus = 0;
        footstepTimer = 0;
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
        if (swordHit == null)
        {
            swordHit = GetComponentInChildren<SwordHit>();
        }
        if (canvas == null)
        {
            canvas = ItemManager.singleton.enemyCanvas;
        }
        if (_characterAnimations == null)
        {
            _characterAnimations = GetComponent<CharacterAnimations>();
        }
    }

    void Update()
    {
        if (AllowedToMove)
        {
            if (Input.GetKeyDown(dashKey) & canDash)
            {
                dashing = true;
                StartCoroutine(Dodge());
            }

            if (Village == true)
            {
                movement.x = Input.GetAxisRaw("Horizontal") * -1f;
                movement.z = Input.GetAxisRaw("Vertical") * -1f;
            }

            if (Woods == true)
            {
                movement.x = Input.GetAxisRaw("Vertical") * -1f;
                movement.z = Input.GetAxisRaw("Horizontal") * 1f;
            }

            if (Input.GetButtonDown("Jump") & AllowedToJump)
            {
                Jump();
                AllowedToJump = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) & AllowedToAttack)
            {
                StartCoroutine(Attack());
            }

            //Set rotation vectors for isometric movement
            /*
            if (movement.x == 1 & movement.z == 1)
            {
                look = new Vector3(0, 45, 0);
            }
            else if (movement.x == 1 & movement.z == 0)
            {
                look = new Vector3(0, 90, 0);
            }
            else if (movement.x == 1 & movement.z == -1)
            {
                look = new Vector3(0, 135, 0);
            }
            else if (movement.x == 0 & movement.z == -1)
            {
                look = new Vector3(0, 180, 0);
            }
            else if (movement.x == -1 & movement.z == -1)
            {
                look = new Vector3(0, 225, 0);
            }
            else if (movement.x == -1 & movement.z == 0)
            {
                look = new Vector3(0, 270, 0);
            }
            else if (movement.x == -1 & movement.z == 1)
            {
                look = new Vector3(0, 315, 0);
            }
            else if (movement.x == 0 & movement.z == 1)
            {
                look = new Vector3(0, 360, 0);
            }*/

            UpdateMousePosition();

            if (_characterAnimations != null)
            {
                _characterAnimations.SetSpeed(movement.magnitude);
            }

            ChangeFocus(focusRefreshRate * Time.deltaTime);

            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

            FootstepSounds();
        }

    }

    void UpdateMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, groundLayers))
        {
            Vector3 target = hit.point;
            target.y = transform.position.y;

            // Make the character face the target point
            transform.LookAt(target);

            // Calculate the normalized direction vector
            Vector3 direction = (target - transform.position).normalized;

            // Calculate the forward and right components using dot products
            float forward = Vector3.Dot(movement.normalized, direction);
            float right = Vector3.Dot(movement.normalized, Vector3.Cross(Vector3.up, direction));

            // Ensure forward and right values are in the range [-1, 1] (optional, based on requirements)
            forward = Mathf.Clamp(forward, -1f, 1f);
            right = Mathf.Clamp(right, -1f, 1f);

            // Set the character's animation direction
            _characterAnimations.SetRunDirection(new Vector3(right, 0, forward));
        }
    }

    /*
    void FixedUpdate()
    {
        if (AllowedToMove == true)
        {
            //Setting up isometric movement
            transform.rotation = Quaternion.Euler(look);
            IsoMovement = IsomentricConverter(movement);
            rb.MovePosition(transform.position + IsoMovement * speed * Time.fixedDeltaTime);
        }
    }
    */

    /*
    private Vector3 IsomentricConverter(Vector3 vector)
    {
        //Setting up isometric movement pt 2, the sequel
        Quaternion rotation = Quaternion.Euler(0f,45f,0f);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }
    */

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
        Vector3 dashDirection = new Vector3(movement.x, 0, movement.z);
        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }
        rb.AddForce(dashDirection * dashPower, ForceMode.Impulse);
        trail.emitting = true;
        dashing = true;
        yield return new WaitForSeconds(dashTime);
        trail.emitting = false;
        dashing = false;
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
    }

    public void PlayCastSound()
    {
        audioSource.PlayOneShot(castSound);
    }

    private void FootstepSounds()
    {
        if (movement != Vector3.zero)
        {
            if(footstepTimer <= 0)
            {
                audioSource.PlayOneShot(footstep, 0.5f);
                footstepTimer = footstepRate;
            }
            else
            {
                footstepTimer -= Time.deltaTime;
            }
        }

    }

    private IEnumerator Attack()
    {
        AllowedToAttack = false;
        AllowedToMove = false;
        if (_characterAnimations != null)
        {
            _characterAnimations.Attack();
        }
        audioSource.PlayOneShot(swordSound);
        rb.AddForce(transform.forward * 7, ForceMode.Impulse);
        var swordRenderer = sword.GetComponentInChildren<Renderer>();
        swordTrail.StartTrail();
        Color red = new Color(1f, 0f, 0f, 1f);
        swordRenderer.material.SetColor("_Color", red);
        yield return new WaitForSeconds(attackCooldown);
        Color white = new Color(1f, 1f, 1f, 1f);
        swordRenderer.material.SetColor("_Color", white);
        swordTrail.StopTrail();
        AllowedToAttack = true;
        AllowedToMove = true;
    }

    private IEnumerator Hit(float damage, float knockBackForce, Transform attacker)
    {
        health = health - damage;
        healthBar.Decrease(damage);
        Vector3 direction = (transform.position - attacker.position).normalized;
        rb.AddForce(direction * knockBackForce, ForceMode.Impulse);
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


    public void OnEnemyHit(float damage, float knockBackForce, Transform attacker)
    {
        StartCoroutine(Hit(damage, knockBackForce, attacker));


    }
}
