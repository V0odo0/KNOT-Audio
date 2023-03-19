using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio.Demo
{
    [RequireComponent(typeof(Collider))]
    public class KnotDemoTipTrigger : MonoBehaviour
    {
        [SerializeField] private int _tipId;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _isCollectedMaterial;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<KnotDemoPlayerController>() != null)
            {
                KnotDemoUIManager.Instance.ShowTipPanel(_tipId);
                _renderer.material = _isCollectedMaterial;
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
