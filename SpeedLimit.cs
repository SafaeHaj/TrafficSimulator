using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimit : TrafficProps 
{
    public void Execute(Vehicle Car){
        Car.maxSpeed = maxSpeed;
    }
}