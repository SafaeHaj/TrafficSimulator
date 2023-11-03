using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Transactions;
using System.Threading.Tasks.Dataflow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVehicle : MonoBehaviour
{
    private float speed;
    private float acceleration;
    private bool isBraking;
    private float BrakingForce;

    void start()
    {

    }

    void update()
    {
    }

    private void GoForward()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
    private void Steer()
    {
        float angle = Mathf.Atan2(lookAt.y - transform.position.y, lookAt.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private float EvaluateSpeed(Vector3 previousPosition, Vector3 currentPosition, float timeDelta)
    {
        float distanceBetween = Vector3.Distance(currentPosition, previousPosition);
        float speed = distanceBetween / timeDelta;
        return speed;
    }
    private void Brake(int brakingIntensity)
    {
        speed -= BrakingForce * BrakingIntensity;
        speed = Mathf.Max(0, speed);
    }
    private bool FindNextTarget();
    private List<RayCast2D>[] CheckForObstacles()
    {
        int angle = 5;
        int rayNumber = (int)(90 / angle);
        
        List<RaycastHit2D> hitsRight = new List<RaycastHit2D>(), hitsLeft = new List<RaycastHit2D>();
        List<RaycastHit2D>[] obstacles = new List<RaycastHit2D>[2];

        for (int rayNum = 0; rayNum <= rayNumber / 2; rayNum++) {
            currentDirection = Math.Cos(angle * rayNum);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward * currentDirection, RAYCAST_DISTANCE);
            hitsRight.Add(hit);
        }
        obstacles[0] = hitsRight;

        for (int rayNum = 1; rayNum <= rayNumber / 2; rayNum++) {
            currentDirection = Math.Cos(Math.PI/2 - angle * rayNum);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward * currentDirection, RAYCAST_DISTANCE);
            hitsLeft.Add(hit);
        }
        obstacles[1] = hitsLeft;

        return obstacles;
    }
    private void ApplyTrafficRules()
    {
        // We need Rules hhh
    }
}

public class VehicleSpawner : Placeable {
    
}


