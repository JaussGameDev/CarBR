using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerMainMenu networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;

        public void HostLobby()
        {
            networkManager.networkAddress = "90.89.196.190";
            Debug.Log("networkManager.networkAddress 1:" + networkManager.networkAddress);
            networkManager.StartHost();
            Debug.Log("networkManager.networkAddress 2:" + networkManager.networkAddress);

            landingPagePanel.SetActive(false);
        }
    }
}
