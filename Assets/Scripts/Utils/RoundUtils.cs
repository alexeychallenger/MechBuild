using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Round utils
    /// </summary>
    static class RoundUtils
    {
        /// <summary>
        /// Round Vector3
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="tolerance"></param>
        public static Vector3 Vector3Round (Vector3 vector, float tolerance)
        {
            Vector3 resultVector = new Vector3
            ( 
                Mathf.Round((vector.x * tolerance) / tolerance),
                Mathf.Round((vector.y * tolerance) / tolerance),
                Mathf.Round((vector.z * tolerance) / tolerance)
            );
            return resultVector;
        }

        /// <summary>
        /// Round Quaternion
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static Quaternion QuaretnioRound(Quaternion vector, float tolerance)
        {
            Quaternion resultVector = new Quaternion
            (
                Mathf.Round((vector.x * tolerance) / tolerance),
                Mathf.Round((vector.y * tolerance) / tolerance),
                Mathf.Round((vector.z * tolerance) / tolerance),
                Mathf.Round((vector.w * tolerance) / tolerance)
            );
            return resultVector;
        }

        public static Vector3 AbsVector3(Vector3 vector)
        {
            Vector3 absVector = new Vector3
            (
                Mathf.Abs(vector.x),
                Mathf.Abs(vector.y),
                Mathf.Abs(vector.z)
            );
            return absVector;
        }
    }
}
