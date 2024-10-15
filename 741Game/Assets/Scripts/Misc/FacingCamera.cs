using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    private Camera Camera;

    private void Awake()
    {
        Camera = ItemManager.singleton.mainCamera;
    }

    private void Update()
    {
        transform.LookAt(Camera.transform, Vector3.up);
    }
}
