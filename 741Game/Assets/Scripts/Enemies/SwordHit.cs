using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    private enum SwordType
    {
        Player,
        Enemy
    }

    [SerializeField] private float Damage;
    [SerializeField] private float knockbackForce;
    [SerializeField] private Transform Holder;
    [SerializeField] private SwordType swordType;
    [SerializeField] private AudioSource hitSound;


    public void SetSword(float damage, float knockback, Transform holder)
    {
        Damage = damage;
        knockbackForce = knockback;
        Holder = holder;
        hitSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (swordType == SwordType.Player)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (other.gameObject.GetComponent<EnemyBase>() != null)
                {
                    other.gameObject.GetComponent<EnemyBase>().OnPlayerHit(Damage, knockbackForce, Holder);
                    if(hitSound != null)
                    {
                        hitSound.Play();
                    }
                }
            }
        }
        else if (swordType == SwordType.Enemy)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerController>().OnEnemyHit(Damage, knockbackForce, Holder);
                if(hitSound != null)
                {
                    hitSound.Play();
                }
            }
        }

    }
}
