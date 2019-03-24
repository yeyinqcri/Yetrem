using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    private bool isTouchOver = false;

    public bool IsTouchOver { get => isTouchOver; set => isTouchOver = value; }

    private void OnMouseOver()
    {
        IsTouchOver = true;
    }
    private void OnMouseExit()
    {
        IsTouchOver = false;
    }
}
