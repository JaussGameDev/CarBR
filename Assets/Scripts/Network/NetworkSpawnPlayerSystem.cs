﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;

namespace Mirror.MainMenu
{
    //Trouver pourquoi ça spawn pas :
    public class NetworkSpawnPlayerSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private List<string> names = new List<string>(); 

        private static List<Transform> spawnPoints = new List<Transform>();

        private int nextIndex = 0;

        public static void AddSpawnPoint(Transform transform)
        {
            spawnPoints.Add(transform);
            spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }

        public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

        public override void OnStartServer() => NetworkManagerMainMenu.OnServerReadied += SpawnPlayer;

        public override void OnStartClient()
        {
            Inputs.InputManager.Add(ActionMapNames.Player);
            Inputs.InputManager.Controls.Player.Look.Enable();
        }
        public void SetNames(List<string> nameList)
        {
            names = nameList;
        }

        [ServerCallback]

        private void OnDestroy() => NetworkManagerMainMenu.OnServerReadied -= SpawnPlayer;

        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            Debug.Log("SpawnPlayer : Start");
            Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);
            names = FindObjectOfType<NetworkManagerMainMenu>().GetNames();

            if(spawnPoint == null)
            {
                Debug.LogError($"Missing spawn point for player {nextIndex}");
                return;
            }
            GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
            NetworkServer.Spawn(playerInstance, conn);
            Debug.Log("names.Count = " + names.Count);
            playerInstance.GetComponent<ChatBehavior>().SetDisplayName(names[nextIndex]);

            nextIndex++;
        }

    }
}
