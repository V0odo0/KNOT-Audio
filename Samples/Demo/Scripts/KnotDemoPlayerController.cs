using UnityEngine;

namespace Knot.Audio.Demo
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class KnotDemoPlayerController : MonoBehaviour
    {
        public CharacterController CharacterController 
            => _characterController ?? (_characterController = GetComponent<CharacterController>());
        private CharacterController _characterController;

        public Animator Animator
            => _animator ?? (_animator = GetComponent<Animator>());
        private Animator _animator;

        [SerializeField] private Vector3 _cameraFollowOffset;
        [SerializeField] private float _cameraFollowSpeed = 5;
        [SerializeField] private float _moveSpeed = 0.1f;
        [SerializeField] private float _jumpHeight = 0.5f;
        [SerializeField] private Transform _playerRoot;


        [Header("Sounds")]
        [SerializeField] private KnotAudioDataReference _footstepSound;
        [SerializeField] private KnotAudioDataReference _windInEarsLoopSound;


        void Awake()
        {
            _windInEarsLoopSound.Play(KnotAudioPlayMode.Loop).AttachTo(_playerRoot, Vector3.zero);
        }

        void Update()
        {
            if (Camera.main == null)
                return;

            var camTargetPos = transform.TransformPoint(_cameraFollowOffset);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, camTargetPos, Time.deltaTime * _cameraFollowSpeed);
            
            var moveSpeed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _moveSpeed;
            CharacterController.SimpleMove(moveSpeed);

            Animator.SetFloat("MoveSpeed", CharacterController.velocity.magnitude);
        }


        public void PlayFootstepSound()
        {
            _footstepSound.Play().AttachTo(_playerRoot, Vector3.zero);
        }
    }
}
