using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSign : MonoBehaviour, ItrafficProp
{
    public float speedSet;

    public void execute(Vehicle car){
        car.MaxSpeed = speedSet;
    }

}
