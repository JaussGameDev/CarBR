using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
    [SerializeField] private bool enoughPlayer = false;
    [SerializeField] private bool inGame = false;
    [SerializeField] private bool playing = false;
    [SerializeField] private Mirror.MatchManager match = null;
    [SerializeField] private List<Car> cars;
    [SerializeField] private List<Car> alive;
    [SerializeField] private List<GameObject> players;
    [SerializeField] private Timer timer;
    // Start is called before the first frame update

    public bool GetInGame()
    {
        return inGame;
    }
    public bool GetPlaying()
    {
        return playing;
    }
    void Start()
    {
        match = GetComponent<Mirror.MatchManager>();
        Canvas c = GetComponentInChildren<Canvas>();
        Debug.Log("       ---------CANVAS : " + c.name);
        timer = c.GetComponentInChildren<Timer>();

    }
    // Update is called once per frame
    void Update()
    {
        enoughPlayer = match.GetEnoughtPlayer();

        if (enoughPlayer)
        {
            Debug.Log("-------------enoughPlayer ok");
            playing = timer.GetPlaying();
            if (playing)
            {
                match.RpcHandleTimer(true);
            }
            if (inGame)
            {
                Debug.Log("-------------inGame ok");
                int i = 0;
                foreach (Car car in cars)
                {
                    alive.Remove(car);
                    if (car.GetAlive())
                    {
                        i++;
                        Debug.Log("         ----------     i =" + i);
                        alive.Add(car);
                    }
                }
                if (i <= 0)
                {
                    foreach (Car winner in alive)
                    {
                        Debug.Log("The winner is : " + winner);
                    }
                    match.RpcHandleTimer(false);
                    inGame = false;
                }
            }
            else
            {
                cars = match.GetCars();
                StartTimer();
                foreach (Car car in cars)
                {
                    match.RpcHandleTimer(false);
                }
                inGame = true;
            }
        }
    }

    public void StartTimer()
    {
        Debug.Log("----------------StartTimer");
        timer.StartTimer();
    }
}
