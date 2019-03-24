using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public GameObject paint;

    private Drawing drawing;

    private void Start()
    {
        drawing = FindObjectOfType<Drawing>();
    }
    private void Update()
    {
        if(drawing.IsTouchOver && Input.GetButton("Fire1"))
        {
            Vector3 instancePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            instancePos.z = -2f;
            Instantiate(paint, instancePos, Quaternion.identity);
        }
        else
        {
            Debug.Log("not drawing");
        }
    }
}
