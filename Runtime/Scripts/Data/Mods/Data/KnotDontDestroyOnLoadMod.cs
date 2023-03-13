using System;
using Knot.Audio.Attributes;
using Object = UnityEngine.Object;

namespace Knot.Audio
{
    [Serializable]
    [KnotTypeInfo(displayName: "Dont Destroy On Load")]
    public class KnotDontDestroyOnLoadMod : IKnotAudioDataMod, IKnotAudioGroupMod
    {
        public void Initialize(KnotAudioSourceController sourceController)
        {
            if (KnotAudio.Manager == null)
                Object.DontDestroyOnLoad(sourceController);
            else sourceController.transform.SetParent(KnotAudio.Manager.transform);
        }
    }
}
