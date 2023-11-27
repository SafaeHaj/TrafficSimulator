namespace MyDataTypes{
    public enum RoadType{
        Straight,
        Crossroad,
        Roundabout
    }

    public enum SignType{
        StopSign,
        YieldSign
    }

    public struct VehicleConfig{
        public float maxSpeed;
        public float acceleration;
    }
}

public static class consts{
    public static float GRID_SIZE = 1.75f;
    public static float RAYCAST_DISTANCE = 2f;
}
