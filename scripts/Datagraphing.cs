using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;

public class Datagraphing : MonoBehaviour
{
    private long _timer;

    public GameObject carsFolder;
    
    private struct DataPoint
    {
        public long time;
        public int value;
    }

    private struct TDataPoint
    {
        public int type;
        public float takentime;

    }

    private struct TimeDataPoint{
        public long time;
        public int type;
        public float avgspeedRation;
        public float nearbyDensity;
    }

    private List<DataPoint> dataPoints = new List<DataPoint>();
    private List<TDataPoint> TdataPoints = new List<TDataPoint>();
    private List<TimeDataPoint> TimedataPoints = new List<TimeDataPoint>();

    public event Action collectCycle;

    private string filePath  = "C:/Users/UM6P/Desktop/Learning/S3/Algorithmique foundations I/Project/SimulationData";

    void Update(){
        UpdateCarCount();
        if (Input.GetKeyDown(KeyCode.X)){
            Debug.Log("X key is pressed down!");
            WriteDataToFile();  
        }
    }

    void UpdateCarCount(){
        if (System.DateTimeOffset.UtcNow.ToUnixTimeSeconds() - _timer <= 60f)
            return; 
        _timer = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        collectCycle?.Invoke();

        DataPoint newDataPoint = new DataPoint
        {
            time = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            value = carsFolder.transform.childCount
        };

        dataPoints.Add(newDataPoint);
    }

    public void AddTypeValues(int type, float takentime){

        TDataPoint newDataPoint = new TDataPoint{
            type = type,
            takentime = takentime,
        };
        TdataPoints.Add(newDataPoint);
    }

    public void collectData(int type, float avgspeedRation, float nearbyDensity){

        TimeDataPoint newDataPoint = new TimeDataPoint{
            time = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            type = type,
            avgspeedRation = avgspeedRation,
            nearbyDensity = nearbyDensity
        };
        TimedataPoints.Add(newDataPoint);

    }

    void WriteDataToFile(){
        CultureInfo culture = CultureInfo.InvariantCulture;
        using (StreamWriter writer = new StreamWriter(filePath + "/carspermin.csv"))
        {
            writer.WriteLine("Time,Car");
            foreach (DataPoint dataPoint in dataPoints)
            {
                writer.WriteLine($"{dataPoint.time},{dataPoint.value}");
            }

        }

        using (StreamWriter writer = new StreamWriter(filePath + "/carsData.csv"))
        {
            writer.WriteLine("type,takentime");
            foreach (TDataPoint dataPoint in TdataPoints)
            {
                writer.WriteLine($"{dataPoint.type},{dataPoint.takentime}");
            }
        }

        using (StreamWriter writer = new StreamWriter(filePath + "/takenTimeData.csv"))
        {
            writer.WriteLine("time,type,avgspeedRatio,nearbyDensity");
            foreach (TimeDataPoint dataPoint in TimedataPoints)
            {
                writer.WriteLine($"{dataPoint.time}, {dataPoint.type}, {dataPoint.avgspeedRation.ToString("F2", culture)}, {dataPoint.nearbyDensity.ToString("F2", culture)}");
            }
        }

    }
}
