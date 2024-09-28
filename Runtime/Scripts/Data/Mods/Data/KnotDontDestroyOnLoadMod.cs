using System;
using Knot.Core;
using Object = UnityEngine.Object;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Dont Destroy On Load")]
    public class KnotDontDestroyOnLoadMod : IKnotAudioDataMod, IKnotAudioGroupMod
    {
        public void Setup(KnotAudioControllerBase controller)
        {
            if (KnotAudio.Manager == null)
                Object.DontDestroyOnLoad(controller);
            else controller.transform.SetParent(KnotAudio.Manager.transform);
        }
    }

    public partial struct KnotAudioControllerHandle
    {
        public KnotAudioControllerHandle DontDestroyOnLoad()
        {
            if (Controller != null)
                Controller.AppendMods(new KnotDontDestroyOnLoadMod());

            return this;
        }
    }
}
