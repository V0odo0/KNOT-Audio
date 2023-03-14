using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio.Demo
{
    public class KnotDemoGameManager : MonoBehaviour
    {
        [SerializeField] private KnotAudioProjectSettings _projectSettings;


        void Awake()
        {
            KnotAudio.ProjectSettings = _projectSettings;
        }
    }
}
