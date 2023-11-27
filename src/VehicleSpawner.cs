using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculeSpawner : Monobehaviour 
{
    
void Start{
}

void Update{
}

public void SpawCarAtLocation(Transform SpawnPoint, GameObject Car){
    if(SpawnPoint != NULL && Car != NULL){
        Instantiate(Car,SpawnPoint.position,SpawnPoint.rotation);
    }
    else{
        Debug.LogWarning("Car or Point not set!");
    }
}

public void ConfigureCar(GameObject Car, float speed, string type, Color color){
    if(car != NULL){
        CarScript carScript = Car.GetComponent<CarScript>();
        if(carScript != NULL){
            car.SetType(type);
            car.SetSpeed(speed);
            car.SetColor(color);
        }
        else{
            Debug.LogWarning("The CarScript is NULL");
        }
    }
    else{
        Debug.LogWarning("The car is NULL");
    }
}

private void setType(GameObject Car, string type){
    Car.Type = type;
}
private void setSpeed(GameObject Car, float speed){
    Car.speed = speed;
}
private void setType(GameObject Car, Color color){
    Car.color = color;
}

}