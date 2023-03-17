using System;
using Knot.Audio.Attributes;
using Object = UnityEngine.Object;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Dont Destroy On Load")]
    public class KnotDontDestroyOnLoadMod : IKnotAudioDataMod, IKnotAudioGroupMod
    {
        public void Setup(KnotAudioController controller)
        {
            if (KnotAudio.Manager == null)
                Object.DontDestroyOnLoad(controller);
            else controller.transform.SetParent(KnotAudio.Manager.transform);
        }
    }
}
