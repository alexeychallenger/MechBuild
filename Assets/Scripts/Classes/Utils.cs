using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Scripts.Classes
{
    /// <summary>
    /// Утилиты
    /// </summary>
    static class Utils
    {

        /// <summary>
        /// Округление Vector3 к заданной точности
        /// </summary>
        /// <param name="vector">Вектор для округления</param>
        /// <param name="tolerance">Заданная точность</param>
        public static Vector3 Vector3Round (Vector3 vector, float tolerance)
        {
            Vector3 resultVector = new Vector3( 
                                                Mathf.Round((vector.x * tolerance) / tolerance),
                                                Mathf.Round((vector.y * tolerance) / tolerance),
                                                Mathf.Round((vector.z * tolerance) / tolerance)
                                              );
            return resultVector;
        }

        /// <summary>
        /// Округление Quaternion к заданной точности
        /// </summary>
        /// <param name="vector">Quaternion для округления</param>
        /// <param name="tolerance">Заданная точность</param>
        /// <returns></returns>
        public static Quaternion AngleRound(Quaternion vector, float tolerance)
        {
            Quaternion resultVector = new Quaternion(   
                                                        Mathf.Round((vector.x * tolerance) / tolerance),
                                                        Mathf.Round((vector.y * tolerance) / tolerance),
                                                        Mathf.Round((vector.z * tolerance) / tolerance),
                                                        Mathf.Round((vector.w * tolerance) / tolerance)
                                                    );
            return resultVector;
        }

        

    }
}
