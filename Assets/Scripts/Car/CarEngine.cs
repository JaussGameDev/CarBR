using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarEngine : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private WheelsManager wheels;
    [SerializeField] private CarColliders colliders;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector2 MovementInputs;

    private float MOTOR_TORQUE;
    [SerializeField] private float currMotorTorque;

    private float MAX_STEER;

    private float maxSpeed;
    private float speed;

    public const float MOTOR_BRAKE = 200f;
    public const float BRAKE = 2500f;

    public float BOOST_POWER;

    public int isForward = 0;

    public Transform centerOfMass;

    private void Awake()
    {
        //Debug.Log("Client");
        car = GetComponent<Car>();

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
        colliders = transform.GetComponentInChildren<CarColliders>();
        wheels = GetComponentInChildren<WheelsManager>();

        maxSpeed = 2 * car.GetSpeed();
        MOTOR_TORQUE = 3350f + 1.5f * car.GetPower();
        MAX_STEER = car.GetSteer();
    }
    void FixedUpdate()
    {
        SetSpeed();
        SetTorque();

        if (car.getEngineEnable())
        {
            foreach (Wheel wheel in wheels.throttleWheels)
            {
                Debug.Log("Engine, FixedUpdate : MovementInputs.y :" + MovementInputs.y);
                Debug.Log("Engine, FixedUpdate : currMotoTorque.y :" + currMotorTorque);
                wheel.wheelCollider.motorTorque = MovementInputs.y * currMotorTorque;
            }
        }
        foreach (Wheel wheel in wheels.steerWheels)
        {
            wheel.wheelCollider.steerAngle = MovementInputs.x * MAX_STEER;
        }
    }

    private void Update()
    {
        var pos = Vector3.zero;
        var rot = Quaternion.identity;

        foreach (Wheel wheel in wheels.wheels)
        {
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.transform.position = pos;
            wheel.transform.rotation = rot;
        }
        
    }

    public List<Wheel> GetWheels()
    {
        return wheels.wheels;
    }
    public void SetSpeed()
    {
        // vitesse * 8 c'est pas full style
        speed = 5 * _rigidbody.velocity.magnitude;
    }
    public float GetSpeed()
    {
        return speed;
    }
    private void SetTorque()
    {
        if (speed >= maxSpeed)
        {
            currMotorTorque = 1f;
        }
        else
        {
            var fakeSpeed = (3.5f * (speed + 1f));
            currMotorTorque = (maxSpeed / Mathf.Clamp(fakeSpeed, 90f, 500f * maxSpeed)) * MOTOR_TORQUE;
        }
        if (MovementInputs.y == 0) currMotorTorque = 0f;

        Debug.Log("CarEngine : SetTorque = currMotorTorque =" + currMotorTorque);
    }
    public float GetTorque()
    {
        return currMotorTorque;
    }
    public int IsForward()
    {
        var Head = colliders.GetHeadCollider();
        var Tail = colliders.GetTailCollider();
        var x = Mathf.Sign((Head.position.x - Tail.position.x));
        var z = Mathf.Sign((Head.position.z - Tail.position.z));

        if (Mathf.Sign(_rigidbody.velocity.x) == x && Mathf.Sign(_rigidbody.velocity.z) == z)
        {
            // FORWARD
            return 1;
        }
        else if (Mathf.Abs(_rigidbody.velocity.x) <= 0.1f && Mathf.Abs(_rigidbody.velocity.z) <= 0.1f)
        {
            // NOT MOVING
            return 0;
        }
        else
        {
            // BACKWARD
            return 2;
        }

    }
    public float GetMotorBrake()
    {
        return MOTOR_BRAKE;
    }
    public float GetBrake()
    {
        return BRAKE;
    }
    public void SetMaxSpeed(float speed)
    {
        maxSpeed = speed;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }
    public void SetInput(Vector2 input)
    {
        MovementInputs = input;
    }
}
