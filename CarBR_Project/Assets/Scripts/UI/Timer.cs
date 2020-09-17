using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] private bool counting = false;
    [SerializeField] private bool playing = false;
    [SerializeField] private TMPro.TextMeshProUGUI timer;
    [SerializeField] private float launchTimer = 10f;
    [SerializeField] private int UItimer = 10;
    // Start is called before the first frame update
    void Awake()
    {
        timer = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!counting) timer.gameObject.SetActive(false);
        if (counting)
        {
            //Debug.Log("countin : " + counting);
            timer.gameObject.SetActive(true);
            StartTimer();
        }

    }
    public void StartTimer()
    {
        if (launchTimer > -1)
        {
            UItimer = Mathf.RoundToInt(launchTimer + 0.5f);
            timer.text = UItimer.ToString();
            launchTimer -= Time.deltaTime;
            counting = true;
            timer.gameObject.SetActive(true);
            playing = false;
        }
        else
        {
            timer.gameObject.SetActive(false);
            counting = false;
            playing = true;
            launchTimer = 3f;
        }
    }
    public void SetCounting(bool value)
    {
        if (value)
        {
            timer.gameObject.SetActive(true);
            playing = false;
        }
        //Debug.Log("setCountin : " + value);
        counting = value;
    }
    public bool GetCounting()
    {
        return counting;
    }
    public void SetPlaying(bool value)
    {
        //Debug.Log("setPlaying : " + value);
        playing = value;
    }
    public bool GetPlaying()
    {
        return playing;
    }
}
