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
    private const float HEALTH = 50f;         //  250 + 2 * this
    private const float SPEED = 100f;       //  2 * this
    private const float POWER = 60f;        //  10 * this
    private const float STEER = 40f;

    private const float BOOST = 62f;
    private const float BOOST_AMOUNT = 50f;
    private const float BOOST_TIME_TO_MAX = 50f;

    private int blinkTime = 5;

    [SerializeField] private bool engineOn = false;
    [SerializeField] private float health;
    [SerializeField] private bool isAlive = true;
    [SerializeField] private GameObject cameraUser;
    [SerializeField] private GameObject UI;
    [SerializeField] float respawnTimer = 0f;


    [SerializeField] private Transform up;
    [SerializeField] private Transform down;
    [SerializeField] private CarEngine engine;
    [SerializeField] private Turbo turbo;

    [SerializeField] private Animator respawnAnimator;

    [SerializeField] private TextMeshProUGUI upsideDownText;
    private bool upsideDown;

    private void Awake()
    {
        //SetEngineOn(false);
        respawnAnimator.enabled = false;
        ResetHeatlh();
    }
    private void Start()
    {
        engine = GetComponent<CarEngine>();
        turbo = GetComponent<Turbo>();
        foreach (TextMeshProUGUI tm in UI.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (tm.gameObject.name == "upsideDown") upsideDownText = tm;
        }
        engineOn = true;
        particles = transform.GetComponentInChildren<ParticleManager>();
        UI.SetActive(true);
        upsideDownText.enabled = false;
        ActiveCamera();
    }
    private void Update()
    {
        if (!isAlive && Time.unscaledTime > respawnTimer) Respawn();
    }
    private void FixedUpdate()
    {
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

    public float GetHealth()
    {
        return health;
    }
    public void ResetHeatlh()
    {
        health = 250f + 3f * HEALTH;
    }
    public void ResetBoost()
    {
        turbo.ResetBoost();
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

    public bool RemoveHealth(float amount)
    {
        if (amount >= health)
        {
            health = 0f;
            Die();
            return false;
        }
        else
        {
            health -= amount;
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
        respawnTimer = Time.unscaledTime + 5.01f;
        SetEngineOn(false);
        respawnAnimator.enabled = true;
        respawnAnimator.Play(0);
    }
    public void Respawn()
    {
        engine.GetRigidbody().velocity = Vector3.zero;
        SetAlive(true);
        ResetHeatlh();
        ResetBoost();
        respawnAnimator.enabled = false;
        FindObjectOfType<Mirror.MainMenu.NetworkSpawnPlayerSystem>().RepawnPlayer(this);
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
        if (upsideDown)
        {
            engine.GetRigidbody().velocity = Vector3.zero;
            transform.Rotate(180 * Vector3.forward);
        }
    }



}
