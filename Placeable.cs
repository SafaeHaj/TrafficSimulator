using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generics;

public class Placeable : MonoBehaviour {
    private vector3 _gridPosition;

    public vector3 gridPosition {
        
        get {return _gridPosition;}

        set (int x, int y, int z = 0) {
            if (_gridPosition == null) {
                _gridPosition = new vector3(x, y, z);
            }

            else {
                _gridPosition.set(x, y, z);
            }
        }
    }
}

