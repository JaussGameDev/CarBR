using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

namespace Mirror.MainMenu
{

    public class NetworkRoomPlayerLobby : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject lobbyUI = null;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
        [SerializeField] private ChatBehavior chat = null;
        [SerializeField] private Button startGameButton = null;
        [SerializeField] private TMP_Text IpAddress = null;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string displayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;

        private bool isLeader;
        public bool IsLeader
        {
            set
            {
                isLeader = value;
                startGameButton.gameObject.SetActive(value);
            }
        }
        private NetworkManagerMainMenu room;

        private NetworkManagerMainMenu Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerMainMenu;
            }
        }

        public string[] GetNameText()
        {
            var list = new string[Room.RoomPlayers.Count];
            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                list[i] = playerNameTexts[i].text;
            }
            return list;
        }
        public override void OnStartAuthority()
        {
            Debug.Log("OnStartAuthority : NetworkRoomPlayerLooby");
            chat = GetComponent<ChatBehavior>();
            CmdSetDisplayName(PlayerNameInput.DisplayName);

            lobbyUI.SetActive(true);
        }
        public override void OnStartServer()
        {
            chat = GetComponent<ChatBehavior>();
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);

            UpdateDisplay();
        }

        //public override void OnStopClient()
        public override void OnNetworkDestroy()
        {
            Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public void HandleReadyStatusChanged(bool oldvalue, bool newValue) => UpdateDisplay();

        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

        private void UpdateDisplay()
        {
            //If it's not me
            if(!hasAuthority)
            {
                //Find me
                foreach (var player in Room.RoomPlayers)
                {
                    if (player.hasAuthority)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }
                return;
            }

            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "WaitingForPlayer...";
                playerReadyTexts[i].text = string.Empty;
            }

            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                playerNameTexts[i].text = Room.RoomPlayers[i].displayName;
                playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                    "<color=green>Ready</color>" :
                    "<color=red>Not Ready</color>";
            }
        }

        public void HandleReadyToStart(bool ReadyToStart)
        {
            if (!isLeader) { return; }

            startGameButton.interactable = ReadyToStart;
        }

        [Command]
        private void CmdSetDisplayName(string name)
        {
            Debug.Log("CmdSetDisplayName :" + name);
            displayName = name;
            CltCallSetName(name);
        }

        [ClientCallback]
        private void CltCallSetName(string name)
        {
            Debug.Log("CltCallSetName :" + name);
            chat.SetDisplayName(name);
        }

        [Command]
        public void CmdReadyUp()
        {
            IsReady = !IsReady;

            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {
            if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }
            

            Room.StartGame();
        }
        [Client]
        public void SetIPAddressUI(string address)
        {
            Debug.Log("Client : SetIPAddressUI :" + address);
            IpAddress.text = "IP Address : " + address;
        }
    }
}