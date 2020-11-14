using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo : MonoBehaviour
{
    private Car car;
    private CarEngine engine;
    private Rigidbody _rigidbody;
    private CarColliders colliders;
    private ParticleManager particles;

    private float POWER;
    private float AMOUNT;
    private float realAmount;
    private float increaseSpeed = 0.1f;
    private Vector3 direction;
    [SerializeField] private bool isBoosting = false;

    private float TIME_TO_MAX = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<Car>();
        engine = transform.GetComponent<CarEngine>();

        _rigidbody = engine.GetComponentInChildren<Rigidbody>();
        colliders = transform.GetComponentInChildren<CarColliders>();
        particles = transform.GetComponentInChildren<ParticleManager>();
        POWER = (1f/2f) * car.GetBoost();
        AMOUNT = 200f + 3f * car.GetBoostAmount();
        realAmount = AMOUNT;
        TIME_TO_MAX = (1f / 100f) * car.GetBoostTimeToMax();
        


        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (realAmount < AMOUNT) realAmount += 0.1f;
        Boost();
    }

    public float GetBoost()
    {
        return realAmount;
    }
    public void ResetBoost()
    {
        realAmount = AMOUNT;
    }

    private bool Boost()
    {
        if (realAmount > 0 && car.getEngineEnable() && isBoosting)
        {
            direction = colliders.GetHeadCollider().position - colliders.GetTailCollider().position;
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, direction.normalized * POWER, TIME_TO_MAX * Time.deltaTime);
            realAmount -= 1;
            return true;
        }
        else
        {
            return false;
        }
            
            
    }

    public void StartBoost()
    {
        Debug.Log("Turbo : StartBoost");
        isBoosting = true;
        foreach (ParticleSystem boost in particles.GetBoosts())
        {
            boost.Play();
        }
    }
    public void StopBoost()
    {
        Debug.Log("Turbo : StopBoost");
        isBoosting = false;
        foreach (ParticleSystem boost in particles.GetBoosts())
        {
            boost.Stop();
        }
    }

}
