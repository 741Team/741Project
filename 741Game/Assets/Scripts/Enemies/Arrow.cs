using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float maxLifetime;
    [SerializeField] private float damage;
    [SerializeField] private float knockback;

    public void Project(Vector3 direction)
    {
        ///Set force in direction
        rb.AddForce((direction) * speed);
        Destroy(this.gameObject, maxLifetime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().OnHitByEnemy(damage, knockback, transform);
            Destroy(this.gameObject, 0.1f);
        }
    }
}
