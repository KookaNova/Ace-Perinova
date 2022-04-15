using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace AcePerinova.Controller
{
    public class InputInterpreter : MonoBehaviour, SpacecraftInputs.IFlightActions
    {
        PlayerController sc;
        
        public float thrust, brake, yaw;
        public Vector2 torque, cameraInput;
        public bool pIsFiring, sIsFiring;
        [HideInInspector]
        public UnityEvent 
            cameraChangedEvent,
            teamIncrementEvent,
            teamDecrementEvent,
            targetSelectEvent,
            cameraFollowToggleEvent;



        SpacecraftInputs _controls;

        public void OnEnable() {
            //sc = GetComponent<PlayerController>();
            _controls = new SpacecraftInputs();

            _controls.Flight.SetCallbacks(this);
            _controls.Flight.Enable();
        
        
        }
        public void OnDisable() {
            _controls.Flight.Disable();
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            brake = context.ReadValue<float>();
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
            thrust = context.ReadValue<float>();
        }

        public void OnTorque(InputAction.CallbackContext context)
        {
            //input y = pitch, input x = roll
            torque = context.ReadValue<Vector2>();
        }

        public void OnYaw(InputAction.CallbackContext context)
        {
            yaw = context.ReadValue<float>();
        }

        public void OnCameraOrientation(InputAction.CallbackContext context)
        {
            cameraInput = context.ReadValue<Vector2>();
        }

        public void OnPrimaryWeapon(InputAction.CallbackContext context)
        {
            pIsFiring = context.ReadValueAsButton();
        }

        public void OnSecondaryWeapon(InputAction.CallbackContext context)
        {
            sIsFiring = context.ReadValueAsButton();
        }

        public void OnCameraChange(InputAction.CallbackContext context)
        {
            if(context.performed)
            cameraChangedEvent.Invoke();
        }

        public void OnTargetTeamIncrement(InputAction.CallbackContext context)
        {
            if(context.performed)
            teamIncrementEvent.Invoke();
        }

        public void OnTargetTeamDecrement(InputAction.CallbackContext context)
        {
            if(context.performed)
            teamIncrementEvent.Invoke();
        }

        public void OnTargetSelect(InputAction.CallbackContext context)
        {
            if(context.performed)
            targetSelectEvent.Invoke();
        }

        public void OnToggleCameraFollow(InputAction.CallbackContext context)
        {
            if(context.performed)
            cameraFollowToggleEvent.Invoke();
        }
    }
}


