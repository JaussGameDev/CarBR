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
        AMOUNT = 200f + 3 * car.GetBoostAmount();
        realAmount = AMOUNT;
        TIME_TO_MAX = (1f / 100f) * car.GetBoostTimeToMax();
        


        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (realAmount < AMOUNT) realAmount += 0.05f;
        Boost();
        direction = colliders.GetHeadCollider().position - colliders.GetTailCollider().position;
    }

    public float GetBoost()
    {
        return realAmount;
    }

    private bool Boost()
    {
        if (realAmount > 0 && car.getEngineEnable() && isBoosting)
        {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, direction.normalized * POWER, TIME_TO_MAX * Time.deltaTime);

            foreach (ParticleSystem boost in particles.GetBoosts())
            {
                boost.Play();
            }
            realAmount -= 1;
            return true;
        }
        else
        {
            foreach (ParticleSystem boost in particles.GetBoosts())
            {
                boost.Stop();
            }
            return false;
        }
            
            
    }

    public void StartBoost()
    {
        Debug.Log("Turbo : StartBoost");
        isBoosting = true;
    }
    public void StopBoost()
    {
        Debug.Log("Turbo : StopBoost");
        isBoosting = false;
    }

}
