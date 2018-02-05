using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class SerializeUtils<T> where T : ISerializationCallbackReceiver
    {
        public static Action<GameErrorEventArgs> ErrorOccurred;

        public static string Serialize(T objectToSerialize)
        {
            try
            {
                objectToSerialize.OnBeforeSerialize();
                return JsonUtility.ToJson(objectToSerialize);
            }
            catch (Exception ex)
            {
                DbLog.LogError(ex.Message);
                if (ErrorOccurred != null)
                {
                    ErrorOccurred(new GameErrorEventArgs(ex.Message));
                }
                return null;
            }
        }

        public static T Deserialize(string serializedText)
        {
            try
            {
                T deserializedObject = JsonUtility.FromJson<T>(serializedText);
                deserializedObject.OnAfterDeserialize();
                return deserializedObject;
            }
            catch (Exception ex)
            {
                DbLog.LogError(ex.Message);
                if (ErrorOccurred != null)
                {
                    ErrorOccurred(new GameErrorEventArgs(ex.Message));
                }
                return default(T);
            }
        }
    }
}
