using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Knot.Audio.Editor
{
    internal class KnotBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();
            if (!preloadedAssets.Contains(KnotAudio.SettingsProfile))
                PlayerSettings.SetPreloadedAssets(preloadedAssets.Append(KnotAudio.SettingsProfile).ToArray());
        }
    }
}

