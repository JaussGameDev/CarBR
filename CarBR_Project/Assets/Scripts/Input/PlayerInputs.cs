using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Inputs.Mirror
{
    public class PlayerInputs : NetworkBehaviour
    {
        [SerializeField] CarEngine engine = null;
        [SerializeField] Turbo boost = null;
        [SerializeField] BrakeManager brake = null;
        [SerializeField] Car car = null;

        [SerializeField] Vector2 move = Vector2.zero;
        [SerializeField] bool boosting = false;
        [SerializeField] bool flipping = false;
        public override void OnStartAuthority()
        {
            enabled = true;

            engine = GetComponent<CarEngine>();
            brake = GetComponent<BrakeManager>();
            boost = GetComponent<Turbo>();
            car = GetComponent<Car>();

            InputManager.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            InputManager.Controls.Player.Move.canceled += _ => StopMovement();

            InputManager.Controls.Player.Boost.performed += _ => StartBoost();
            InputManager.Controls.Player.Boost.canceled += _ => StopBoost();

            InputManager.Controls.Player.Flip.performed += _ => Flip();

        }

        [Client]
        private void SetMovement(Vector2 input)
        {
            if (!hasAuthority) return;
            move = input;
            engine.SetInput(input);
            brake.SetInput(input);
        }

        [Client]
        private void StopMovement()
        {
            if (!hasAuthority) return;
            move = Vector2.zero;
            engine.SetInput(Vector2.zero);
            brake.SetInput(Vector2.zero);
        }

        [Client]
        private void StartBoost()
        {
            if (!hasAuthority) return;
            boosting = true;
            Debug.Log("PlayerInputs : StartBoost");
            boost.StartBoost();
        }

        [Client]
        private void StopBoost()
        {
            if (!hasAuthority) return;
            boosting = false;
            Debug.Log("PlayerInputs : StopBoost");
            boost.StopBoost();
        }

        [Client]
        private void Flip()
        {
            if (!hasAuthority) return;
            boosting = true;
            car.Flip();
            boosting = false;
        }
    }
}
