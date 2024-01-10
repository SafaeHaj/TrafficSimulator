using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ItrafficProp{
    void execute(Vehicle Car);
}

public enum LightState{
    Red,
    Green,
    Off,
}


public static class Conts{
    public const float RAYCAST_DISTANCE = 0.3f;
    public const long SPAWN_COOLDOWN = 0;
}


public static class utility{

    public static bool Vector3Equality(Vector3 vector1, Vector3 vector2, float error)
    {
        bool ret = true;

        if (Mathf.Abs(vector1.x - vector2.x) > error)
        {
            ret = false;
        }

        if (Mathf.Abs(vector1.y - vector2.y) > error)
        {
            ret = false;
        }

        if (Mathf.Abs(vector1.z - vector2.z) > error)
        {
            ret = false;
        }

        return ret;
    }

}