using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IFreezable
{
    [Header("Movement Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float speedMagnitudeCap;
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
    [SerializeField] protected float focusMin;
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
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip dashSound;

    [Header("Cameras")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera mainCamera;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera focusCamera;

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
        if(health <= 0)
        {
            if(_characterAnimations != null)
            {
                _characterAnimations.Die();
                AllowedToMove = false;
            }
            if(ItemManager.singleton.gameOver != null)
            {
                ItemManager.singleton.gameOver.gameObject.SetActive(true);
            }
        }
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


            UpdateMouseScreenSpace();

            if (_characterAnimations != null)
            {
                _characterAnimations.SetSpeed(movement.magnitude);
            }

            ChangeFocus(focusRefreshRate * Time.deltaTime);



            if (!dashing)
            {
                rb.AddForce(movement * speed, ForceMode.VelocityChange);
                Vector2 velocity = new Vector2(rb.velocity.x, rb.velocity.z);
                velocity = Vector2.ClampMagnitude(velocity, speedMagnitudeCap);
                rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);
            }

            FootstepSounds();

            if(focus >= focusMin)
            {
                ItemManager.singleton.castReady.enabled = true;
                ItemManager.singleton.castNotReady.enabled = false;
            }
            else
            {
                ItemManager.singleton.castReady.enabled = false;
                ItemManager.singleton.castNotReady.enabled = true;
            }
        }

    }

    void UpdateMouseScreenSpace()
    {
        //Get the mouse position on the screen and set the player rotation accordingly
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (Woods == true)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
        }
        else if (Village == true)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, (-angle)+270f, 0));
        }

        //Calculate the direction vector based on the rotation and movement input
        Vector3 direction = new Vector3(movement.x, 0, movement.z);
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 moveDirection = forward * direction.z + (right * -1) * direction.x;

        _characterAnimations.SetRunDirection(moveDirection);
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
        if(dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }
        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }
        rb.AddForce(dashDirection * dashPower, ForceMode.VelocityChange);
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
                audioSource.PlayOneShot(footstep, 0.2f);
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
        if (_characterAnimations != null)
        {
            _characterAnimations.SetHit();
        }
        if(hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
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


    public void OnHitByEnemy(float damage, float knockBackForce, Transform attacker)
    {
        StartCoroutine(Hit(damage, knockBackForce, attacker));

    }

    public void SwitchToFocusCam()
    {
        if(focusCamera != null)
        {
            focusCamera.Priority = 20;
        }

    }

    public void SwitchToMainCam()
    {
        if (focusCamera != null)
        {
            focusCamera.Priority = 0;
        }
    }

    public bool CanEnterFocus()
    {
        return focus >= focusMin;
    }
}
