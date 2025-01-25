using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemOnAnimatedMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private ParticleSystem particleSystem;
    private Mesh bakedMesh;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        bakedMesh = new Mesh();
    }

    void LateUpdate()
    {
        if (skinnedMeshRenderer == null || particleSystem == null)
            return;

        // Bake the current state of the skinned mesh into a static mesh
        skinnedMeshRenderer.BakeMesh(bakedMesh);

        // Assign the baked mesh to the particle system's shape
        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Mesh;
        shape.mesh = bakedMesh;
        shape.useMeshColors = true; // Optional: Use mesh colors for particles
    }
}
