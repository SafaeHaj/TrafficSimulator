using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour{

    public int type;
    public long startTime;
    public List<float> maxestspeeds;
    public List<float> maxspeeds;
    public int nearbycars;

    private float rotationSpeed = 10f;
    public bool isBraking;
    public float speed = 0;
    private float _maxspeed = 0f;


    public float MaxSpeed {
        get {return _maxspeed;}
        set {
            if (value != _maxspeed){
                maxestspeeds.Add(value);
                maxspeeds.Add(0);
            }
            _maxspeed = value;
        }
    }

    public float Speed { 
        get {return speed;}
        set {
            float realval = System.Math.Min(value, MaxSpeed);
            speed = System.Math.Max(0, realval); 

            maxspeeds[maxestspeeds.Count - 1] = System.Math.Max(speed, maxspeeds[maxestspeeds.Count - 1]);
        }
    }

    private int _rotation_count;

    public Waypoints starting_point;
    public List<Waypoints> path = new List<Waypoints>(); 
    public int current = 0;

    void Start(){
        startTime = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        _rotation_count = 1;

        Datagraphing objectWithTag = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Datagraphing>();
        objectWithTag.collectCycle += OnDataCollectionCycle;
    }

    void Update(){
        Steer(path[current].transform.position);

        HandleObstacles();
        EvaluateSpeed();

        GoForward();
        
        if (Vector3.Distance(transform.position, path[current].transform.position) < 0.1f){
            current += 1;

            if (current >= path.Count){
                evaluateAnalytics();
                Destroy(transform.gameObject);
            }

            Speed = Speed * .9f;
        }
    }

    private void GoForward(){
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void Steer(Vector3 lookAt){

        Vector3 direction = (lookAt - transform.position).normalized;
        direction = new Vector3(direction.x, direction.y, 0);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle , Vector3.forward);

        if (_rotation_count <= 2){
            transform.rotation = targetRotation; 
            _rotation_count += 1;
        }
        else{
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * Speed);
        }
    }

    private void EvaluateSpeed(){
        if (isBraking)
            Speed -= Speed;
        else 
            Speed += .05f;
    }

    private void HandleObstacles(){
        int counter = 0;

        isBraking = false;
        List<RaycastHit2D>[] obstacles = CheckForObstacles();

        foreach (RaycastHit2D hit in obstacles[0]){
            if (hit.collider != null && hit.collider.gameObject != transform.gameObject){
                
                ItrafficProp trafficProp = hit.collider.gameObject.GetComponent<ItrafficProp>();
                if (trafficProp != null){
                    trafficProp.execute(this);
                }
                else{
                    bool isEq = utility.Vector3Equality(transform.right, hit.collider.gameObject.transform.right, 0.1f);
                    if ( !isEq ){
                        isBraking = true;
                    }
                    counter += 1;
                }

    
            }
        }

        foreach (RaycastHit2D hit in obstacles[1]){
            if (hit.collider != null && hit.collider.gameObject != transform.gameObject){
                //isBraking = true;

                ItrafficProp trafficProp = hit.collider.gameObject.GetComponent<ItrafficProp>();
                if (trafficProp != null){
                    trafficProp.execute(this);
                }
                else
                    counter += 1;
            }
        }

        foreach (RaycastHit2D hit in obstacles[2]){
            if (hit.collider != null && hit.collider.gameObject != transform.gameObject){

                ItrafficProp trafficProp = hit.collider.gameObject.GetComponent<ItrafficProp>();
                if (trafficProp != null){
                    trafficProp.execute(this);
                }
                else{
                    isBraking = true;
                    counter += 1;
                }
            }
        }

        nearbycars = counter;

    }


    private List<RaycastHit2D>[] CheckForObstacles(){
        int angle = 2;
        int rayNumber = (int)(90 / angle);
        
        List<RaycastHit2D> hitsRight = new List<RaycastHit2D>(), hitsLeft = new List<RaycastHit2D>(), hitsfront = new List<RaycastHit2D>();
        List<RaycastHit2D>[] obstacles = new List<RaycastHit2D>[3];
        
        Vector3 origin = transform.position + transform.right * (GetComponent<BoxCollider2D>().size.x/2 * transform.localScale.x + 0.01f);


        Vector3 direction = transform.right;
        RaycastHit2D hit = Physics2D.Raycast(origin ,direction.normalized , Conts.RAYCAST_DISTANCE * (1 + (int) Speed) );
        hitsfront.Add(hit);
        obstacles[2] = hitsfront;

        for (int rayNum = 1; rayNum <= rayNumber / 2; rayNum++) {
            float real_angle = angle * rayNum * Mathf.Deg2Rad;
            direction = transform.right * Mathf.Cos(real_angle) + transform.up * Mathf.Sin(real_angle);

            hit = Physics2D.Raycast(origin ,direction.normalized , Conts.RAYCAST_DISTANCE);
            hitsRight.Add(hit);
        }
        obstacles[1] = hitsRight;

        for (int rayNum = 1; rayNum <= rayNumber / 2; rayNum++) {
            float real_angle = ((float) System.Math.PI/2 - angle * rayNum ) * Mathf.Deg2Rad;
            direction = transform.right * Mathf.Cos(real_angle) + transform.up * Mathf.Sin(real_angle);

            hit = Physics2D.Raycast(origin ,direction.normalized , Conts.RAYCAST_DISTANCE);
            hitsLeft.Add(hit);
        }
        obstacles[0] = hitsLeft;

        return obstacles;
    }


    public void OnDataCollectionCycle(){
        Datagraphing objectWithTag = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Datagraphing>();
        int i = maxestspeeds.Count - 1;
        objectWithTag.collectData(type, (maxspeeds[i] / maxestspeeds[i]) , nearbycars);
    }
    public void evaluateAnalytics(){
        Datagraphing objectWithTag = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Datagraphing>();
        objectWithTag.AddTypeValues(type, (float) (System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() - startTime));
        objectWithTag.collectCycle -= OnDataCollectionCycle;
    }


    void OnDrawGizmos(){
        DrawRaycasts();
    }

    private void DrawRaycasts()
    {
        int angle = 2;
        int rayNumber = (int)(90 / angle);
  
        Vector3 origin = transform.position + transform.right * (GetComponent<BoxCollider2D>().size.x/2 * transform.localScale.x + 0.01f);
        Vector3 direction = transform.right;
        Gizmos.color = Color.black;
        DrawCast(origin , origin +  direction.normalized * Conts.RAYCAST_DISTANCE * (1 + (int) Speed));

        for (int rayNum = 1; rayNum <= rayNumber / 2; rayNum++) {
            float real_angle = angle * rayNum * Mathf.Deg2Rad;
            direction = transform.right * Mathf.Cos(real_angle) + transform.up * Mathf.Sin(real_angle);
            Gizmos.color = Color.blue;
            DrawCast(origin , origin +  direction.normalized * Conts.RAYCAST_DISTANCE);
        }

        for (int rayNum = 1; rayNum <= rayNumber / 2; rayNum++) {
            float real_angle = ((float) System.Math.PI/2 - angle * rayNum ) * Mathf.Deg2Rad;
            direction = transform.right * Mathf.Cos(real_angle) + transform.up * Mathf.Sin(real_angle);

            Gizmos.color = Color.red;
            DrawCast(origin , origin +  direction.normalized * Conts.RAYCAST_DISTANCE);
        }
    }

    private void DrawCast(Vector3 origin, Vector3 dest)
    {
        Gizmos.DrawLine(origin, dest);
    }

}

 
        