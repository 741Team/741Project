using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;


    [SerializeField] private float dashTime;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashCooldown;
    private bool canDash;


    private Canvas canvas;
    Vector3 movement;
    Vector3 IsoMovement;

    [SerializeField] Vector3 look;

    [SerializeField] private bool AllowedToMove;

    [SerializeField] private TrailRenderer trail;

    [SerializeField] private Bar dashBar;
    [SerializeField] GameObject DashBar;

    public Transform feet;
    private bool allowedToJump;
    public LayerMask groundLayers;

    [SerializeField] private float maxDamage;
    public float damage;

    [SerializeField] private GameObject sword;

    void Start()
    {
        AllowedToMove = true;

        canvas = ItemManager.singleton.enemyCanvas;

        dashBar.transform.SetParent(canvas.transform);
        canDash = true;
        dashBar.Setup(100);
        DashBar.SetActive(false);

        allowedToJump = true;

        damage = 0;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal")*-1f;
        movement.z = Input.GetAxisRaw("Vertical")*-2f;
        
        if (Input.GetButtonDown("Jump") & allowedToJump & AllowedToMove)
        {
            Jump();
            allowedToJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) & canDash & AllowedToMove)
        {
            StartCoroutine(Dodge());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) & AllowedToMove)
        {
            StartCoroutine(Attack());
        }


        if (movement.x == 1 & movement.z == 2)
        {
            look = new Vector3(0,0,0);
        }
        else if (movement.x == 1 & movement.z == 0)
        {
            look = new Vector3(0,45,0);
        }
        else if (movement.x == 1 & movement.z == -2)
        {
            look = new Vector3(0,90,0);
        }
        else if (movement.x == 0 & movement.z == -2)
        {
            look = new Vector3(0,135,0);
        }
        else if (movement.x == -1 & movement.z == -2)
        {
            look = new Vector3(0,180,0);
        }
        else if (movement.x == -1 & movement.z == 0)
        {
            look = new Vector3(0,225,0);
        }
        else if (movement.x == -1 & movement.z == 2)
        {
            look = new Vector3(0,270,0);
        }
        else if (movement.x == 0 & movement.z == 2)
        {
            look = new Vector3(0,315,0);
        }
    }

    void FixedUpdate()
    {
        if (AllowedToMove == true)
        {
            transform.rotation = Quaternion.Euler(look);
            IsoMovement = IsomentricConverter(movement);
            rb.MovePosition(transform.position + IsoMovement * speed * Time.fixedDeltaTime);
        }
    }
    
    private Vector3 IsomentricConverter(Vector3 vector)
    {
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
            allowedToJump = true;
        }
    }

    private IEnumerator Attack()
    {
        damage = maxDamage;
        var swordRenderer = sword.GetComponent<Renderer>();
        Color red = new Color(1f, 0f, 0f, 1f);
        swordRenderer.material.SetColor("_Color", red);
        yield return new WaitForSeconds(1f);
        Color white = new Color(1f, 1f, 1f, 1f);
        swordRenderer.material.SetColor("_Color", white);
        damage = 0;
    }

}
