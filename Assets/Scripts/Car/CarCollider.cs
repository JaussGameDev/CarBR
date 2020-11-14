using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollider : MonoBehaviour

{
    private Car car;
    private float RESISTANCE;

    private void Awake()
    {
        car = GetComponent<Car>();
    }
    private void Start()
    {

        RESISTANCE = car.GetResistance();
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.GetContact(0).thisCollider.name != "head" || col.gameObject.tag == "Ground")
        {
            var amount = 10f * col.relativeVelocity.magnitude;
            amount = Mathf.Clamp(amount, 50f, 1000f);
            amount -= 50f;
            amount = amount * 200f / (RESISTANCE + 200f);
            if (amount > 0)
            {

                var kill = car.RemoveHealth(amount);
                if (!kill)
                {
                    Debug.Log("You Died");
                }
            }
        }
    }
}
