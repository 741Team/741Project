using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{

    Animator _anim;
    [SerializeField]
    float _attackTime = 1;
    float _defaultAttackTime;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _defaultAttackTime = _attackTime;

    }

    public void SetAttackTime(float time)
    {
        _defaultAttackTime = time;
    }

    public void SetRunDirection(Vector3 direction)
    {
        _anim.SetFloat("Right", direction.x);
        _anim.SetFloat("Forward", direction.z);
    }

    private void Update()
    {
        if (_attackTime > 0)
        {
            _attackTime -= Time.deltaTime;
            _anim.SetFloat("AttackTime", _attackTime);
        }
    }

    public void SetSpeed(float speed)
    {
        _anim.SetFloat("Movement", speed);
    }

    public void Attack()
    {
        _anim.SetTrigger("Attack");
        _attackTime = _defaultAttackTime;
    }

    public void EnterFocus()
    {
        _anim.SetFloat("Movement", 0);
        _anim.SetBool("Focus", true);
    }

    public void ExitFocus()
    {
        _anim.SetBool("Focus", false);
    }

    public void Cast()
    {
        _anim.SetTrigger("Cast");
        _attackTime = _defaultAttackTime;
    }
}
