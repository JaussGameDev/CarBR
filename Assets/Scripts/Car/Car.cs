using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Car : MonoBehaviour
{
    private ParticleManager particles;


    private string displayName = "Loading...";

    private const float STRENGH = 60f;
    private const float RESISTANCE = 60f;
    private const float LIFE = 50f;         //  250 + 2 * this
    private const float SPEED = 100f;       //  2 * this
    private const float POWER = 60f;        //  10 * this
    private const float STEER = 40f;

    private const float BOOST = 62f;
    private const float BOOST_AMOUNT = 50f;
    private const float BOOST_TIME_TO_MAX = 50f;

    private int blinkTime = 5;

    [SerializeField] private bool engineOn = false;
    [SerializeField] private float life;
    [SerializeField] private bool isAlive = true;
    [SerializeField] private GameObject cameraUser;
    [SerializeField] private GameObject UI;


    [SerializeField] private Transform up;
    [SerializeField] private Transform down;

    [SerializeField] private TextMeshProUGUI upsideDownText;
    private bool upsideDown;

    private void Awake()
    {
        life = 250 + 3 * LIFE;
    }
    private void Start()
    {
        foreach (TextMeshProUGUI tm in UI.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (tm.gameObject.name == "upsideDown") upsideDownText = tm;
        }
        engineOn = true;
        particles = transform.GetComponentInChildren<ParticleManager>();
        upsideDownText.enabled = false;
    }

    private void FixedUpdate()
    {
        //Debug.Log("not pass yet");
        //Debug.Log("pass");
        UI.SetActive(true);
        ActiveCamera();
        CheckReverse();
    }


    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    public void textBlink()
    {
        if (blinkTime == 0)
        {
            upsideDownText.enabled = !upsideDownText.enabled;
            blinkTime = 5;
        }
        else
        {
            blinkTime--;
        }

    }

    public float GetLife()
    {
        return life;
    }
    public float GetStrengh()
    {
        return STRENGH;
    }
    public float GetResistance()
    {
        return RESISTANCE;
    }
    public float GetSpeed()
    {
        return SPEED;
    }
    public float GetPower()
    {
        return POWER;
    }
    public float GetBoost()
    {
        return BOOST;
    }
    public float GetBoostAmount()
    {
        return BOOST_AMOUNT;
    }
    public float GetBoostTimeToMax()
    {
        return BOOST_TIME_TO_MAX;
    }
    public float GetSteer()
    {
        return STEER;
    }
    public bool GetAlive()
    {
        return isAlive;
    }

    public bool removeLife(float amount)
    {
        if (amount >= life)
        {
            life = 0f;
            Die();
            return false;
        }
        else
        {
            life -= amount;
            return true;
        }
    }
    public void Die()
    {
        SetAlive(false);
        foreach (ParticleSystem explosion in particles.GetExplosions())
        {
            explosion.Play();
        }
    }
    public void SetAlive(bool b)
    {
        isAlive = b;
    }
    public void SetUpDown()
    {
        up = transform.Find("up");
        down = transform.Find("down");
    }

    public void SetEngineOn(bool value)
    {
        engineOn = value;
    }
    public bool getEngineEnable()
    {
        return engineOn;
    }
    public bool GetLocal()
    {
        return true;
    }

    public void CheckReverse()
    {
        if (up.position.y - down.position.y < -0.9f)
        {
            Debug.Log(blinkTime);
            textBlink();
            upsideDown = true;

        }
        else
        {
            upsideDownText.enabled = false;
            upsideDown = false;
        }

    }
    public void ActiveCamera()
    {
        cameraUser.SetActive(true);
    }

    public void Flip()
    {
        if (upsideDown) transform.Rotate(180 * Vector3.forward);
    }



}
