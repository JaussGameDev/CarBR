using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class MatchManager : NetworkBehaviour
    {
        [SerializeField] private bool enoughPlayer = false;

        //[SerializeField] private bool inGame = false;
        //[SerializeField] private bool playing = false;
        [SerializeField] private List<Car> cars;
        //[SerializeField] private List<Car> alive;
        [SerializeField] private List<GameObject> players;
        //[SerializeField] private Timer timer;


        public bool GetEnoughtPlayer()
        {
            return enoughPlayer;
        }
        public void Connection(GameObject player)
        {
            players.Add(player);
            cars.Add(player.gameObject.GetComponent<Car>());
            if (cars.Count >= 1)
            {
                enoughPlayer = true;
            }
        }
        public void Quit()
        {
            int count = cars.Count;
            int i = 0;
            while (i < count -1)
            {
                players.RemoveAt(i);
                cars.RemoveAt(i);
                i++;
            }
        }
        public List<Car> GetCars()
        {
            return cars;
        }
        [ClientRpc]
        public void RpcHandleTimer(bool value)
        {
            foreach(Car car in cars) car.SetEngineOn(value); 
        }

    }
}
