using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knot.Audio.Demo
{
    public class KnotDemoPlayerTriggerEvent : MonoBehaviour
    {
        public UnityEvent Enter;
        public UnityEvent Exit;
        public UnityEvent Stay;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<KnotDemoPlayerController>() != null)
                Enter.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {

            if (other.gameObject.GetComponent<KnotDemoPlayerController>() != null)
                Exit.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {

            if (other.gameObject.GetComponent<KnotDemoPlayerController>() != null)
                Stay.Invoke();
        }

    }
}
