using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float value;
    private float MaxValue;

    [SerializeField] private bool refillable;
    [SerializeField] private float refillRate;

    [SerializeField] private bool FocusBar;
    [SerializeField] private float focusRefillRate;

    private bool canEnterFocus;

    public void Setup(float maxValue)
    {
        MaxValue = maxValue;
        slider.maxValue = maxValue;
        slider.value = maxValue;
        if (FocusBar)
        {
            Reset();
        }
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

    private void Update()
    {
        if (FocusBar & slider.value == MaxValue & canEnterFocus)
        {
            Debug.Log("Enter Focus Mode");
            canEnterFocus = false;
            Reset();
        }
    }

    private void Reset()
    {
        slider.value = 0f;
        canEnterFocus = true;
    }

    private void FixedUpdate()
    {
        if (refillable)
        {
            slider.value = slider.value + refillRate;
        }

        if (FocusBar & canEnterFocus)
        {
            slider.value = slider.value + focusRefillRate;
        }
    }
}
