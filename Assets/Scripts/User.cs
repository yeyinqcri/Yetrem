using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    private Drawing drawing;

    private void Start()
    {
        drawing = FindObjectOfType<Drawing>();
    }
    private void Update()
    {
        if(drawing.IsTouchOver && Input.GetButton("Fire1"))
        {
            Debug.Log("is drawing");
        }
        else
        {
            Debug.Log("not drawing");
        }
    }
}
