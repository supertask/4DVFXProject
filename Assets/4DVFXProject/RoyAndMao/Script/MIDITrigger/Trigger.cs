using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using System;
using System.Collections;
using System.Collections.Generic;

namespace VFXProject4D
{
    public sealed class Trigger : MonoBehaviour
    {
        [Serializable] public class UnityEventFloat : UnityEvent<float> { }
        
        [SerializeField] InputAction _action = null;
        [SerializeField] UnityEventFloat _event = null;

        void OnEnable()
        {
            _action.performed += OnPerformed;
            _action.Enable();
        }

        void OnDisable()
        {
            _action.Disable();
            _action.performed -= OnPerformed;
        }

        void OnPerformed(InputAction.CallbackContext ctx)
          => _event.Invoke(ctx.ReadValue<float>());
    }
}
