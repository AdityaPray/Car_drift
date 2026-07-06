// WaypointCircuit.cs
using UnityEngine;

public class WaypointCircuit : MonoBehaviour
{
    public Transform[] waypoints;

    public Transform GetWaypoint(int index)
    {
        return waypoints[index % waypoints.Length];
    }

    public int TotalWaypoints => waypoints.Length;

    // Visualisasi di editor
    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform a = waypoints[i];
            Transform b = waypoints[(i + 1) % waypoints.Length];
            if (a != null && b != null)
                Gizmos.DrawLine(a.position, b.position);
        }
    }
}