using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boostometer : MonoBehaviour
{
    public Slider slider;
    private Car car;
    private Turbo turbo;
    private float amount;


    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        car = GetComponentInParent<Car>();
        turbo = GetComponentInParent<Turbo>();
        setMaxBoost( car.GetBoostAmount() );
    }

    private void FixedUpdate()
    {
        setBoost();
    }

    public float GetBoostAmount()
    {
        return turbo.GetBoost();
    }
    public void setMaxBoost(float boost)
    {
        slider.maxValue = boost * 3 + 200;
    }

    public void setBoost()
    {
        slider.value = GetBoostAmount();
    }
}
