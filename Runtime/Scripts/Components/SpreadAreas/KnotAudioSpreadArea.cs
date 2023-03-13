using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public abstract class KnotAudioSpreadArea : MonoBehaviour
    {
        [SerializeField] private List<AudioSource> _audioSources;
        [SerializeField] private List<Transform> _transforms;


        public virtual void Attach(Transform t)
        {

        }

        public virtual void Attach(AudioSource audioSource)
        {

        }

        public virtual void Detach(Transform t)
        {

        }

        public virtual void Detach(AudioSource audioSource)
        {

        }
    }
}
