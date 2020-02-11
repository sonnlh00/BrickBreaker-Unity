using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LetterboxCamera {

    /* TerrainSegment.cs
     *
     * Manages and constructs a segment of terrain
     *
     * Copyright Hexdragonal Games 2015
     * Written by Tom Elliott */

    public enum UVMappingMode {
        STRETCH_MATCH,
        TILING
    }

    public class TerrainSegment : MonoBehaviour {

        // Mesh/Collider Config
        [HideInInspector]
        public int numberOfPoints = 50;
        [HideInInspector]
        public float length = 27;
        [HideInInspector]
        public bool createEdge = false;
        [HideInInspector]
        public float baseDepthOfTerrain = -5f;
        [HideInInspector]
        public float maxHeight = 11f;
        [HideInInspector]
        public AnimationCurve terrainDescription = new AnimationCurve();
        [HideInInspector]
        public UVMappingMode uvMode;
        [HideInInspector]
        public int pixelsPerUnit = 32;
        [HideInInspector]
        public float trimOffset = 0.1f;
        [HideInInspector]
        public float unitScale = 25;

        private float curveLength = 0;
        private float verticeXspacing;
        private List<Vector3> peakVertices;
        private List<Vector3> originalPeakVertices;
        private Vector3[] vertices;
        private Vector2[] uvs;
        private int[] triangles;
        private Mesh mesh;
        private MeshRenderer meshRenderer;

        // The 'trim' is a texture that lines the top of the terrain
        private Mesh trimMesh;
        private MeshRenderer trimMeshRenderer;
        private Vector2[] trimUvs;
        private Vector3[] trimVertices;
        private string trimName = "TerrainTrim";

        private EdgeCollider2D edgeCollider;

        private void Awake() {
            Setup(numberOfPoints, true, 0);
            RecalculateTerrainFromCurve();
        }

        /// <summary>
        /// Sets the Vertices of the Terrain
        /// Calculates the UVs for that set of verts
        /// </summary>
        /// <param name="newVertices"></param>
        private void CalculateUVsAndVerts(List<Vector3> newVertices) {
            // Calculate the scale of the UVs based on the in world pixels per unit over the width and height of the Terrain texture (this is auto-scaled to local space)
            Vector2 uvScale = new Vector2(1, 1);
            if (meshRenderer.sharedMaterial != null && meshRenderer.sharedMaterial.mainTexture != null) {
                uvScale.x = ((float)pixelsPerUnit) / ((float)meshRenderer.sharedMaterial.mainTexture.width);
                uvScale.y = ((float)pixelsPerUnit) / ((float)meshRenderer.sharedMaterial.mainTexture.height);
            }

            if (uvMode == UVMappingMode.STRETCH_MATCH) {
                float percent = 0;
                for (int i = 0; i < newVertices.Count; i++) {
                    vertices[i] = newVertices[i];

                    if (newVertices[i].y == baseDepthOfTerrain) // Base of the terrain (second vert in sequence)
                    {
                        uvs[i] = new Vector2(percent, 0);
                    } else // Peak of the terrain (First vert in sequence)
                      {
                        // We set the percentage here as the peak vertex is the first in sequence, so it sets the percentage for the next vert too
                        percent = Util.Percent(0, newVertices.Count - 2, i);
                        uvs[i] = new Vector2(percent, 1);
                    }
                }
            } else if (uvMode == UVMappingMode.TILING) {
                if (meshRenderer.sharedMaterial != null && meshRenderer.sharedMaterial.mainTexture != null) {
                    uvScale.x = ((float)pixelsPerUnit) / ((float)meshRenderer.sharedMaterial.mainTexture.width);
                    uvScale.y = ((float)pixelsPerUnit) / ((float)meshRenderer.sharedMaterial.mainTexture.height);
                }
                for (int i = 0; i < newVertices.Count; i++) {
                    vertices[i] = newVertices[i];
                    uvs[i] = new Vector2(newVertices[i].x * uvScale.x, newVertices[i].y * uvScale.y);
                }
            }

            // Set the Verts and UVs for the Terrain trim serperatly as it needs to be tiled and meshed only at the top
            for (int i = 0; i < newVertices.Count; i++) {
                trimVertices[i] = newVertices[i];

                if (newVertices[i].y == baseDepthOfTerrain && i - 1 >= 0) // Base of the terrain (second vert in sequence)
                {
                    // Set the UV and Vert to the terrain peak - texture height
                    trimVertices[i].y = newVertices[i - 1].y - (1f / ((float)uvScale.y));
                    trimUvs[i] = new Vector2(newVertices[i].x * uvScale.x, 0);
                } else // Peak of the terrain (First vert in sequence)
                  {
                    trimUvs[i] = new Vector2(newVertices[i].x * uvScale.x, 1);
                }
            }
        }

        /// <summary>
        /// Creates a flat mesh and assigns it to the mesh filter on this Terrain
        /// Also sets up the 'trim' child object and it's mesh/filter/renderer
        /// </summary>
        /// <param name="numberOfPoints"></param>
        /// <param name="length"></param>
        /// <param name="createEdge"></param>
        /// <param name="startingY"></param>
        public void Setup(int numberOfPoints, bool createEdge, float startingY) {
            this.numberOfPoints = numberOfPoints;
            if (terrainDescription.keys.Length < 2) {
                terrainDescription.AddKey(0, 0);
                terrainDescription.AddKey(1, 0);
            }
            curveLength = terrainDescription.keys[terrainDescription.keys.Length - 1].time - terrainDescription.keys[0].time;
            this.length = curveLength * unitScale;
            this.createEdge = createEdge;
            edgeCollider = this.gameObject.GetComponent<EdgeCollider2D>();
            if (mesh != null) {
                DestroyImmediate(mesh);
            }
            mesh = new Mesh();
            mesh.name = "Terrain Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
            meshRenderer = this.GetComponent<MeshRenderer>();
            verticeXspacing = length / numberOfPoints;

            GameObject trimChild = null;
            Transform trimChildTransform = this.transform.Find(trimName);

            if (trimChildTransform == null) {
                trimChild = new GameObject(trimName);
                trimChild.transform.parent = this.transform;
                trimChild.transform.localPosition = Vector3.zero;
                trimChild.transform.localScale = new Vector3(1, 1, 1);
                trimChild.transform.localRotation = new Quaternion();

                trimChild.AddComponent<MeshFilter>();
                trimChild.AddComponent<MeshRenderer>();
            } else {
                trimChild = trimChildTransform.gameObject;
            }

            if (trimMesh != null) {
                DestroyImmediate(trimMesh);
            }
            trimMesh = new Mesh();
            trimMesh.name = "Terrain Trim Mesh";

            trimChild.GetComponent<MeshFilter>().mesh = trimMesh;
            trimMeshRenderer = trimChild.GetComponent<MeshRenderer>();

            GenerateLandSegment(startingY);
        }

        /// <summary>
        /// Recalculated the hills on this terrain from the pre-given Animation curve
        /// </summary>
        public void RecalculateTerrainFromCurve() {
            List<Vector3> newPeaks = new List<Vector3>();
            if (terrainDescription.keys.Length < 2) {
                terrainDescription.AddKey(0, 0);
                terrainDescription.AddKey(1, 0);
            }
            curveLength = terrainDescription.keys[terrainDescription.keys.Length - 1].time - terrainDescription.keys[0].time;
            length = curveLength * unitScale;

            for (int i = 0; i < numberOfPoints; i++) {
                float t = Util.Percent(0, numberOfPoints - 1, i);
                newPeaks.Add(new Vector3(length * t, terrainDescription.Evaluate(Util.Lerp(terrainDescription.keys[0].time, curveLength, t)) * maxHeight, 0));
            }

            BuildHills(newPeaks);
        }

        /// <summary>
        /// Builds hills into the terrain with a base depth and some peaks
        /// Peaks are based on local space
        /// </summary>
        /// <param name="peaks"></param>
        public void BuildHills(List<Vector3> peaks) {
            peakVertices = peaks;
            List<Vector3> lines = new List<Vector3>();
            for (int i = 0; i < peakVertices.Count; i++) {
                lines.Add(peakVertices[i]);
                lines.Add(new Vector3(peakVertices[i].x, baseDepthOfTerrain, 0));
            }

            vertices = new Vector3[lines.Count];
            uvs = new Vector2[lines.Count];
            trimVertices = new Vector3[lines.Count];
            trimUvs = new Vector2[lines.Count];

            CalculateUVsAndVerts(lines);

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            trimMesh.vertices = trimVertices;
            trimMesh.uv = trimUvs;
            trimMesh.triangles = triangles;
            trimMesh.RecalculateBounds();
            trimMesh.RecalculateNormals();
            trimMeshRenderer.transform.localPosition = new Vector3(0, trimOffset, -0.01f);

            if (createEdge) {
                edgeCollider.points = GetEdge();
            }
        }

        // Alters the current Mesh to a flat base
        public void GenerateLandSegment(float y) {
            peakVertices = new List<Vector3>();
            originalPeakVertices = new List<Vector3>();
            for (int i = 0; i < numberOfPoints; i++) {
                peakVertices.Add(new Vector3(i * verticeXspacing, y, 0));
                originalPeakVertices.Add(new Vector3(i * verticeXspacing, 1, 0));
            }

            List<Vector3> lines = new List<Vector3>();
            for (int i = 0; i < numberOfPoints; i++) {
                lines.Add(peakVertices[i]);
                lines.Add(new Vector3(peakVertices[i].x, -5, 0));
            }

            List<int> tris = new List<int>();
            for (int i = lines.Count - 1; i > 2; i -= 2) {
                tris.Add(i - 3);
                tris.Add(i - 1);
                tris.Add(i);
                tris.Add(i);
                tris.Add(i - 2);
                tris.Add(i - 3);
            }

            vertices = new Vector3[lines.Count];
            uvs = new Vector2[lines.Count];
            trimVertices = new Vector3[lines.Count];
            trimUvs = new Vector2[lines.Count];

            for (int i = 0; i < lines.Count; i++) {
                vertices[i] = lines[i];
                if (lines[i].y == 0) {
                    uvs[i] = new Vector2(lines[i].x, 0);
                } else
                    uvs[i] = new Vector2(lines[i].x, 1);

            }


            triangles = new int[tris.Count];
            for (int i = 0; i < tris.Count; i++)
                triangles[i] = tris[i];

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            if (createEdge)
                edgeCollider.points = GetEdge();
        }

        public Vector2[] GetEdge() {
            Vector2[] vertices = new Vector2[peakVertices.Count];
            for (int i = 0; i < peakVertices.Count; i++)
                vertices[i] = new Vector2(peakVertices[i].x, peakVertices[i].y);
            return vertices;
        }

        public Vector3 GetRightVertex() {
            return peakVertices[peakVertices.Count - 1];
        }

        public Mesh GetMesh() {
            return mesh;
        }

        public Material GetTrimMaterial() {
            if (trimMeshRenderer != null) {
                return trimMeshRenderer.sharedMaterial;
            } else {
                return null;
            }
        }

        public Material GetMaterial() {
            if (meshRenderer != null) {
                return meshRenderer.sharedMaterial;
            } else {
                return null;
            }
        }

        public void SetTrimMaterial(Material newMaterial) {
            if (trimMeshRenderer != null) {
                trimMeshRenderer.sharedMaterial = newMaterial;
            }
        }

        public void SetMaterial(Material newMaterial) {
            if (meshRenderer != null) {
                meshRenderer.sharedMaterial = newMaterial;
            }
        }
    }
}