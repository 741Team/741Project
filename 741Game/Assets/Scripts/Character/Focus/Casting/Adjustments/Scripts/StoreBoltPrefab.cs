using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBoltPrefab : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject forward;
    [SerializeField] GameObject right;

    public GameObject GetPrefab()
    {
        return prefab;
    }
    public GameObject GetForward() {
        return forward;
    }
    public GameObject GetRight()
    {
        return right;
    }
}
