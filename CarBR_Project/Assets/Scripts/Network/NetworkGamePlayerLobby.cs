using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

namespace Mirror.MainMenu
{

    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        [SerializeField] private string displayName = "Loading...";

        private NetworkManagerMainMenu room;

        private NetworkManagerMainMenu Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerMainMenu;
            }
        }


        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.GamePlayers.Add(this);
        }

        //public override void OnStopClient()
        public override void OnNetworkDestroy()
        {
            Room.GamePlayers.Remove(this);
        }

        [Server]
        public void SetDisplayName(string name)
        {
            displayName = name;
        }

    }
}