#pragma warning disable CS0649

using Knot.Core;
using UnityEngine;

namespace Knot.Audio
{
    [KnotTypeInfo("Project Settings")]
    public class KnotAudioProjectSettings : KnotAudioSettingsProfile
    {
        public KnotAudioSettingsProfile CustomProfile => _customProfile;
        [SerializeField, HideInInspector] private KnotAudioSettingsProfile _customProfile;
    }
}