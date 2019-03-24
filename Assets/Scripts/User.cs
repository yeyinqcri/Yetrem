using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public GameObject paint;

    

    private Drawing drawing;
    private Color paintColor = Color.red;

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
            GameObject instance = Instantiate(paint, instancePos, Quaternion.identity);

            instance.GetComponent<SpriteRenderer>().color = paintColor;
        }
        else
        {
            Debug.Log("not drawing");
        }
    }

    public void ChangeColor(GameObject clickedColor)
    {
        paintColor = clickedColor.GetComponent<Image>().color;
    }
}
