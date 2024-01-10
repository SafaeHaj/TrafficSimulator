using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{   
    private long _timer;
    
    public List<Waypoints> StartPoints;
    public List<Waypoints> ExitPoints;
    public List<Sprite> ListColors;
    public GameObject carsFolder;
    public GameObject VehiclePrefab;


    void Update(){
        
        if (System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() - _timer <= Conts.SPAWN_COOLDOWN)
            return; 
        _timer = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        SpawCarAtLocation(GetRandomPoint(), VehiclePrefab);

    }

    private void SpawCarAtLocation(Waypoints SpawnPoint, GameObject Car){
        GameObject SpawnedCar = Instantiate(Car, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        SpawnedCar.transform.SetParent(carsFolder.transform);

        Vehicle CarComp = SpawnedCar.GetComponent<Vehicle>();
        CarComp.MaxSpeed = 1f;
        
        CarComp.starting_point = SpawnPoint;
        int color = GeneratePath(CarComp);

        SpawnedCar.GetComponent<SpriteRenderer>().sprite = ListColors[color];
        CarComp.type = color;
    }

    private Waypoints GetRandomPoint(){
        int index = Random.Range(0, StartPoints.Count);
        return StartPoints[index];
    }

    private int GenerateRandomPath(Vehicle car){
        car.path.Add(car.starting_point);
        Waypoints cur = car.starting_point;

        while (cur.connections.Count != 0){
            int index = Random.Range(0, cur.connections.Count);
            cur = cur.connections[index];
            car.path.Add(cur);
        }

        return ExitPoints.IndexOf(cur);
    }


    public int GeneratePath(Vehicle car)
    {
        GenerateRandomPath(car);
        OptimizePath(car);
        return ExitPoints.IndexOf(car.path.Last());
    }


    private void OptimizePath(Vehicle car)
    {
        // A* algorithm to optimize the path
        Dictionary<Waypoints, Waypoints> cameFrom = new Dictionary<Waypoints, Waypoints>();
        Dictionary<Waypoints, float> gCost = new Dictionary<Waypoints, float> { { car.starting_point, 0f } };
        List<Waypoints> openSet = new List<Waypoints> { car.starting_point };

        while (openSet.Count > 0)
        {
            Waypoints current = openSet.OrderBy(node => gCost[node] + Heuristic(node, car.path.LastOrDefault())).First();
            openSet.Remove(current);

            if (current == car.path.Last())
            {
                // Reconstruct the optimized path from the backtrace
                ReconstructOptimizedPath(car, cameFrom, current);
                break;
            }

            foreach (Waypoints neighbor in current.connections)
            {
                float tentativeGCost = gCost[current] + CostToMove(current, neighbor);

                if (!gCost.ContainsKey(neighbor) || tentativeGCost < gCost[neighbor])
                {
                    gCost[neighbor] = tentativeGCost;
                    cameFrom[neighbor] = current;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    private void ReconstructOptimizedPath(Vehicle car, Dictionary<Waypoints, Waypoints> cameFrom, Waypoints current)
    {
        List<Waypoints> optimizedPath = new List<Waypoints>();
        while (cameFrom.ContainsKey(current))
        {
            optimizedPath.Add(current);
            current = cameFrom[current];
        }
        optimizedPath.Reverse(); // Reverse to get the correct order from start to target
        car.path.Clear();
        car.path.AddRange(optimizedPath);
    }


    private float Heuristic(Waypoints node, Waypoints goal)
    {
        return Vector3.Distance(node.transform.position, goal.transform.position);
    }

    private float CostToMove(Waypoints from, Waypoints to)
    {
        return Vector3.Distance(from.transform.position, to.transform.position);
    }

    
}
