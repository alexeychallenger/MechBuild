using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class DbLog : MonoBehaviour
    {
        private static bool enableLoggingToConsole = true;

        #region Log

        public static void Log(object message)
        {
            if (enableLoggingToConsole) Debug.Log(message);
        }

        public static void Log(object message, Color color, object context)
        {
#if UNITY_EDITOR
            if (enableLoggingToConsole) Debug.LogFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
#else
                Log(message, context);
#endif
        }

        public static void Log(object message, Color color, Object context)
        {
#if UNITY_EDITOR
            if (enableLoggingToConsole) Debug.LogFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
#else
                Log(message, context);
#endif
        }

        public static void Log(object message, Object context)
        {
            if (enableLoggingToConsole) Debug.Log(message, context);
        }

        public static void Log(object message, object context)
        {
            if (enableLoggingToConsole) Debug.LogFormat("[{0}]: {1}", context, message);
        }

        public static void LogFormat(Object context, string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogFormat(context, format, args);

        }

        public static void LogFormat(string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogFormat(format, args);

        }

        #endregion Log

        #region LogWarning
        public static void LogWarning(object message)
        {
            if (enableLoggingToConsole) Debug.LogWarning(message);
        }

        public static void LogWarning(object message, Color color, object context)
        {
#if UNITY_EDITOR
            if (enableLoggingToConsole) Debug.LogWarningFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
#else
                LogWarning(message, context);
#endif
        }

        public static void LogWarning(object message, Color color, Object context)
        {
#if UNITY_EDITOR
            if (enableLoggingToConsole) Debug.LogWarningFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
#else
                LogWarning(message, context);
#endif
        }

        public static void LogWarning(object message, Object context)
        {
            if (enableLoggingToConsole) Debug.LogWarning(message, context);
        }

        public static void LogWarning(object message, object context)
        {
            if (enableLoggingToConsole) Debug.LogWarningFormat("[{0}]: {1}", context, message);
        }

        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogWarningFormat(context, format, args);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogWarningFormat(format, args);
        }

        #endregion LogWarning

        #region LogError

        public static void LogError(object message)
        {
            if (enableLoggingToConsole) Debug.LogError(message);
        }

        public static void LogError(object message, Color color, object context)
        {
#if UNITY_EDITOR
            if (enableLoggingToConsole) Debug.LogErrorFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
#else
                LogError(message, context);
#endif
        }

        public static void LogError(object message, Color color, Object context)
        {
#if UNITY_EDITOR
            if (enableLoggingToConsole) Debug.LogErrorFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
#else
                LogError(message, context);
#endif
        }

        public static void LogError(object message, Object context)
        {
            if (enableLoggingToConsole) Debug.LogError(message, context);
        }

        public static void LogError(object message, object context)
        {
            if (enableLoggingToConsole) Debug.LogErrorFormat("[{0}]: {1}", context, message);
        }

        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogErrorFormat(context, format, args);

        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogErrorFormat(format, args);

        }

        #endregion LogError

        #region LogAssertion

        public static void LogAssertion(object message)
        {
            if (enableLoggingToConsole) Debug.LogAssertion(message);
        }

        public static void LogAssertion(object message, Color color, object context)
        {
#if UNITY_EDITOR
            if (enableLoggingToConsole) LogAssertionFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
#else
                LogAssertion(message, context);
#endif
        }

        public static void LogAssertion(object message, Color color, Object context)
        {
#if UNITY_EDITOR
            if (enableLoggingToConsole) Debug.LogAssertionFormat("<color=#{0}>[{1}]</color>: {2}", GetColorHexString(color), context, message);
#else
                LogAssertion(message, context);
#endif
        }

        public static void LogAssertion(object message, Object context)
        {
            if (enableLoggingToConsole) Debug.LogAssertion(message, context);
        }

        public static void LogAssertion(object message, object context)
        {
            if (enableLoggingToConsole) Debug.LogAssertionFormat("[{0}]: {1}", context, message);
        }

        public static void LogAssertionFormat(Object context, string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogAssertionFormat(context, format, args);

        }

        public static void LogAssertionFormat(string format, params object[] args)
        {
            if (enableLoggingToConsole) Debug.LogAssertionFormat(format, args);

        }

        public static void LogException(System.Exception exception)
        {
            if (enableLoggingToConsole) Debug.LogException(exception);
        }

        #endregion LogAssertion

        public static void LogMethodCall(string methodName, object sender)
        {
            Log(string.Format("[{0}] called in [{1}] object", methodName, sender));
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






