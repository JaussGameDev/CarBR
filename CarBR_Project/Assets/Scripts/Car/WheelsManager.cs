using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsManager : MonoBehaviour
{
    public List<Wheel> wheels;
    public List<Wheel> throttleWheels;
    public List<Wheel> steerWheels;



    // Update is called once per frame
    void Awake()
{
    wheels.AddRange(transform.GetComponentsInChildren<Wheel>());

        foreach(Wheel wheel in wheels)
        {
            if (wheel.throttle) throttleWheels.Add(wheel);
            if (wheel.steer) steerWheels.Add(wheel);
        }
        
    }
}
