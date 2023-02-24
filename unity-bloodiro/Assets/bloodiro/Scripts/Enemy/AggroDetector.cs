using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

namespace Quickjam.Enemy
{
    public class AggroDetector : MonoBehaviour
    {
        System.Action<PlayerController> _triggerEnterCallback;
        System.Action<PlayerController> _triggerStayCallback;
        System.Action _triggerExitCallback;

        public void Initialize(System.Action<PlayerController> triggerEnterCallback, System.Action<PlayerController> triggerStayCallback, System.Action triggerExitCallback)
        {
            _triggerEnterCallback = triggerEnterCallback;
            _triggerStayCallback = triggerStayCallback;
            _triggerExitCallback = triggerExitCallback;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_triggerEnterCallback != null && other.CompareTag("Player"))
            {
                _triggerEnterCallback(other.GetComponent<PlayerController>());
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_triggerStayCallback != null && other.CompareTag("Player"))
            {
                _triggerStayCallback(other.GetComponent<PlayerController>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(_triggerExitCallback != null)
            {
                _triggerExitCallback();
            }
        }
    }
}
