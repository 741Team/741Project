using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float value;

    [SerializeField] private bool refillable;
    [SerializeField] private float refillRate;

    public void Setup(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }
    
    public void Increase(float plus)
    {
        slider.value = slider.value + plus;
    }

    public void Decrease(float sub)
    {
        slider.value = slider.value - sub;
    }

    public void SetValue(float val)
    {
        slider.value = val;
    }

    private void FixedUpdate()
    {
        if (refillable)
        {
            slider.value = slider.value + refillRate;
        }
    }
}
