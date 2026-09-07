using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio.Demo
{
    public class KnotDemoGameManager : MonoBehaviour
    {
        public static KnotDemoGameManager Instance => _instance ?? (_instance = FindObjectOfType<KnotDemoGameManager>());
        private static KnotDemoGameManager _instance;


        public HashSet<int> TipsCollected = new HashSet<int>();

        [SerializeField] private Bounds _levelBounds;
        [SerializeField] private Transform _playerStartPos;
        [SerializeField] private KnotDemoPlayerController _playerController;


        void Awake()
        {
            _playerController.transform.position = _playerStartPos.position;
        }

        void Update()
        {
            if (!_levelBounds.Contains(_playerController.transform.position))
                _playerController.transform.position = _playerStartPos.position;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if (_playerStartPos != null)
                Gizmos.DrawSphere(_playerStartPos.position, 0.25f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_levelBounds.center, _levelBounds.size);

            Gizmos.color = Color.white;
        }


        public void Pause()
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }

        public void UnPause()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }
}
