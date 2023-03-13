using System;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Playback Behaviour")]
    public class KnotPlaybackBehaviourMod : IKnotAudioDataMod
    {
        public IKnotPlaybackBehaviour Behaviour
        {
            get => _behaviour;
            set => _behaviour = value;
        }
        [SerializeReference, KnotTypePicker(typeof(IKnotPlaybackBehaviour), false)]
        private IKnotPlaybackBehaviour _behaviour;


        public KnotPlaybackBehaviourMod() { }

        public KnotPlaybackBehaviourMod(IKnotPlaybackBehaviour behaviour)
        {
            _behaviour = behaviour;
        }


        public void Initialize(KnotNativeAudioSourceController sourceController)
        {
            var instance = Behaviour?.GetInstance(sourceController);
            if (instance != null)
                sourceController.PlaybackBehaviours.Add(instance);
        }
    }
}
