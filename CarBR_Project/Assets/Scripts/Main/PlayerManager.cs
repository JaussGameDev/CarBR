using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Mirror
{
    public class PlayerManager : NetworkBehaviour
    {
        [Header("Components")]
        [SerializeField] Car car = null;
        [SerializeField] CarEngine engine = null;
        [SerializeField] BrakeManager brakes = null;
        [SerializeField] Turbo turbo = null;
        [SerializeField] CarCollider colliders = null;
        [SerializeField] ChatBehavior chat = null;

        [Header("Player")]
        [SerializeField] string displayName = null;

        // Start is called before the first frame update
        public override void OnStartAuthority()
        {
            car = GetComponentInChildren<Car>();
            engine = GetComponentInChildren<CarEngine>();
            brakes = GetComponentInChildren<BrakeManager>();
            turbo = GetComponentInChildren<Turbo>();
            colliders = GetComponentInChildren<CarCollider>();
            chat = GetComponent<ChatBehavior>();

            chat.enabled = true;
            car.enabled = true;
            engine.enabled = true;
            brakes.enabled = true;
            turbo.enabled = true;
            colliders.enabled = true;

            car.SetEngineOn(true);

        }

        [ClientCallback]
        public void setDisplayName(string name)
        {
            displayName = name;
            chat.SetDisplayName(name);
        }

    }
}
