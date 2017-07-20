using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMeshDeformer : MonoBehaviour {

	Mesh mesh;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3[] vertices = mesh.vertices;
		int i = 0;
		while (i < vertices.Length) {
			vertices [i].z += Mathf.Sin(vertices[i].x + Time.time*10) * Time.deltaTime * 5;
			i++;
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds ();
	}
}
