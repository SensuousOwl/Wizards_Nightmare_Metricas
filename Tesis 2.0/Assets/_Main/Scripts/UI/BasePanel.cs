using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public bool IsOpen { get; private set; }

    public event Action OnOpen;
    public event Action OnClose;
    
    public virtual void Open()
    {
        IsOpen = true;
        if (canvas != null)
            canvas.enabled = true;
        else 
            gameObject.SetActive(true);
        
        OnOpen?.Invoke();
    }

    public virtual void Close()
    {
        IsOpen = false;
        
        if (canvas != null)
            canvas.enabled = false;
        else 
            gameObject.SetActive(false);
        
        OnClose?.Invoke();
    }
}
