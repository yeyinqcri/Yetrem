using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public GameObject paint;
    public Image eraserPanel;
    

    private Drawing drawing;

    private Color paintColor = Color.red;
    private int pencilSize = 10;

    private bool isErasing = false;
    private bool isDrawing = false;

    private void Start()
    {
        drawing = FindObjectOfType<Drawing>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && drawing.IsTouchOver)
            isDrawing = true;
        if (Input.GetButtonUp("Fire1"))
            isDrawing = false;

        if (isDrawing)
        {
            if (isErasing)
            {
                foreach (GameObject p in GameObject.FindGameObjectsWithTag("Paint"))
                {
                    if (p.GetComponent<Paint>().IsTouchOver)
                        Destroy(p.gameObject);
                }
                return;
            }

            Vector3 instancePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            instancePos.z = -2f;

            GameObject instance = Instantiate(paint, instancePos, Quaternion.identity);
            instance.GetComponent<SpriteRenderer>().color = paintColor;
            instance.transform.localScale = new Vector3((float)pencilSize / 10, (float)pencilSize / 10);
        }
    }

    public void ChangeColor(GameObject clickedColor)
    {
        paintColor = clickedColor.GetComponent<Image>().color;
    }
    public void ChangeSize(GameObject clickedSize)
    {
        pencilSize = int.Parse(clickedSize.name);
    }
    public void ToggleErasingState()
    {
        isErasing = !isErasing;
        if (isErasing)
            eraserPanel.color = Color.red;
        else
            eraserPanel.color = Color.white;
    }
}
