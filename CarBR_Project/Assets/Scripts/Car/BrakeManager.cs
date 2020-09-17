using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeManager : MonoBehaviour
{
    private List<Wheel> wheels;
    [SerializeField] private List<Wheel> motorBrakeWheel;
    private CarEngine engine;


    private float MOTOR_BRAKE;
    private float BRAKE ;

    [SerializeField] private float motorBrake;
    [SerializeField] private float brake;
    [SerializeField] private Vector2 MovementInputs;

    private void Update()
    {
        MotorBrake();
        Brake();
    }

    // Start is called before the first frame update
    void Start()
    {
        engine = transform.GetComponent<CarEngine>();

        wheels = engine.GetWheels();
        MOTOR_BRAKE = engine.GetMotorBrake();
        BRAKE = engine.GetBrake();

        foreach(Wheel wheel in wheels)
        {
            if (wheel.side == "back")
            {
                motorBrakeWheel.Add(wheel);
            }
        }
    }

    public float GetBrake()
    {
        engine.isForward = engine.IsForward();

        if (engine.isForward == 1 && MovementInputs.y <= -0.2f)
        {
            brake = BRAKE;
        }
        else if (engine.isForward == 2 && MovementInputs.y >= 0.2f)
        {
            brake = BRAKE;
        }
        else brake = 0f;


        return brake;
    }

    public void Brake()
    {
        brake = GetBrake();
        foreach (Wheel wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = brake;
        }
        Debug.Log("BrakeManager, Brake : brake = " + brake);
    }

    public void MotorBrake()
    {
            if (MovementInputs.y == 0)
            {
                motorBrake = MOTOR_BRAKE;
                foreach (Wheel wheel in motorBrakeWheel)
                {
                    wheel.wheelCollider.brakeTorque = motorBrake;
                }

                Debug.Log("BrakeManager, MotorBrake : motorBrake = " + motorBrake);
            }
            else
            {
                motorBrake = 0f;
                Debug.Log("BrakeManager, MotorBrake : motorBrake = " + motorBrake);
            }
    }
        
    
    public void SetInput(Vector2 input)
    {
        MovementInputs = input;
    }
}
