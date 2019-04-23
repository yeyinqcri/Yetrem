using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public GameObject paint;
    public Image eraserPanel;
    public int maximumOverridePaint = 20;
    

    private Drawing drawing;

    public Color PaintColor { get; set; }
    private int pencilSize = 10;

    private bool isErasing = false;
    private bool isDrawing = false;
    public bool IsPaused { get; set; }
    public bool IsDrawing { get => isDrawing; set => isDrawing = value; }

    private void Start()
    {
        drawing = FindObjectOfType<Drawing>();
        PaintColor = new Color(128,0,0);
        FindObjectOfType<UIManager>().UpdatePencilSizeColor(PaintColor);
    }
    private void Update()
    {
        if (IsPaused)
            return;

        if (Input.GetButtonDown("Fire1") && drawing.IsTouchOver)
            isDrawing = true;
        if (Input.GetButtonUp("Fire1"))
            isDrawing = false;

        if (isDrawing)
        {
            foreach (CanvasUI o in FindObjectsOfType<CanvasUI>())
                if (o.IsTouchOver)
                    return;

            GameObject[] paintObjects = GameObject.FindGameObjectsWithTag("Paint");
            if (isErasing)
            {
                foreach (GameObject p in paintObjects)
                {
                    if (p.GetComponent<Paint>().IsTouchOver)
                        Destroy(p.gameObject);
                }
                return;
            }

            int overPaint = 0;
            foreach (GameObject p in paintObjects)
            {
                if (p.GetComponent<Paint>().IsTouchOver)
                {
                    overPaint++;
                    if (overPaint > maximumOverridePaint)
                        return;
                }
            }
                

            Vector3 instancePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            instancePos.z = -2f;

            GameObject instance = Instantiate(paint, instancePos, Quaternion.identity,drawing.gameObject.transform);
            instance.GetComponent<SpriteRenderer>().color = PaintColor;
            instance.transform.localScale = new Vector3((float)pencilSize / 10, (float)pencilSize / 10);
        }
    }

    public void ChangeColor(string rgb)
    {
        int r = int.Parse(rgb.Substring(0, 3));
        int g = int.Parse(rgb.Substring(3, 3));
        int b = int.Parse(rgb.Substring(6, 3));

        PaintColor = new Color(r/255F,g / 255F, b / 255F);
        FindObjectOfType<UIManager>().UpdatePencilSizeColor(PaintColor);
    }
    public void ChangeSize(GameObject clickedSize)
    {
        pencilSize = int.Parse(clickedSize.name);
        FindObjectOfType<UIManager>().SetPencilSizeSelected(clickedSize);
    }
    public void ToggleErasingState()
    {
        isErasing = !isErasing;
        if (isErasing)
            eraserPanel.color = Color.red;
        else
            eraserPanel.color = Color.white;
    }
    public void TogglePausedState()
    {
        IsPaused = !IsPaused;
    }
}
