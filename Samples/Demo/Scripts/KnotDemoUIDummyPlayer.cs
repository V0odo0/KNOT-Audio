using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Knot.Audio.Demo
{
    public class KnotDemoUIDummyPlayer : MonoBehaviour
    {
        [SerializeField] private KnotAudioDataReference _audioDataRef;
        [SerializeField] private Transform _audioPivot;
        [SerializeField] private List<Toggle> _allModToggles;

        public void Play()
        {
            var selectedMods = _audioDataRef.Provider.AudioData.GetAllMods().Where((mod, i) => _allModToggles[i].isOn).ToArray();

            _audioDataRef.Provider.AudioData.AudioClip.Play(KnotAudioPlayMode.OneShot, selectedMods).AttachTo(_audioPivot, Vector3.zero);
        }
    }
}
