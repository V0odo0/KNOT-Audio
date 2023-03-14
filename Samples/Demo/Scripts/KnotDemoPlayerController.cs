using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Knot.Audio.Demo
{
    [RequireComponent(typeof(CharacterController))]
    public class KnotDemoPlayerController : MonoBehaviour
    {
        public CharacterController CharacterController 
            => _characterController ?? (_characterController = GetComponent<CharacterController>());
        private CharacterController _characterController;


        [SerializeField] private float _moveSpeed = 0.1f;
        [SerializeField] private float _jumpHeight = 0.5f;


        void Update()
        {
            var x = Input.GetAxis("Horizontal") * _moveSpeed;
            var z = Input.GetAxis("Vertical") * _moveSpeed;
            float y = 0;

            if (Input.GetKeyDown(KeyCode.Space) && CharacterController.isGrounded)
            {
                y = Mathf.Sqrt(_jumpHeight);
            }

            CharacterController.SimpleMove(new Vector3(x, y, z));
        }
    }
}
