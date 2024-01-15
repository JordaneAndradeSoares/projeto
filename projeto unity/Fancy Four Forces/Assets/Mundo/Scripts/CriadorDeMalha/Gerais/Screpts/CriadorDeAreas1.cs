using PlasticGui.WorkspaceWindow.Home;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using teste;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Ageral
{
    public class CriadorDeAreas1 : MonoBehaviour
    {

        public List<Vector3> PontosFloresta;
        [HideInInspector]
        public List<Vector3> ListaVertices = new List<Vector3>();
      




        public triangulador trl;
        public Mesh mesh ;
        [HideInInspector]
        public bool renderMalha,concavo;




        MeshCollider colisorFloresta;
        public void criarMalha(List<Vector3> vertices)
        {
            mesh = GetComponent<Mesh>();
            if(mesh == null)
            {
                mesh = new Mesh();
            }

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


            colisorFloresta = GetComponent<MeshCollider>();
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


        public Vector3 PontoAleatorio()
        {
            // Armazena os índices dos pontos para evitar buscas repetidas
            int indexA = Random.Range(0, PontosFloresta.Count);
            int indexB;
            int indexC;

            do
            {
                indexB = Random.Range(0, PontosFloresta.Count);
            } while (indexB == indexA);

            do
            {
                indexC = Random.Range(0, PontosFloresta.Count);
            } while (indexC == indexB || indexC == indexA);

            Vector3 a = PontosFloresta[indexA];
            Vector3 b = PontosFloresta[indexB];
            Vector3 c = PontosFloresta[indexC];

            return Vector3.Lerp(Vector3.Lerp(a, b, Random.Range(0f, 1f)), c, Random.Range(0f, 1f));
        }




        [HideInInspector]
        public bool ativar_desativar;
  
        private void Start()
        {
            if (trl == null)
            {
                trl = gameObject.AddComponent<triangulador>();
            }

            if(mesh == null)
            {
                mesh = GetComponent<MeshFilter>().mesh;
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

                ListaVertices.Add(PontosFloresta[v1] - transform.position);
                ListaVertices.Add(PontosFloresta[v2] - transform.position);
                ListaVertices.Add(PontosFloresta[v3] - transform.position);
            }
        }

    }

    [CustomEditor(typeof(CriadorDeAreas1))]
    public class EditorCriadorDeAreas1 : Editor
    {
        public SerializedProperty PontosFloresta, ListaVertices,PontosFloresta_vizu, mesh;
          void OnEnable()
        {
            mesh = serializedObject.FindProperty("mesh");
            PontosFloresta = serializedObject.FindProperty("PontosFloresta");
            ListaVertices = serializedObject.FindProperty("ListaVertices");

            CriadorDeAreas1 meuScript = (CriadorDeAreas1)target;
            if (meuScript.trl == null)
            {
             meuScript.trl = meuScript.gameObject.AddComponent<triangulador>();
            }
         
        }
     
        public override void OnInspectorGUI()
        {
         //   base.OnInspectorGUI();
       
            serializedObject.Update();
            EditorGUILayout.PropertyField(PontosFloresta, new GUIContent("Pontos adicionados"));
            EditorGUILayout.PropertyField(ListaVertices, new GUIContent("ListaVertices"));
            EditorGUILayout.PropertyField(mesh, new GUIContent("mesh"));
            CriadorDeAreas1 meuScript = (CriadorDeAreas1)target;
            EditorGUI.BeginChangeCheck();
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
                    meuScript.PontosFloresta[x] = Handles.PositionHandle(meuScript.PontosFloresta[x], Quaternion.identity) ;
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
