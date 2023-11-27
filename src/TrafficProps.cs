using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generics;

public class TrafficProps : Placeable {
    private SignType _type;
    private VehicleConfig _signStats;
}


public class TrafficSign : TrafficProps {
    public SignType type {get; set;}
    public VehicleConfig signStats {get; set;}
}

public class TrafficLight : TrafficProps {
    private long _cycle;
    private int _redLightDuration;
    private int _greenLightDuration;
    private bool _isRed;
}