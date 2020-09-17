using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mirror
{
    public class ChatBehavior : NetworkBehaviour
    {
        [SerializeField] private GameObject UIObject = null;
        [SerializeField] private TMP_Text chatText = null;
        [SerializeField] private String displayName = null;
        [SerializeField] private TMP_InputField inputField = null;
        // Start is called before the first frame update

        private static event Action<string> OnMessage;
        public override void OnStartAuthority()
        {
            enabled = true;

            if (UIObject != null) UIObject.SetActive(true);

            OnMessage += HandleNewMessage;
        }


        [ClientCallback]
        private void OnDestroy()
        {
            if (!hasAuthority) { return; }
            OnMessage -= HandleNewMessage;
            
        }

        private void HandleNewMessage(string message)
        {
            chatText.text += message;
        }


        [Client]
        public void SetDisplayName(String name)
        {
            displayName = name;
        }

        [Client]
        public void Send(string message)
        {
            if (!hasAuthority) { return; }
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }
            if (!Input.GetKeyDown(KeyCode.Return)) {
                return; 
            }

                CmdSendMessage(inputField.text);

            inputField.text = string.Empty;
        }

        [Command]
        private void CmdSendMessage(String message)
        {
            if (string.IsNullOrWhiteSpace(displayName)) { RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}"); return; }
            RpcHandleMessage($"[{displayName}]: {message}");
        }



        [ClientRpc]
        private void RpcHandleMessage(string message)
        {
            OnMessage?.Invoke($"\n{message}");
        }

        }

}
