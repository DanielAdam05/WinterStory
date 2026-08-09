using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ForestScatter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Terrain terrain;
    [SerializeField] private Texture2D densityMask;
    [SerializeField] private GameObject[] treePrefabs;

    [Header("Sampling")]
    [SerializeField] private float cellSize = 4f;
    [Tooltip("0 = no jitter (grid), 1 = full cell jitter (very random).")]
    [Range(0f, 1f)]
    [SerializeField] private float jitterAmmount = 0.8f;
    [Range(0f, 1f)]
    [SerializeField] private float maxDensity = 0.7f;

    [Header("Constraints")]
    [SerializeField] private float maxSlopeAngle = 35f;
    [SerializeField] private LayerMask groundLayer = ~0;

    [Header("Randomization")]
    [SerializeField] private Vector3 baseRotationEuler = new Vector3(-90f, -90f, 0f);
    [SerializeField] private Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    [SerializeField] private bool randomYRotation = true;

    [Header("Output")]
    [SerializeField] private bool clearExistingBeforeBake = true;
    [SerializeField] private Transform outputParent;

    private List<GameObject> spawned = new();

    public void Bake()
    {
        if (terrain == null || densityMask == null || treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogError("ForestScatter: assign a valid terrain, mask and tree prefab(s)");
            return;
        }

        if (clearExistingBeforeBake)
            ClearSpawned();

        Transform spawnParent = outputParent != null ? outputParent : this.transform;
        TerrainData data = terrain.terrainData;
        Vector3 terrainPos =  terrain.transform.position;
        float width = data.size.x;
        float length = data.size.z;

        for(float x = 0; x < width; x += cellSize)
        {
            for(float z = 0; z < length; z += cellSize)
            {
                // Select a random point in the grid WITH the jitter
                float jx = x + Random.Range(-cellSize, cellSize) * 0.5f * jitterAmmount;
                float jz = z + Random.Range(-cellSize, cellSize) * 0.5f * jitterAmmount;
                jx = Mathf.Clamp(jx, 0, width);
                jz = Mathf.Clamp(jz, 0, length);

                float worldX = terrainPos.x + jx;
                float worldZ = terrainPos.z + jz;

                // Sample mask texture
                float u = jx / width;
                float v = jz / length;
                float maskValue = densityMask.GetPixelBilinear(u, v).grayscale;

                // Roll whether to spawn at this point or not judging by MAX DENSITY
                float probability = maskValue * maxDensity;
                if (Random.value > probability)
                    continue; // skip this

                // ------ After the point is selected, spawn: -------
                Vector3 rayOrigin = new Vector3(worldX, terrainPos.y + data.size.y + 50f, worldZ);
                if(!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, /* max distance: */ data.size.y + 200f, groundLayer))
                    continue;

                float slope = Vector3.Angle(hit.normal, Vector3.up);
                if (slope > maxSlopeAngle) 
                    continue; //skip

                GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

                // start from the fixed base rotation then randomize
                Quaternion baseRotation = Quaternion.Euler(baseRotationEuler);
                Quaternion randomYaw = randomYRotation ? Quaternion.Euler(0f, Random.Range(0f, 360f), 0f) : Quaternion.identity;
                Quaternion finalRotation = randomYaw * baseRotation;

                GameObject instance = Instantiate(prefab, /*pos*/ hit.point, finalRotation, spawnParent);

                float scale = Random.Range(scaleRange.x, scaleRange.y);
                instance.transform.localScale *= scale;

                spawned.Add(instance);
            }
        }

        Debug.Log($"ForestScatter: Spawned {spawned.Count} trees");

    }

    public void ClearSpawned()
    {
        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            if (spawned[i] != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(spawned[i]);
                else
                    Destroy(spawned[i]);
#else
                Destroy(spawned[i]);
#endif
            }
        }
        spawned.Clear();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ForestScatter))]
public class ForestScatterToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ForestScatter forestScatter = (ForestScatter)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Bake Forest", GUILayout.Height(30)))
        {
            forestScatter.Bake();
        }
        if (GUILayout.Button("Clear Spawned Trees"))
        {
            forestScatter.ClearSpawned();
        }
    }
}
#endif