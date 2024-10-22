using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrail : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;

    private void Start()
    {
        trail.emitting = false;
    }

    public void StartTrail()
    {
        trail.emitting = true;
    }

    public void StopTrail()
    {
        trail.emitting = false;
    }
}
