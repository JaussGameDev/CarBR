using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MainMenu
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        void Awake() => NetworkSpawnPlayerSystem.AddSpawnPoint(transform);

        void OnDestroy() => NetworkSpawnPlayerSystem.RemoveSpawnPoint(transform);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);

        }
    }
}