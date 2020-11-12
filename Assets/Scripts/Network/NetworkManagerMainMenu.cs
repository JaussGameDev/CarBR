using System;
using System.Linq;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;

namespace Mirror.MainMenu
{
    public class NetworkManagerMainMenu : NetworkManager
    {
        [SerializeField] private int minPlayer = 2;
        [Scene] [SerializeField] private string menuScene = string.Empty;
        [SerializeField] private List<string> names = new List<string>();

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

        [Header("Game")]
        [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
        [SerializeField] private GameObject playerSpawnSystem = null;
        [SerializeField] private GameObject roundSystem = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;

        public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
        public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

        public override void OnStartServer() { spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList(); Debug.unityLogger.logEnabled = true; }

        public override void OnStartClient()
        {
            //To stop logs :
            Debug.unityLogger.logEnabled = true;

            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                Debug.Log("Loading spawanable prefabs :" + prefab);
                ClientScene.RegisterPrefab(prefab);
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            Debug.Log("OnClientConnect");
            base.OnClientConnect(conn);
            OnClientConnected?.Invoke();
        }
        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }
            if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" != menuScene)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene)
            {
                bool isLeader = RoomPlayers.Count == 0;

                NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.IsLeader = isLeader;

                if (isLeader) roomPlayerInstance.SetIPAddressUI(GetLocalIPv4());
                if (!isLeader) roomPlayerInstance.SetIPAddressUI(networkAddress);

                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

                RoomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            OnServerStopped?.Invoke();

            RoomPlayers.Clear();
            GamePlayers.Clear();
        }
        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        public bool IsReadyToStart()
        {
            if (numPlayers < minPlayer) { return false; }

            foreach (var player in RoomPlayers)
            {
                if (!player.IsReady) { return false; }
            }

            return true;
        }
        public List<string> GetNames()
        {
            return names;
        }

        public void StartGame()
        {
            Debug.Log("01/03 Start Game : Start");
            if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene)
            {

                Debug.Log("02/03 Start Game : If In");
                if (!IsReadyToStart()) { return; }

                ServerChangeScene("TrackTest");
                Debug.Log("03/03 Start Game : If Out End");
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {

            Debug.Log("01/04 ServerChangeScene : Not Pass");
            var nameList = new List<string>();
            //From menu to game
            if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene && newSceneName.StartsWith("Track"))
            {

                Debug.Log("02/04 ServerChangeScene : If Pass");
                for (int i = 0; i < RoomPlayers.Count; i++)
                { 
                    var conn = RoomPlayers[i].connectionToClient;
                    var gameplayerInstance = Instantiate(gamePlayerPrefab);
                    gameplayerInstance.SetDisplayName(RoomPlayers[i].displayName);
                    nameList.Add(RoomPlayers[i].displayName);

                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject, true);

                    Debug.Log("08/04 ServerChangeScene : For Loop In 06");
                }
            }
            base.ServerChangeScene(newSceneName);
            names = nameList;
            if (newSceneName.StartsWith("Track"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);
                playerSpawnSystemInstance.GetComponent<NetworkSpawnPlayerSystem>().SetNames(names);

                Debug.Log("03/04 OnServerChangeScene : End If In | " + newSceneName);
            };
        }

        public override void OnServerSceneChanged(string newSceneName)
        {

            Debug.Log("01/03 OnServerChangeScene : Not Pass If | " + newSceneName);
            if (newSceneName.StartsWith("Track"))
            {
                Debug.Log("02/03 OnServerChangeScene : Start If In | " + newSceneName);
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);

                GameObject roundSystemInstance = Instantiate(roundSystem);
                NetworkServer.Spawn(roundSystemInstance);
                Debug.Log("03/03 OnServerChangeScene : End If In | " + newSceneName);
            };
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);

            OnServerReadied?.Invoke(conn);
        }

        public string GetLocalIPv4()
        {
            Debug.Log("Get Local IPv4 : " + Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(
                    f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .ToString());

            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(
                    f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .ToString();
        }


    }
}
