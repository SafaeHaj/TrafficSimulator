using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour, ItrafficProp
{      
    public Sprite red, green, off;
    public LightState state = LightState.Off;
    public float CycleDuration;

    private long _timer;

    void Update(){

        if (System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() - _timer <= CycleDuration)
            return; 
        _timer = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (state == LightState.Red){
            state = LightState.Green;
            GetComponent<SpriteRenderer>().sprite = green;
        }
        else{
            state = LightState.Red;
            GetComponent<SpriteRenderer>().sprite = red;
        }

    }

    public void execute(Vehicle car){
        if (state == LightState.Red){
            car.isBraking = true;
        }
    }
}
