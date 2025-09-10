using UnityEngine;
using UnityEditor;

public class TerrainToMesh : EditorWindow
{
    [MenuItem("Tools/Terrain/Export Terrain To Mesh")]
    public static void ExportTerrainToMesh()
    {
        Terrain terrain = Selection.activeGameObject?.GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("Please select a Terrain in the Hierarchy.");
            return;
        }

        TerrainData td = terrain.terrainData;
        int w = td.heightmapResolution;
        int h = td.heightmapResolution;
        Vector3 size = td.size;
        float[,] heights = td.GetHeights(0, 0, w, h);

        Vector3[] vertices = new Vector3[w * h];
        Vector2[] uvs = new Vector2[w * h];
        int[] triangles = new int[(w - 1) * (h - 1) * 6];

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float height = heights[y, x];
                vertices[y * w + x] = new Vector3(
                    ((float)x / (w - 1)) * size.x,
                    height * size.y,
                    ((float)y / (h - 1)) * size.z
                );
                uvs[y * w + x] = new Vector2((float)x / (w - 1), (float)y / (h - 1));
            }
        }

        int t = 0;
        for (int y = 0; y < h - 1; y++)
        {
            for (int x = 0; x < w - 1; x++)
            {
                int i = y * w + x;
                triangles[t++] = i;
                triangles[t++] = i + w;
                triangles[t++] = i + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + w;
                triangles[t++] = i + w + 1;
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Allow large meshes
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GameObject meshObj = new GameObject("Full Terrain Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        meshObj.GetComponent<MeshFilter>().mesh = mesh;
        meshObj.GetComponent<MeshRenderer>().material = terrain.materialTemplate;

        AssetDatabase.CreateAsset(mesh, "Assets/FullTerrainMesh.asset");
        AssetDatabase.SaveAssets();

        Debug.Log("Full terrain converted to mesh!");
    }
}
