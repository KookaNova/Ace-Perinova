using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AcePerinova.Controller
{
    public class InputInterpreter : MonoBehaviour, SpacecraftInputs.IFlightActions
    {
        public PlayerController sc;

        SpacecraftInputs _controls;

        public void OnEnable() {
            sc = GetComponent<PlayerController>();
            _controls = new SpacecraftInputs();

            _controls.Flight.SetCallbacks(this);
            _controls.Flight.Enable();
        
        
        }
        public void OnDisable() {
            _controls.Flight.Disable();
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            sc.brake = context.ReadValue<float>();
        }

        public void OnStart(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnTab(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnThrust(InputAction.CallbackContext context)
        {
            sc.thrust = context.ReadValue<float>();
        }

        public void OnTorque(InputAction.CallbackContext context)
        {
            //input y = pitch, input x = roll
            var rot = context.ReadValue<Vector2>();
            sc.pitch = rot.y; sc.roll = rot.x;
        }

        public void OnYaw(InputAction.CallbackContext context)
        {
            sc.yaw = context.ReadValue<float>();
        }

    }
}


