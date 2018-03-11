using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(GInputField))]
public class GInputField : MonoBehaviour
{
    public static InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
    }
}
