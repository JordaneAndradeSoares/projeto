using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ManipulacaoDeMalha {


  public static Vector3 GetRandomPositionInMesh(Mesh mesh)
    {

        int randomTriangleIndex = Random.Range(0, mesh.triangles.Length / 3);
        int startIndex = randomTriangleIndex * 3;

        Vector3 vertexA = mesh.vertices[mesh.triangles[startIndex]];
        Vector3 vertexB = mesh.vertices[mesh.triangles[startIndex + 1]];
        Vector3 vertexC = mesh.vertices[mesh.triangles[startIndex + 2]];

        float u = Random.Range(0f, 1f);
        float v = Random.Range(0f, 1f);

        if (u + v > 1f)
        {
            u = 1f - u;
            v = 1f - v;
        }

        Vector3 randomPosition = vertexA + u * (vertexB - vertexA) + v * (vertexC - vertexA);


        return randomPosition;

    }


}
