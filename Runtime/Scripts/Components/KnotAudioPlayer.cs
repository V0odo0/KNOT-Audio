using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [AddComponentMenu(KnotAudio.CoreName + "/Audio Player", 1000)]
    public class KnotAudioPlayer : MonoBehaviour
    {
        public virtual IList<IKnotAudioDataProvider> AudioDataProviders
        {
            get => _audioDataProviders ?? (_audioDataProviders = new List<IKnotAudioDataProvider>());
            set
            {
                if (value == null)
                    _audioDataProviders?.Clear();
                else _audioDataProviders = value.ToList();
            }
        }
        [SerializeReference, KnotTypePicker(typeof(IKnotAudioDataProvider))]
        private List<IKnotAudioDataProvider> _audioDataProviders;

        public virtual bool PlayOnAwake
        {
            get => _playOnAwake;
            set => _playOnAwake = value;
        }
        [SerializeField] private bool _playOnAwake;


        protected virtual void Awake()
        {
            if (PlayOnAwake)
                Play();
        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void Reset()
        {
            _audioDataProviders = new List<IKnotAudioDataProvider>(new []{new KnotInstanceAudioDataProvider()});
        }


        public virtual void Play()
        {
            if (AudioDataProviders.Count > 0)
                KnotAudio.Play(AudioDataProviders[0]);
        }

        public virtual void Play(int id)
        {
            if (AudioDataProviders.Count >= id - 1)
                KnotAudio.Play(AudioDataProviders[id]);
        }
    }
}
