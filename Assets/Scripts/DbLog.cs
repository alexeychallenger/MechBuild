using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class DbLog : MonoBehaviour
    {
        private static bool enableLoggingToConsole = true;

        public static void LogMethodCall(string methodName, object sender)
        {
            Log(string.Format("[{0}] called in [{1}] object", methodName, sender));
        }

        public static void Log(object message)
        {
            if (enableLoggingToConsole) Debug.Log(message);
        }

        public static void Log(object message, Color color, object sender)
        {
            if (enableLoggingToConsole) Debug.LogFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), sender, message);
        }

        public static void Log(object message, Color color, Object context)
        {
            if (enableLoggingToConsole) Debug.LogFormat(context, "<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
        }

        public static void Log(object message, Object context)
        {
            if (enableLoggingToConsole) Debug.Log(message, context);

        }

        public static void LogFormat(Object context, string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogFormat(context, format, args);

        }

        public static void LogFormat(string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogFormat(format, args);

        }

        public static void LogWarning(object message, Object context)
        {
            if (enableLoggingToConsole) Debug.LogWarning(message);
        }

        public static void LogWarning(object message)
        {
            if (enableLoggingToConsole) Debug.LogWarning(message);
        }

        public static void LogError(object message)
        {
            if (enableLoggingToConsole) Debug.LogError(message);
        }

        public static void LogError(object message, Object context)
        {
            if (enableLoggingToConsole) Debug.LogError(message);
        }

        public static void LogAssertion(object message)
        {
            if (enableLoggingToConsole) Debug.LogAssertion(message);
        }

        public static void LogAssertion(object message, Object context)
        {
            if (enableLoggingToConsole) Debug.LogAssertion(message);
        }


        public static void LogException(System.Exception exception)
        {
            if (enableLoggingToConsole) Debug.LogException(exception);
        }

        private static string GetColorHexString(Color color)
        {
            string colorString = string.Empty;
            colorString += ((int)(color.r * 255)).ToString("X02");
            colorString += ((int)(color.g * 255)).ToString("X02");
            colorString += ((int)(color.b * 255)).ToString("X02");
            return colorString;
        }

    }
}






