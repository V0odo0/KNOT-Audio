#pragma warning disable CS0649

using System.Collections.Generic;
using Knot.Audio.Attributes;
using UnityEngine;

namespace Knot.Audio
{
    [KnotTypeInfo("Project Settings")]
    public class KnotAudioProjectSettings : KnotAudioSettingsProfile
    {
        internal static KnotAudioProjectSettings Empty => _empty ?? (_empty = CreateDefault());
        private static KnotAudioProjectSettings _empty;

        public KnotAudioSettingsProfile CustomSettings => _customSettings;
        [SerializeField, HideInInspector] private KnotAudioSettingsProfile _customSettings;


        public static KnotAudioProjectSettings CreateDefault()
        {
            var instance = CreateInstance<KnotAudioProjectSettings>();

            var uiAudioGroup = new KnotAudioGroup("UI", 
                new KnotDontDestroyOnLoadMod(),
                new KnotSpatialBlendMod(0),
                new KnotBypassConfigMod(true, true, true),
                new KnotAudioListenerSetupMod(true, false));
            instance._audioGroups = new List<KnotAudioGroup>(new[] { uiAudioGroup });

            return instance;
        }
    }
}