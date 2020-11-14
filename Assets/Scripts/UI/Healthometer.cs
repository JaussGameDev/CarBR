using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthometer : MonoBehaviour
{
    public Slider slider;
    private Car car;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        car = GetComponentInParent<Car>();
        setMaxHealth(car.GetHealth());
    }

    private void FixedUpdate()
    {
        setHealth();    
    }

    private float GetHealth()
    {
        return car.GetHealth();
    }

    public void setMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth()
    {
        slider.value = GetHealth();
    }
}
