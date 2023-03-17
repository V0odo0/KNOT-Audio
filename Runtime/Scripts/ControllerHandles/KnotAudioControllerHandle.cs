using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knot.Audio
{
    public partial struct KnotAudioControllerHandle
    {
        public readonly KnotAudioController Controller;

        public KnotAudioControllerHandle(KnotAudioController controller)
        {
            Controller = controller;
        }
    }
}
