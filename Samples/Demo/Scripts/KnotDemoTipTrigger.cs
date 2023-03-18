using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio.Demo
{
    [RequireComponent(typeof(Collider))]
    public class KnotDemoTipTrigger : MonoBehaviour
    {
        [SerializeField] private int _tipId;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<KnotDemoPlayerController>() != null)
            {
                KnotDemoUIManager.Instance.ShowTipPanel(_tipId);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<KnotDemoPlayerController>() != null)
            {
                KnotDemoUIManager.Instance.HideTipPanel(_tipId);
            }
        }
    }
}
