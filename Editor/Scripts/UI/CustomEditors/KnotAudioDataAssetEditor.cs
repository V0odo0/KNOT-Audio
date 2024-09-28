using UnityEditor;

namespace Knot.Audio.Editor
{
    [CustomEditor(typeof(KnotAudioDataAsset), true)]
    [CanEditMultipleObjects]
    internal class KnotAudioDataAssetEditor : UnityEditor.Editor
    {
        void OnDisable()
        {
            EditorUtils.StopAllPreviewClips();
        }
    }
}

