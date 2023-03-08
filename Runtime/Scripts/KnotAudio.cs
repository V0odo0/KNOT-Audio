using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Knot.Audio
{
    public static class KnotAudio
    {
        internal const string CoreName = "KNOT Audio";
        internal const string Version = "0.1.0";


        internal static void Log(string message, LogType type)
        {
            message = $"{CoreName}: {message}";
            switch (type)
            {
                default:
                    Debug.Log(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
            }
        }


        static KnotAudioSource InstantiateAudioSource()
        {
            var audioSource = new GameObject(nameof(KnotAudioSource)).AddComponent<KnotAudioSource>();

            return audioSource;
        }


        public static KnotAudioSource Play(IKnotAudioDataProvider provider, params IKnotAudioMod[] mods)
        {
            if (provider == null)
                return null;

            return Play(provider.AudioData);
        }

        public static KnotAudioSource Play(IKnotAudioData data, params IKnotAudioMod[] mods)
        {
            if (data == null || data.AudioClip == null)
                return null;

            var audioSource = InstantiateAudioSource();
            audioSource.Initialize(data, mods);
            audioSource.Play();

            return audioSource;
        }

        public static KnotAudioSource Play(AudioClip clip, params IKnotAudioMod[] mods)
        {
            if (clip == null)
                return null;

            var audioSource = InstantiateAudioSource();
            audioSource.Initialize(new KnotAudioData(clip), mods);
            audioSource.Play();

            return audioSource;
        }
    }
}

