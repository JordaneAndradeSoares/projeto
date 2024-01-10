using System.Collections;
using System.Collections.Generic;
using teste;
using UnityEditor;
using UnityEngine;

namespace Ageral
{
    public class CriadorDeAreas1 : MonoBehaviour
    {

        public List<Vector3> PontosFloresta = new List<Vector3>();
        [HideInInspector]
        public List<Vector3> ListaVertices = new List<Vector3>();

     
        public triangulador trl;
        public Mesh mesh;
        [HideInInspector]
        public bool renderMalha,concavo;
        public void AdicionarVertice()
        {
            Vector2 aux = new Vector3();
            aux = transform.position;
            PontosFloresta.Add(aux);

        }
        public void removerVertice()
        {
            PontosFloresta.RemoveAt(PontosFloresta.Count - 1);
        }



        MeshCollider colisorFloresta;
        public void criarMalha(List<Vector3> vertices)
        {
            mesh = new Mesh();

            // Atribuir os vértices à malha
            mesh.vertices = vertices.ToArray();

            // Definir os triângulos (assumindo que os vértices estão em grupos de três para formar triângulos)
            int[] triangles = new int[vertices.Count];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = i;
            }

            // Atribuir os triângulos à malha
            mesh.triangles = triangles;

            // Recalcular normais e bounds (opcional, mas geralmente desejável)
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Atribuir a malha ao componente MeshFilter do GameObject
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;



            if (colisorFloresta == null)
            {
                colisorFloresta = gameObject.AddComponent<MeshCollider>();
            }
            colisorFloresta.sharedMesh = meshFilter.sharedMesh;
            // Atribuir um material (pode ajustar conforme necessário)
            if (renderMalha)
            {
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }


        }
        public void triangular()
        {
            ListaVertices = trl.triangular(PontosFloresta);
        }


        public void percorrerMalha(Mesh mesh_)
        {
           
            float valorMax = CalcularAreaDaMesh(mesh) ;
            for (int x = 0; x < mesh.triangles.Length / 3; x++)
            {


                int randomTriangleIndex = mesh.triangles[x];
                int startIndex = randomTriangleIndex * 3;

                Vector3 vertexA = mesh.vertices[mesh.triangles[startIndex]];
                Vector3 vertexB = mesh.vertices[mesh.triangles[startIndex + 1]];
                Vector3 vertexC = mesh.vertices[mesh.triangles[startIndex + 2]];

                for (float z = 0; z < (int)valorMax; z++)
                {


                    float u = Random.Range(0f, 1f);
                    float v = Random.Range(0f, 1f);

                    if (u + v > 1f)
                    {
                        u = 1f - u;
                        v = 1f - v;
                    }

                    Vector3 randomPosition = vertexA + u * (vertexB - vertexA) + v * (vertexC - vertexA);
                     }
               
            }
        }
   


        [HideInInspector]
        public bool ativar_desativar;
  
        private void Start()
        {
            if (trl == null)
            {
                trl = gameObject.AddComponent<triangulador>();
            }
        }
        float CalcularAreaDaMesh(Mesh mesh)
        {
            int[] triangles = mesh.triangles;
            Vector3[] vertices = mesh.vertices;

            float areaTotal = 0f;

            // Loop através dos triângulos
            for (int i = 0; i < triangles.Length; i += 3)
            {
                // Obtém os vértices do triângulo
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];

                // Calcula a área do triângulo usando a fórmula de Heron
                float a = Vector3.Distance(v0, v1);
                float b = Vector3.Distance(v1, v2);
                float c = Vector3.Distance(v2, v0);
                float s = (a + b + c) / 2f;
                float areaTriangulo = Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));

                // Adiciona a área do triângulo à área total
                areaTotal += areaTriangulo;
            }

            return areaTotal;
        }

       
        public void calcularMalha()
        {
            ListaVertices.Clear();
            
            for(int x = 0; x < PontosFloresta.Count - 2; x++)
            {
                int v1 = x;
                int v2 = (x) + 1;
                int v3 = (x) + 2;

                ListaVertices.Add(PontosFloresta[v1] );
                ListaVertices.Add(PontosFloresta[v2] );
                ListaVertices.Add(PontosFloresta[v3] );
            }
        }

    }

    [CustomEditor(typeof(CriadorDeAreas1))]
    public class EditorCriadorDeAreas1 : Editor
    {

        SerializedProperty frequencia, amplitude, escalaDensidade, ResistenciaDasArvores, ListaDeArvores_data;
        void OnEnable()
        {
            frequencia = serializedObject.FindProperty("frequencia");
            amplitude = serializedObject.FindProperty("amplitude");
            escalaDensidade = serializedObject.FindProperty("escalaDensidade");
            ResistenciaDasArvores = serializedObject.FindProperty("ResistenciaDasArvores");
            ListaDeArvores_data = serializedObject.FindProperty("ListaDeArvores_data");



            CriadorDeAreas1 meuScript = (CriadorDeAreas1)target;
            if (meuScript.trl == null)
            {
                meuScript.trl = meuScript.gameObject.AddComponent<triangulador>();
            }
        }
        public override void OnInspectorGUI()
        {



            serializedObject.Update();
          

                CriadorDeAreas1 meuScript = (CriadorDeAreas1)target;
            if (GUILayout.Button(" ativar / desativa   guizmo"))
            {
                meuScript.ativar_desativar = !meuScript.ativar_desativar;
                serializedObject.ApplyModifiedProperties();
                Repaint();
            }
            if (GUILayout.Button(meuScript.concavo ? "Concavo" : "Convexo"))
            {
                meuScript.concavo = !meuScript.concavo;
            }


            if (GUILayout.Button("Adicionar Vertice"))
            {

                meuScript.AdicionarVertice();
            }
            if (GUILayout.Button("remover Vertice"))
            {

                meuScript.removerVertice();
            }

           
               


                if (GUILayout.Button("Calcular area ?"))
                {

                    calcularArea_();
                }
          



           
            serializedObject.ApplyModifiedProperties();

        }
        private void OnSceneGUI()
        {
            CriadorDeAreas1 meuScript = (CriadorDeAreas1)target;

            if (meuScript.ativar_desativar)
            {
                for (int x = 0; x < meuScript.PontosFloresta.Count; x++)
                {
                    meuScript.PontosFloresta[x] = Handles.PositionHandle(meuScript.PontosFloresta[x] + meuScript.transform.position, Quaternion.identity) - meuScript.transform.position;
                }
                if (meuScript.PontosFloresta.Count > 3)
                {
                    try
                    {
                        Handles.color = Color.red;

                        Handles.DrawLine(meuScript.PontosFloresta[meuScript.PontosFloresta.Count-1] + meuScript.transform.position, meuScript.PontosFloresta[meuScript.PontosFloresta.Count - 2] + meuScript.transform.position);

                        Handles.DrawLine(meuScript.PontosFloresta[meuScript.PontosFloresta.Count-1] + meuScript.transform.position, meuScript.PontosFloresta[meuScript.PontosFloresta.Count - 3] + meuScript.transform.position);
                    }
                    catch
                    {
                        Debug.Log(meuScript.PontosFloresta.Count);
                    }
                }
            }
        }
        public void calcularArea_()
        {
            CriadorDeAreas1 meuScript = (CriadorDeAreas1)target;

            if (meuScript.concavo)
            {
                meuScript.triangular();
              
            }
            else
            {
                meuScript.calcularMalha();
            
            }
            meuScript.criarMalha(meuScript.ListaVertices);
        }





    }
}
