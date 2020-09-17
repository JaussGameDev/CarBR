using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

namespace Inputs.Mirror 
{
    public class PlayerCameraManager : NetworkBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Vector2 maxFollowOffset = new Vector2(-12f, 12f);
        [SerializeField] private Vector3 cameraVelocity = new Vector3(100f, 100f, 100f);

        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
        [SerializeField] private Vector2 playerInput = new Vector3(0f, 0f);
        [SerializeField] private Vector3 followOffset = new Vector3(0f, 5f, -14f);
        [SerializeField] private Vector3 usedOffset = new Vector3(0f, 5f, -14f);
        [SerializeField] private Vector2 _trackedObjectOffset = new Vector2(1f, -4f);
        [SerializeField] private Vector2 usedTrackedObjectOffset = new Vector2(1f, -4f);


        private CinemachineTransposer transposer;
        private CinemachineComposer composer;


        public override void OnStartAuthority()
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();

            virtualCamera.gameObject.SetActive(true);

            enabled = true;

            InputManager.Controls.Player.Look.performed += ctx => SetInput(ctx.ReadValue<Vector2>());
            InputManager.Controls.Player.Look.canceled += _ => ResetOffset();
            ResetOffset();
        }

        [ClientCallback]
        private void Update()
        {
            SetOffset();
            Look();
        }

        private void ResetOffset()
        {
            playerInput.x = 0f;
            playerInput.y = 0f;
            transposer.m_FollowOffset = followOffset;
            usedTrackedObjectOffset = _trackedObjectOffset;
        }

        private void SetInput(Vector2 lookAxis)
        {
            playerInput.x = lookAxis.x;
            playerInput.y = lookAxis.y;
        }
        private void SetOffset()
        {
            float deltaTime = Time.deltaTime;

            float followOffsetX = Mathf.Clamp(
                transposer.m_FollowOffset.x - (playerInput.x * cameraVelocity.x * deltaTime),
                maxFollowOffset.x,
                maxFollowOffset.y);

            float followOffsetY = followOffset.y;

            float followOffsetZ = Mathf.Clamp(
                transposer.m_FollowOffset.z + Mathf.Abs(playerInput.x * cameraVelocity.z * deltaTime),
                followOffset.z,
                -2f);

            float usedTrackedOffsetY = usedTrackedObjectOffset.x;
            float usedTrackedOffsetZ = usedTrackedObjectOffset.y;

            if (playerInput.x != 0f)
            {
                float trackedOffsetY = Mathf.Clamp(
                        usedTrackedObjectOffset.x - Mathf.Abs(playerInput.x * cameraVelocity.x * deltaTime),
                        -_trackedObjectOffset.x,
                        _trackedObjectOffset.x);

                float trackedOffsetZ = Mathf.Clamp(
                        usedTrackedObjectOffset.y + Mathf.Abs(playerInput.x * cameraVelocity.x * deltaTime),
                        _trackedObjectOffset.y,
                        -0.5f);

                usedTrackedOffsetY = trackedOffsetY;
                usedTrackedOffsetZ = trackedOffsetZ;
            }

            if (playerInput.y <= -0.3f)
            {
                followOffsetZ = Mathf.Clamp(
                    transposer.m_FollowOffset.z + Mathf.Abs(playerInput.y * cameraVelocity.z * deltaTime * 10f),
                    followOffset.z,
                    14f);


                followOffsetY = Mathf.Clamp(
                    transposer.m_FollowOffset.y + (playerInput.y * cameraVelocity.z * deltaTime),
                    2f,
                    followOffset.y);

                float trackedOffset = Mathf.Clamp(
                        usedTrackedObjectOffset.x - Mathf.Abs(playerInput.y * cameraVelocity.y * deltaTime),
                        -_trackedObjectOffset.x,
                        _trackedObjectOffset.x);
                usedTrackedOffsetY = trackedOffset;

            }

            usedOffset.x = followOffsetX;
            usedOffset.y = followOffsetY;
            usedOffset.z = followOffsetZ;

            usedTrackedObjectOffset.x = usedTrackedOffsetY;
            usedTrackedObjectOffset.y = usedTrackedOffsetZ;

        }
        private void Look()
        {
            transposer.m_FollowOffset.y = usedOffset.y;
            transposer.m_FollowOffset.x = usedOffset.x;
            transposer.m_FollowOffset.z = usedOffset.z;

            composer.m_TrackedObjectOffset.y = usedTrackedObjectOffset.x;
            composer.m_TrackedObjectOffset.z = usedTrackedObjectOffset.y;
        }

    }
}
