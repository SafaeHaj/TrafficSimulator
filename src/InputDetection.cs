using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetection : MonoBehaviour{

    public event Action<KeyCode> KeyPressed;
    public event Action<Vector3> CursorMoved;

    void Update(){

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.Q)){
            KeyPressed?.Invoke(KeyCode.Q);
        }
        if (Input.GetKeyDown(KeyCode.E)){
            KeyPressed?.Invoke(KeyCode.E);
        }

        if (mouseX != 0 || mouseY != 0){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            CursorMoved?.Invoke(mousePosition);
        }

    }
}
