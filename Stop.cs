using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSign : TrafficProps 
{
    public void Execute(Vehicle Car){
        Car.isBraking = True;
    }
}