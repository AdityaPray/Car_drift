// Assets/Editor/GuardrailPainter.cs
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GuardrailPainter : EditorWindow
{
    // ── Referensi & Data ───────────────────────────
    GameObject        guardrailPrefab;
    List<Vector3>     points      = new List<Vector3>();
    bool              paintMode   = false;

    // ── Pengaturan ─────────────────────────────────
    float offsetSamping   = 4f;
    float jarakPerSegmen  = 3f;
    float tinggiOffset    = 0.05f;
    bool  pasangKiri      = true;
    bool  pasangKanan     = true;
    bool  loopJalur       = false;
    string namaParent     = "Guardrails";

    // ── Tampilan ───────────────────────────────────
    Vector2 scrollPos;
    Color   warnaCenterLine = Color.yellow;
    float   radiusTitik     = 0.4f;

    // ───────────────────────────────────────────────
    [MenuItem("Tools/Guardrail Painter")]
    public static void ShowWindow()
    {
        GetWindow<GuardrailPainter>("Guardrail Painter");
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        paintMode = false;
    }

    // ───────────────────────────────────────────────
    // SCENE VIEW — tangkap klik mouse
    // ───────────────────────────────────────────────
    void OnSceneGUI(SceneView sceneView)
    {
        if (!paintMode) return;

        // Paksa fokus ke SceneView agar event tertangkap
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = Event.current;

        // Klik kiri = tambah titik
        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                Vector3 titikBaru = hit.point + Vector3.up * tinggiOffset;
                points.Add(titikBaru);
                e.Use(); // konsumsi event agar tidak select objek lain
                Repaint();
            }
        }

        // Klik kanan = hapus titik terakhir (undo satu titik)
        if (e.type == EventType.MouseDown && e.button == 1 && !e.alt)
        {
            if (points.Count > 0)
            {
                points.RemoveAt(points.Count - 1);
                e.Use();
                Repaint();
            }
        }

        DrawPreview(sceneView);
    }

    // ───────────────────────────────────────────────
    // Gambar preview garis & titik di Scene View
    // ───────────────────────────────────────────────
    void DrawPreview(SceneView sceneView)
    {
        if (points.Count == 0) return;

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;

        // Gambar titik tengah
        for (int i = 0; i < points.Count; i++)
        {
            // Titik pusat (kuning)
            Handles.color = warnaCenterLine;
            Handles.SphereHandleCap(0, points[i], Quaternion.identity,
                radiusTitik, EventType.Repaint);

            // Label nomor
            Handles.Label(points[i] + Vector3.up * 0.8f,
                $"  {i}", EditorStyles.boldLabel);
        }

        // Gambar garis tengah
        if (points.Count > 1)
        {
            Handles.color = warnaCenterLine;
            for (int i = 0; i < points.Count - 1; i++)
                Handles.DrawLine(points[i], points[i + 1], 2f);

            if (loopJalur)
                Handles.DrawLine(points[points.Count - 1], points[0], 2f);
        }

        // Preview guardrail kiri & kanan (garis tipis)
        if (points.Count > 1)
        {
            int segCount = loopJalur ? points.Count : points.Count - 1;

            for (int i = 0; i < segCount; i++)
            {
                Vector3 posA = points[i];
                Vector3 posB = points[(i + 1) % points.Count];
                Vector3 dir  = (posB - posA).normalized;
                Vector3 kanan = Vector3.Cross(Vector3.up, dir).normalized;

                Vector3 kiriA  = posA - kanan * offsetSamping;
                Vector3 kiriB  = posB - kanan * offsetSamping;
                Vector3 kananA = posA + kanan * offsetSamping;
                Vector3 kananB = posB + kanan * offsetSamping;

                if (pasangKiri)
                {
                    Handles.color = Color.blue;
                    Handles.DrawLine(kiriA, kiriB, 2f);
                }
                if (pasangKanan)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(kananA, kananB, 2f);
                }
            }
        }

        sceneView.Repaint();
    }

    // ───────────────────────────────────────────────
    // GUI WINDOW
    // ───────────────────────────────────────────────
    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("=== Guardrail Painter ===", EditorStyles.boldLabel);
        EditorGUILayout.Space(4);

        // ── Prefab ──
        guardrailPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Guardrail Prefab", guardrailPrefab, typeof(GameObject), false);

        EditorGUILayout.Space(6);

        // ── Tombol Paint Mode ──
        GUI.backgroundColor = paintMode
            ? new Color(0.3f, 1f, 0.4f)   // hijau terang saat aktif
            : new Color(0.7f, 0.7f, 0.7f); // abu saat nonaktif

        string labelPaint = paintMode
            ? "🖌️  MODE MELUKIS: AKTIF  (klik di Scene)"
            : "🖌️  Aktifkan Mode Melukis";

        if (GUILayout.Button(labelPaint, GUILayout.Height(36)))
        {
            paintMode = !paintMode;
            if (paintMode)
            {
                // Fokus ke SceneView saat aktifkan paint mode
                SceneView.lastActiveSceneView?.Focus();
                Debug.Log("[Guardrail] Paint mode ON — Klik kiri: tambah titik | Klik kanan: hapus titik terakhir");
            }
        }
        GUI.backgroundColor = Color.white;

        // Instruksi
        if (paintMode)
        {
            EditorGUILayout.HelpBox(
                "🖱️ Klik KIRI di Scene View → tambah titik\n" +
                "🖱️ Klik KANAN → hapus titik terakhir\n" +
                $"📍 Total titik: {points.Count}",
                MessageType.Info);
        }

        EditorGUILayout.Space(8);

        // ── Pengaturan ──
        GUILayout.Label("Pengaturan", EditorStyles.boldLabel);
        offsetSamping  = EditorGUILayout.Slider("Offset Samping (m)", offsetSamping, 0.5f, 20f);
        jarakPerSegmen = EditorGUILayout.Slider("Jarak Per Guardrail (m)", jarakPerSegmen, 0.5f, 10f);
        tinggiOffset   = EditorGUILayout.FloatField("Offset Tinggi (m)", tinggiOffset);
        pasangKiri     = EditorGUILayout.Toggle("Pasang Kiri (biru)", pasangKiri);
        pasangKanan    = EditorGUILayout.Toggle("Pasang Kanan (merah)", pasangKanan);
        loopJalur      = EditorGUILayout.Toggle("Loop (sambung akhir→awal)", loopJalur);
        namaParent     = EditorGUILayout.TextField("Nama Parent", namaParent);

        EditorGUILayout.Space(10);

        // ── Info titik ──
        EditorGUILayout.LabelField($"Total Titik: {points.Count}",
            EditorStyles.boldLabel);

        EditorGUILayout.Space(4);

        // ── Tombol aksi ──
        GUI.backgroundColor = (guardrailPrefab != null && points.Count >= 2)
            ? Color.green : Color.gray;
        if (GUILayout.Button("▶  PASANG GUARDRAIL", GUILayout.Height(38)))
            PasangGuardrail();

        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("✕  Hapus Semua Titik", GUILayout.Height(26)))
        {
            if (EditorUtility.DisplayDialog("Konfirmasi",
                "Hapus semua titik yang sudah digambar?", "Ya", "Batal"))
                points.Clear();
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("🗑  Hapus Guardrail di Scene", GUILayout.Height(26)))
            HapusGuardrail();

        GUI.backgroundColor = Color.white;
        EditorGUILayout.Space(8);
        EditorGUILayout.EndScrollView();
    }

    // ───────────────────────────────────────────────
    void PasangGuardrail()
    {
        if (guardrailPrefab == null)
        { Debug.LogError("[Guardrail] Prefab belum diisi!"); return; }

        if (points.Count < 2)
        { Debug.LogError("[Guardrail] Minimal 2 titik diperlukan!"); return; }

        HapusGuardrail();

        GameObject parent = new GameObject(namaParent);
        Undo.RegisterCreatedObjectUndo(parent, "Create Guardrails");

        int total = 0;
        int segCount = loopJalur ? points.Count : points.Count - 1;

        for (int i = 0; i < segCount; i++)
        {
            Vector3 posA   = points[i];
            Vector3 posB   = points[(i + 1) % points.Count];
            Vector3 dir    = posB - posA;
            float   panjang = dir.magnitude;
            if (panjang < 0.01f) continue;

            Vector3    dirNorm = dir.normalized;
            Vector3    kanan   = Vector3.Cross(Vector3.up, dirNorm).normalized;
            Quaternion rot     = Quaternion.LookRotation(dirNorm);

            int jumlah = Mathf.Max(1, Mathf.FloorToInt(panjang / jarakPerSegmen));

            for (int j = 0; j < jumlah; j++)
            {
                float   t   = (float)j / jumlah;
                Vector3 pos = Vector3.Lerp(posA, posB, t);

                if (pasangKiri)  { SpawnOne(pos - kanan * offsetSamping, rot, parent.transform, "L"); total++; }
                if (pasangKanan) { SpawnOne(pos + kanan * offsetSamping, rot, parent.transform, "R"); total++; }
            }
        }

        paintMode = false;
        Debug.Log($"[Guardrail] Selesai! {total} guardrail dipasang.");
        Selection.activeGameObject = parent;
    }

    void SpawnOne(Vector3 pos, Quaternion rot, Transform parent, string sisi)
    {
        // Snap ke permukaan
        if (Physics.Raycast(pos + Vector3.up * 15f, Vector3.down, out RaycastHit hit, 40f))
            pos = hit.point + Vector3.up * tinggiOffset;

        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(guardrailPrefab);
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.transform.SetParent(parent);
        obj.name = $"GR_{sisi}_{parent.childCount}";
        Undo.RegisterCreatedObjectUndo(obj, "Place Guardrail");
    }

    void HapusGuardrail()
    {
        GameObject lama = GameObject.Find(namaParent);
        if (lama != null)
        {
            Undo.DestroyObjectImmediate(lama);
            Debug.Log("[Guardrail] Guardrail dihapus.");
        }
    }
}