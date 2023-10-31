using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicule : MonoBehaviour
{
    private float speed;
    private string type;
    private float weight;
    private int horsePower;
    private float noiseLevel;
    private bool isBraking;
    private float BrankingForce;
    private float raycastDistance;

    void start(){
        
    }

    void update(){
    }

    private void GoForward(){
        transform.Translate(speed*Time.deltaTime*Vector3.forward)
    }
    private void Steer(){
        float angle = Mathf.Atan2(lookAt.y - transform.position.y, lookAt.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private float EvaluateSpeed(Vector3 previousPosition, Vector3 currentPosition , float timeDelta){
        float distanceBetween = Vector3.Distance(currentPosition , previousPosition);
        float speed = distanceBetween/timeDelta;
        return speed;
    }
    private void Brake(BrankingIntensity){
        speed -= BrankingForce * BrankingIntensity;
        speed = Mathf.Max(0,speed);
    }
    private bool FindNextTarget(){
        Vector3 raycastDirection = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, raycastDirection, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Obstacle")) // Check the obstacle tag or layer
            {
                float distanceToObstacle = hit.Distance;
                return distanceToObstacle;
            }
            return NULL;
        }
        return NULL;
    }
    public bool CheckForObstacles(LayerMask Obstacle){
        if(Physics.Raycast(origin,direction,raycastDistance,Obstacle)){
            return True;
        }
        return False;
    }
    public void ApplyTrafficRules(){
        // We need Rules hhh
    }
}

 
        