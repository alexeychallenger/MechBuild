using UnityEngine;
using Assets.Scripts.Utils;
using System.Collections;
using Assets.Scripts;

namespace Assets.Scripts
{
    /// <summary>
    /// Скрипт управления камерой
    /// </summary>
    public class CameraScript : MonoBehaviour
    {
        /// <summary>
        /// Target on which the camera is looking
        /// </summary>
        [Header("Refs")]
        public Transform target;
        /// <summary>
        /// Target position pointer
        /// </summary>
        public Transform targetPointer;
        /// <summary>
        /// Camera pointer
        /// </summary>
        public Transform cameraPointer;
        /// <summary>
        /// Camera container
        /// </summary>
        public Transform cameraContainer;
        public Camera camera;

        [Header("Stats")]

        public float followSpeed = 3f;                      //скорость следования камеры за целью
        public float zoomSpeed = 2f;                        //скорость зума
        public float rotateSpeedX;
        public float rotateSpeedY = 3f;                      //скорость поворота
        public Vector3 offset = new Vector3(0, 0, -5);      //положение камеры относительно якоря
        public float vectorTolerance = 0.01f;               //точность округление координат
        public float quaterTolerance = 0.0001f;             //точность округление осей

        public float maxXAngle = 75;

        public float minZoom = 1f;                          //минимальная дальность зума
        public float maxZoom = 30f;                         //максимальная дальность зума

        void Start()
        {
            cameraPointer.localPosition = offset;
            camera.transform.localPosition = offset;   //присвоение смещение камеры относительно центра объекта
        }

        void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(2))                //при нажатии средней кнопки мыши определяется объект на который указывает курсор
            {
                SelectPivot();
            }
            if (Input.GetMouseButton(1))                    //при зажатии правой кнопки мыши разблокируется вращение камеры
            {
                CameraRotate();
            }
            CameraZoom();
        }


        private void LateUpdate()
        {
            //выравнивание оси z
            targetPointer.rotation = Quaternion.Euler(targetPointer.rotation.eulerAngles.x,
                                                      targetPointer.rotation.eulerAngles.y,
                                                      0.0f);

            cameraContainer.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x,
                                                        camera.transform.rotation.eulerAngles.y,
                                                        0.0f);
            FollowTarget();

        }

        /// <summary>
        /// Move camera behind the object
        /// </summary>
        private void FollowTarget()
        {
            if (target != null)
            {
                if (target.tag != "terrain")
                {
                    targetPointer.position = target.position;
                }
            }

            //сглаживание перемещения указателя на объект
            if (RoundUtils.Vector3Round(targetPointer.localPosition, vectorTolerance) !=
                RoundUtils.Vector3Round(cameraContainer.localPosition, vectorTolerance))
            {
                cameraContainer.localPosition = Vector3.Lerp(cameraContainer.localPosition, targetPointer.localPosition, followSpeed * Time.deltaTime);
            }

            //сглаживание зума камеры
            if (RoundUtils.Vector3Round(camera.transform.position, vectorTolerance) !=
                RoundUtils.Vector3Round(cameraPointer.position, vectorTolerance))
            {
                camera.transform.position = Vector3.Lerp(camera.transform.position, cameraPointer.position, followSpeed * Time.deltaTime);
            }

            //сглаживание поворотов камеры вокруг цели
            if (RoundUtils.Vector3Round(cameraContainer.eulerAngles, quaterTolerance) != RoundUtils.Vector3Round(targetPointer.eulerAngles, quaterTolerance))
            {
                cameraContainer.localRotation = Quaternion.Lerp(cameraContainer.localRotation, targetPointer.localRotation, followSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Select pivot point for camera
        /// </summary>
        private void SelectPivot()
        {
            RaycastHit hit;
            //создается луч который определяет объект на который указывает курсор мыши
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //определяет место соприкосновения коллайдера луча и объекта
            if (Physics.Raycast(ray, out hit))
            {
                //определение объекта, на который указывает мышь и назначение его целью
                target = hit.transform;
                if (target.name == "Terrain")
                {
                    targetPointer.position = hit.point;
                    DbLog.Log(string.Format("Terrain point ({0}) selected.", targetPointer.position), Color.grey, this);
                }
                else
                {
                    targetPointer.position = target.position;
                    DbLog.Log(string.Format("{0}'s point ({1}) selected.", target.name, targetPointer.position), Color.grey, this);
                }
            }
            else
            {
                target = null;
                DbLog.Log("No target.", Color.grey, this);
            }
        }

        /// <summary>
        /// Rotating the camera position pointer
        /// </summary>
        private void CameraRotate()
        {
            float currentRotateSpeedX = ConvertXAngleToYAxisRotateSpeed(targetPointer.eulerAngles.x);

            if (Input.GetAxis("Mouse Y") < 0)
            {
                targetPointer.Rotate(rotateSpeedY, 0, 0);
            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                targetPointer.Rotate(-rotateSpeedY, 0, 0);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                targetPointer.Rotate(0, -currentRotateSpeedX, 0);
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                targetPointer.Rotate(0, currentRotateSpeedX, 0);
            }
            AngleClamp();
        }


        /// <summary>
        /// Converting the angle of inclination of the camera on the X axis into the rotation speed of the camera around the Y axis at a given angle of inclination
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public float ConvertXAngleToYAxisRotateSpeed(float x)
        {
            float res;
            if (x > 270 && x < 360)
            {
                x = 360 - x;
            }
            x = 90 - x;
            res = (x * 0.78f);
            res = Mathf.Sqrt(2 * Mathf.Pow(res, 2));
            res = (res * rotateSpeedX);
            return res;
        }

        /// <summary>
        /// Зуммирование указателя положения камеры
        /// </summary>
        private void CameraZoom()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                cameraPointer.localPosition += new Vector3(0, 0, zoomSpeed);
                cameraPointer.localPosition = new Vector3
                (
                    cameraPointer.localPosition.x,
                    cameraPointer.localPosition.y,
                    Mathf.Clamp(cameraPointer.localPosition.z, -maxZoom, -minZoom)
                );
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                cameraPointer.localPosition -= new Vector3(0, 0, zoomSpeed);
                cameraPointer.localPosition = new Vector3
                (
                    cameraPointer.localPosition.x,
                    cameraPointer.localPosition.y,
                    Mathf.Clamp(cameraPointer.localPosition.z, -maxZoom, -minZoom)
                );
            }
        }

        /// <summary>
        /// Ограничивает угол наклона камеры по оси Х
        /// </summary>
        private void AngleClamp()
        {
            if ((targetPointer.eulerAngles.x > maxXAngle) && (targetPointer.eulerAngles.x <= 90))
            {
                targetPointer.eulerAngles = new Vector3
                (
                    maxXAngle,
                    targetPointer.eulerAngles.y,
                    targetPointer.eulerAngles.z
                );
            }
            else if ((targetPointer.eulerAngles.x > 270) && (targetPointer.eulerAngles.x <= 360 - maxXAngle))
            {
                targetPointer.eulerAngles = new Vector3
                (
                    -maxXAngle,
                    targetPointer.eulerAngles.y,
                    targetPointer.eulerAngles.z
                );
            }
        }

    }
}
