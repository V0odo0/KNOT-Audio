using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public interface IKnotPlaybackBehaviourMod : IKnotAudioDataMod
    {
        IKnotPlaybackBehaviourMod GetInstance(KnotAudioController controller);

        void OnBehaviourStateEvent(KnotPlaybackBehaviourEvent behaviourEvent, KnotAudioController controller);
    }
}
