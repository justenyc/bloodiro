using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Quickjam.Player
{
    public abstract class PlayerState
    {
        public string m_stateName { get; protected set; }
        public virtual void StateFixedUpdate() { }
        public virtual void OnStateControllerColliderHit(ControllerColliderHit hit) { }
        public virtual void OnStateTriggerEnter(Collider other) { }
        public virtual void OnStateTriggerExit(Collider other) { }
    }
}
