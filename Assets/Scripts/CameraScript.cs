using UnityEngine;
using Assets.Scripts.Classes;
using System.Collections;

/// <summary>
/// Скрипт управления камерой
/// </summary>
public class CameraScript : MonoBehaviour
{
    [Header("Refs")]

    public Transform target;                            //целевой объект на которую смотрит камера
    public Transform targetPointer;                     //указатель положения цели
    public Transform cameraPointer;                     //указатель положения камеры
    public Transform cameraContainer;                   //контейнер с камерой и указателями ее положения
    public Camera gameCamera;                           //камера

    [Header("Stats")]

    public float followSpeed = 3f;                      //скорость следования камеры за целью
    public float zoomSpeed = 2f;                        //скорость зума
    public float rotateSpeed = 3f;                      //скорость поворота
    public Vector3 offset = new Vector3(0, 0, -5);      //положение камеры относительно якоря
    public float vectorTolerance = 0.01f;               //точность округление координат
    public float quaterTolerance = 0.0001f;             //точность округление осей

    public float maxXAngle = 75;

    public float minZoom = 1f;                          //минимальная дальность зума
    public float maxZoom = 30f;                         //максимальная дальность зума

    private float rotateSpeedX;                         //скорость поворота по оси Х

    void Start()
    {
        rotateSpeedX = rotateSpeed;
        cameraPointer.localPosition = offset;
        gameCamera.GetComponent<Transform>().localPosition = offset;   //присвоение смещение камеры относительно центра объекта
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

        cameraContainer.rotation = Quaternion.Euler(gameCamera.transform.rotation.eulerAngles.x,
                                                    gameCamera.transform.rotation.eulerAngles.y,
                                                    0.0f);    
        FollowTarget();

    }



    /// <summary>
    /// функция следования камеры за объектом
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
        if (Utils.Vector3Round(targetPointer.localPosition, vectorTolerance) !=
            Utils.Vector3Round(cameraContainer.localPosition, vectorTolerance))
        {
            cameraContainer.localPosition = Vector3.Lerp(cameraContainer.localPosition, targetPointer.localPosition, followSpeed * Time.deltaTime);
        }

        //сглаживание зума камеры
        if (Utils.Vector3Round(gameCamera.transform.position, vectorTolerance) !=
            Utils.Vector3Round(cameraPointer.position, vectorTolerance))
        {
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, cameraPointer.position, followSpeed * Time.deltaTime);
        }

        //сглаживание поворотов камеры вокруг цели
        if (Utils.Vector3Round(cameraContainer.eulerAngles, quaterTolerance) != Utils.Vector3Round(targetPointer.eulerAngles, quaterTolerance))
        {
            cameraContainer.localRotation = Quaternion.Lerp(cameraContainer.localRotation, targetPointer.localRotation, followSpeed * Time.deltaTime);
        }


    }

    /// <summary>
    /// функция определения положения якоря камеры
    /// </summary>
    private void SelectPivot()
    {
        RaycastHit hit;
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);                         //создается луч который определяет объект на который указывает курсор мыши
        if (Physics.Raycast(ray, out hit))                                                  //определяет место соприкосновения коллайдера луча и объекта
        {

            target = hit.transform;                                                         //определение объекта, на который указывает мышь и назначение его целью
            if (target.name == "Terrain")
            {
                targetPointer.position = hit.point;
                Debug.Log("Terrain point selected; point is " + targetPointer.position);
            }
            else
            {
                targetPointer.position = target.position;
                Debug.Log(target.name + "; " + targetPointer.position);
            }
        }
        else
        {
            target = null;                                                                       //сброс цели в случае неудачи
            //Debug.Log("No target");
        }
    }

    /// <summary>
    /// Вращение указателя положения камеры
    /// </summary>
    private void CameraRotate()                                                                 
    {

        rotateSpeedX = RotateSpeedConvert(targetPointer.eulerAngles.x); 

        if (Input.GetAxis("Mouse Y") < 0)
        {
            targetPointer.Rotate(rotateSpeed, 0, 0);
        }
        if (Input.GetAxis("Mouse Y") > 0)
        {
            targetPointer.Rotate(-rotateSpeed, 0, 0);
        }
        if (Input.GetAxis("Mouse X") < 0)
        {
            targetPointer.Rotate(0, -rotateSpeedX, 0);
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            targetPointer.Rotate(0, rotateSpeedX, 0);
        }

        AngleClamp();

    }

    
    /// <summary>
    /// Узкоспециализированная функция конвертации угла наклона камеры на оси X в скорость поворота камеры вокруг оси Y на данном угле наклона
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public float RotateSpeedConvert (float x)
    {
        float res;

        if ((x > 270) && (x < 360))
        {
            x = 360 - x;
        }

        x = 90 - x;
        res = (float) (x / 0.9 * 0.7);

        res = Mathf.Sqrt(2 * Mathf.Pow(res, 2));

        res = (float) (res / 0.99 * 0.01*rotateSpeed);

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
            cameraPointer.localPosition = new Vector3(
                                                    cameraPointer.localPosition.x,
                                                    cameraPointer.localPosition.y,
                                                    Mathf.Clamp(cameraPointer.localPosition.z, -maxZoom, -minZoom)
                                                );
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraPointer.localPosition -= new Vector3(0, 0, zoomSpeed);
            cameraPointer.localPosition = new Vector3(
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
            targetPointer.eulerAngles = new Vector3(
                                                        maxXAngle,
                                                        targetPointer.eulerAngles.y,
                                                        targetPointer.eulerAngles.z
                                                    );
        }
        else if ((targetPointer.eulerAngles.x > 270) && (targetPointer.eulerAngles.x <= 360 - maxXAngle))
        {
            targetPointer.eulerAngles = new Vector3(
                                                       -maxXAngle,
                                                        targetPointer.eulerAngles.y,
                                                        targetPointer.eulerAngles.z
                                                    );
        }
    }

}
