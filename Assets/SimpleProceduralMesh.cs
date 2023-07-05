using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleProceduralMesh : MonoBehaviour
{
	public int xSize = 20;
	public int zSize = 20;

	public Gradient gradient;


	Vector3[] vertices;
	int [] triangles;
	Mesh mesh;
	Color[] colours;
	float minTerrainHeight;
	float maxTerrainHeight;
	

	void Start()
	{
		mesh = new Mesh { name = "Procedural Mesh" };

		CreateShape();

		UpdateMesh();
	
		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;

	}

	void CreateShape()
    {
		//Create vertices
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];

		for (int i = 0, z = 0; z <= zSize; z++)
        {
			for (int x = 0; x <= xSize; x++)
            {
				float frequency1 = 0.0f;
				float frequency2 = 0.2f;
				float frequency3 = 0.3f;
				float amplitude1 = 0.4f;
				float amplitude2 = 0.7f;
				float amplitude3 = 0.8f;

				float noiseStrength = 2.0f;


				float y =
					  amplitude1 * Mathf.PerlinNoise(x * frequency1, z * frequency1)
					+ amplitude2 * Mathf.PerlinNoise(x * frequency2, z * frequency2)
					+ amplitude3 * Mathf.PerlinNoise(x * frequency3, z * frequency3)
						* noiseStrength;
				vertices[i] = new Vector3(x, y, z);

				if(y > maxTerrainHeight)
                {
					maxTerrainHeight = y;
                }
				if(y < minTerrainHeight)
                {
					minTerrainHeight = y;
                }

				i++;
            }
        }

		triangles = new int[xSize * zSize * 6];

		int vert = 0;
		int tris = 0;

		for(int z = 0; z < zSize; z++)
        {
			for (int x = 0; x < xSize; x++)
			{
				triangles[tris + 0] = vert + 0;
				triangles[tris + 1] = vert + xSize + 1;
				triangles[tris + 2] = vert + 1;
				triangles[tris + 3] = vert + 1;
				triangles[tris + 4] = vert + xSize + 1;
				triangles[tris + 5] = vert + xSize + 2;

				vert++;
				tris += 6;
			}
			vert++;
		}

		colours = new Color[vertices.Length];
		for (int i = 0, z = 0; z <= zSize; z++)
		{
			for (int x = 0; x <= xSize; x++)
			{
				float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
				colours[i] = gradient.Evaluate(height);
				i++;
			}
		}

	}

	void UpdateMesh()
    {
		mesh.Clear();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.colors = colours;

		mesh.RecalculateNormals();

	
	}


}
