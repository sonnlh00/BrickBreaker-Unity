using UnityEngine;
using UnityEditor;
using System.Collections;

namespace LetterboxCamera {

    /* TerrainSegmentEditor.cs
     *
     * An Editor script for TerrainSegment
     *
     * Originally made by Milo Keeble, source can be found on Github @ https://github.com/Ragepyro/EditorFileMaker */

    [CustomEditor(typeof(TerrainSegment))]
    public class TerrainSegmentEditor : Editor {

        public override void OnInspectorGUI() {
            TerrainSegment terrain = (TerrainSegment)target;
            DrawDefaultInspector();

            if (terrain.GetMesh() == null || terrain.GetComponent<EdgeCollider2D>() == null || terrain.GetComponent<MeshFilter>() == null || terrain.GetComponent<MeshRenderer>() == null) {
                SetupTerrainEditor(terrain);
            } else {
                RunTerrainEditor(terrain);
            }
        }

        /// <summary>
        /// Called if the terrain hasn't been setup yet or
        /// if the terrain GameObject is missing any components
        /// </summary>
        private void SetupTerrainEditor(TerrainSegment terrain) {
            if (terrain.gameObject.name == "GameObject") {
                terrain.gameObject.name = "Terrain";
            }

            if (terrain.GetComponent<EdgeCollider2D>() == null) {
                terrain.gameObject.AddComponent<EdgeCollider2D>();
            }
            if (terrain.GetComponent<MeshFilter>() == null) {
                terrain.gameObject.AddComponent<MeshFilter>();
            }
            if (terrain.GetComponent<MeshRenderer>() == null) {
                MeshRenderer newRenderer = terrain.gameObject.AddComponent<MeshRenderer>();
                newRenderer.receiveShadows = false;
                //newRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            terrain.Setup(terrain.numberOfPoints, terrain.createEdge, 0);
        }

        /// <summary>
        /// Allows the user to edit the length, depth and hill curves of the terrain
        /// </summary>
        private void RunTerrainEditor(TerrainSegment terrain) {
            // Terrain Curve
            terrain.terrainDescription = EditorGUILayout.CurveField("Terrain Description", terrain.terrainDescription);
            EditorGUILayout.Separator();

            // Terrain Size Parameters
            terrain.baseDepthOfTerrain = EditorGUILayout.FloatField("Terrain Depth", terrain.baseDepthOfTerrain);
            Vector2 terrainSize = EditorGUILayout.Vector2Field("Terrain Scaler", new Vector2(terrain.unitScale, terrain.maxHeight));
            terrain.unitScale = terrainSize.x;
            terrain.maxHeight = terrainSize.y;
            terrain.trimOffset = EditorGUILayout.FloatField("Trim Offset", terrain.trimOffset);
            EditorGUILayout.Separator();

            // Terrain UVs and vertices
            terrain.uvMode = (UVMappingMode)EditorGUILayout.EnumPopup("UV Mapping Mode", terrain.uvMode);
            if (terrain.uvMode == UVMappingMode.TILING) {
                terrain.pixelsPerUnit = EditorGUILayout.IntField("Pixels per Unit", terrain.pixelsPerUnit);
            }
            int originalVertexDensity = terrain.numberOfPoints;
            terrain.numberOfPoints = EditorGUILayout.IntField("Vertex Density", terrain.numberOfPoints);
            EditorGUILayout.Separator();

            // Terrain Materials
            terrain.SetMaterial((Material)EditorGUILayout.ObjectField("Terrain Material", terrain.GetMaterial(), typeof(Material), true));
            terrain.SetTrimMaterial((Material)EditorGUILayout.ObjectField("Trim Material", terrain.GetTrimMaterial(), typeof(Material), true));

            // Recalculate Terrain Mesh
            if (terrain.numberOfPoints != originalVertexDensity) {
                terrain.Setup(terrain.numberOfPoints, terrain.createEdge, 0);
            }
            terrain.RecalculateTerrainFromCurve();
        }
    }
}