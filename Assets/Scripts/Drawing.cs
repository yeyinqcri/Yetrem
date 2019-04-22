using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    private bool isTouchOver = false;
    public List<string> Categories { get; set; }

    public bool IsTouchOver { get => isTouchOver; set => isTouchOver = value; }

    private void OnMouseOver()
    {
        IsTouchOver = true;
    }
    private void OnMouseExit()
    {
        IsTouchOver = false;
    }
    public void EnterFullscreen()
    {
        transform.localScale = new Vector3(1f, 1f,1f);
    }
    public void ExitFullscreen()
    {
        transform.localScale = new Vector3(1f, 0.8851f,1f);
    }
}
