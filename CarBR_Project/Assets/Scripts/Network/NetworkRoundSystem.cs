using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

namespace Mirror.MainMenu
{
    public class NetworkRoundSystem : NetworkBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private PlayerManager player = null;

        private NetworkManagerMainMenu room;

        private NetworkManagerMainMenu Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManagerMainMenu.singleton as NetworkManagerMainMenu;
            }
        }

        public override void OnStartAuthority()
        {
            player = GetComponent<PlayerManager>();
        }

        public void CountdownEnded()
        {
            animator.enabled = false;
        }

        #region Server

        public override void OnStartServer()
        {
            NetworkManagerMainMenu.OnServerStopped += CleanUpServer;
            NetworkManagerMainMenu.OnServerReadied += CheckToStartRound;
        }

        [ServerCallback]
        private void OnDestroy() => CleanUpServer();

        [Server]
        private void CleanUpServer()
        {
            NetworkManagerMainMenu.OnServerStopped -= CleanUpServer;
            NetworkManagerMainMenu.OnServerReadied -= CheckToStartRound;
        }

        [ServerCallback]
        private void StartRound()
        {
            RpcStartRound();
        }
        private void CheckToStartRound(NetworkConnection conn)
        {
            if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) { return; }
            animator.enabled = true;

            RpcStartCountdown();
        }
        #endregion
        #region Client

        [ClientRpc]
        private void RpcStartCountdown()
        {
            animator.enabled = true;
        }
        [ClientRpc]
        private void RpcStartRound()
        {
            Debug.Log("----------STAAAAART-----------");
            Inputs.InputManager.Remove(ActionMapNames.Player);
        }
        #endregion
    }
}
