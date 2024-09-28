using UnityEditor;
using UnityEngine;

namespace Knot.Audio.Editor
{
    [CustomPreview(typeof(KnotAudioDataAsset))]
    internal class KnotAudioDataAssetPreview : ObjectPreview
    {
        public override bool HasPreviewGUI()
        {
            return m_Targets.Length == 1;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (!(target is KnotAudioDataAsset dataAsset))
                return;
        }

        public override void OnPreviewSettings()
        {
            base.OnPreviewSettings();
            
            if (!(target is KnotAudioDataAsset dataAsset))
                return;

            EditorGUI.BeginDisabledGroup(dataAsset.AudioData.AudioClip == null);
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_PlayButton")))
                PlayClip(dataAsset);

            EditorGUI.EndDisabledGroup();
        }

        internal static void PlayClip(KnotAudioDataAsset dataAsset)
        {
            if (dataAsset == null || dataAsset.AudioData.AudioClip == null)
                return;

            EditorUtils.StopAllPreviewClips();
            EditorUtils.PlayPreviewClip(dataAsset.AudioData.AudioClip, 10000);
        }
    }
}
