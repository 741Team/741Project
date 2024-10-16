using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation: MonoBehaviour
{
    Vector3 _startRotation;
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = _startRotation;
    }

    public void SetStartRotation()
    {
        _startRotation = transform.eulerAngles;
    }
}
