using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<Waypoints> connections;

    void OnDrawGizmos()
    {
        if (connections == null || connections.Count == 0)
        {
            DrawRedRectangle();
        }
        else
        {
            DrawConnections();
        }

        DrawObjectNameGizmo();
    }

    private void DrawConnections()
    {
        Gizmos.color = Color.green;

        foreach (var connection in connections)
        {
            if (connection != null)
            {
                Gizmos.DrawLine(transform.position, connection.transform.position);
            }
        }
    }

    private void DrawRedRectangle()
    {
        Gizmos.color = Color.red;

        // Draw a red rectangle on top of the waypoint
        float size = 0.1f;
        Gizmos.DrawCube(transform.position + Vector3.up * size, new Vector3(size, size, size));
    }

    private void DrawObjectNameGizmo()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        position.y += .1f;
        string numericPart = GetNumericPart(gameObject.name);
        UnityEditor.Handles.Label(position, numericPart);
    }

    private string GetNumericPart(string input)
    {
        string numericPart = "";
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                numericPart += c;
            }
        }
        return numericPart;
    }
}
