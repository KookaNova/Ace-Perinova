using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AcePerinova.Controller
{
    public class InputInterpreter : MonoBehaviour, SpacecraftInputs.IFlightActions
    {
        public SpacecraftController sc;

        SpacecraftInputs _controls;

        public void OnEnable() {
            sc = GetComponent<SpacecraftController>();
            _controls = new SpacecraftInputs();

            _controls.Flight.SetCallbacks(this);
            _controls.Flight.Enable();
        
        
        }
        public void OnDisable() {
            _controls.Flight.Disable();
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            Debug.Log("BRAKE");
            sc.thrust = 0;
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
            Debug.Log("THRUST");
            sc.thrust = 1;
        }

        public void OnTorque(InputAction.CallbackContext context)
        {
            sc.rotate = new Vector3(context.ReadValue<Vector2>().y, sc.rotate.y, context.ReadValue<Vector2>().x );
            //x = pitch, y = yaw, z = roll
        }

        public void OnYaw(InputAction.CallbackContext context)
        {
            sc.rotate = new Vector3(sc.rotate.x, context.ReadValue<float>(), sc.rotate.z);
        }

    }
}


