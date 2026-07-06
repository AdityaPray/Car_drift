// Assets/Editor/WaypointReorderer.cs
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaypointReorderer : EditorWindow
{
    GameObject container;

    [MenuItem("Tools/Waypoint Reorderer")]
    public static void ShowWindow()
        => GetWindow<WaypointReorderer>("WP Reorderer");

    void OnGUI()
    {
        GUILayout.Label("Urutkan Waypoint agar tidak menyilang",
            EditorStyles.boldLabel);
        EditorGUILayout.Space(6);

        container = (GameObject)EditorGUILayout.ObjectField(
            "Waypoint Container", container, typeof(GameObject), true);

        EditorGUILayout.HelpBox(
            "Pilih RCCP_AI_WaypointsContainer lalu klik Urutkan.",
            MessageType.Info);

        EditorGUILayout.Space(8);

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("▶ URUTKAN WAYPOINT (Nearest Neighbor)",
            GUILayout.Height(36)))
            UrutkanWaypoint();

        GUI.backgroundColor = Color.white;
    }

    void UrutkanWaypoint()
    {
        if (container == null)
        { Debug.LogError("Container belum diisi!"); return; }

        // Kumpulkan semua child
        List<Transform> wps = new List<Transform>();
        foreach (Transform t in container.transform)
            wps.Add(t);

        if (wps.Count < 2)
        { Debug.LogError("Minimal 2 waypoint!"); return; }

        // Nearest Neighbor sort mulai dari index 0
        List<Transform> sorted = new List<Transform>();
        List<Transform> sisa   = new List<Transform>(wps);

        Transform current = sisa[0];
        sorted.Add(current);
        sisa.RemoveAt(0);

        while (sisa.Count > 0)
        {
            float     minDist = float.MaxValue;
            Transform nearest = null;
            int       nearIdx = 0;

            for (int i = 0; i < sisa.Count; i++)
            {
                float d = Vector3.Distance(current.position, sisa[i].position);
                if (d < minDist) { minDist = d; nearest = sisa[i]; nearIdx = i; }
            }

            sorted.Add(nearest);
            sisa.RemoveAt(nearIdx);
            current = nearest;
        }

        // Terapkan urutan baru ke Hierarchy
        Undo.RegisterFullObjectHierarchyUndo(container, "Reorder Waypoints");
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].SetSiblingIndex(i);
            sorted[i].name = $"Waypoint {i}";
        }

        Debug.Log($"[WP Reorderer] {sorted.Count} waypoint diurutkan ulang!");
    }
}