using System.Collections;
using System.Collections.Generic;
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
    }
}

