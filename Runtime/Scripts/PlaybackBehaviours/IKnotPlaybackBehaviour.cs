using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public interface IKnotPlaybackBehaviour
    {
        IKnotPlaybackBehaviour GetInstance(KnotAudioSourceController sourceController);

        void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioSourceController sourceController);
    }
}
