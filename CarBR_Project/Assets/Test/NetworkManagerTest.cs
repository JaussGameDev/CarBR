using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mirror
{
    public class NetworkManagerTest : NetworkManager
    {

        [SerializeField] private GameObject playerSpawnSystem = null;
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


        public void StartGame()
        {
                Debug.Log("01/02 OnServerChangeScene : Start If In");
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);
                Debug.Log("02/02 OnServerChangeScene : End If In");
        }
    }

}
