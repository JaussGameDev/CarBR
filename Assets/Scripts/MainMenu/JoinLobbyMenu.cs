using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace Mirror.MainMenu
{
    public class JoinLobbyMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerMainMenu networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;
        [SerializeField] private TMP_InputField ipAddressInputField = null;
        [SerializeField] private Button joinButton = null;

        private void OnEnable()
        {
            NetworkManagerMainMenu.OnClientConnected += HandleClientConnected;
            NetworkManagerMainMenu.OnClientDisconnected += HandleClientDisconnected;
        }
        private void OnDisable()
        {
            NetworkManagerMainMenu.OnClientConnected -= HandleClientConnected;
            NetworkManagerMainMenu.OnClientDisconnected -= HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            string ipAdress = ipAddressInputField.text;

            networkManager.networkAddress = ipAdress;
            Debug.Log("JoinLobbyMenu, IP adress : " + ipAdress);
            networkManager.StartClient();

            joinButton.interactable = false;
        }

        private void HandleClientConnected()
        {
            joinButton.interactable = true;
            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }

    }
}
