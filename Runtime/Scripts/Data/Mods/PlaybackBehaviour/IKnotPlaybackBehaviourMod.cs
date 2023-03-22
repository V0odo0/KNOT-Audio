using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public interface IKnotPlaybackBehaviourMod : IKnotAudioDataMod
    {
        IKnotPlaybackBehaviourMod GetInstance(KnotAudioControllerBase controller);

        void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioControllerBase controller);
    }
}
