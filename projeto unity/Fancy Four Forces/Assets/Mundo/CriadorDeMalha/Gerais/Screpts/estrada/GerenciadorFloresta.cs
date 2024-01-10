using Ageral;
using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace teste
{
    public class GerenciadorFloresta : MonoBehaviour
    {

        public List<Vector3> PontosFloresta = new List<Vector3>();
        [HideInInspector]
        public List<Vector3> ListaVertices = new List<Vector3>();

     //   public List<GameObject> vertices_GO = new List<GameObject>();

        public triangulador trl;
        public Mesh mesh;
        [HideInInspector]
        public bool renderMalha;
        [Range(0, 1)]
        public float ResistenciaDasArvores;
        [Range(0.001f,0.02f)]
        public float escalaDensidade;
        [Tooltip("zoom, quanto menor maior o zoom")]
        [Range(0,10)]
        public float amplitude;
        [Tooltip("quanto maior o numero mais definido sera os conjuntos")]
        [Range(0.001f,1000)]
        public float  frequencia;
        public List<GameObject> ListaDeArvores_data = new List<GameObject>();
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
            listaDeArvores.Clear();
            float valorMax = CalcularAreaDaMesh(mesh) * escalaDensidade;
            for (int x =0; x < mesh.triangles.Length/3; x++)
            {


                int randomTriangleIndex = mesh.triangles[x];
            int startIndex = randomTriangleIndex * 3;

            Vector3 vertexA = mesh.vertices[mesh.triangles[startIndex]];
            Vector3 vertexB = mesh.vertices[mesh.triangles[startIndex + 1]];
            Vector3 vertexC = mesh.vertices[mesh.triangles[startIndex + 2]];
                
                   for(float z = 0;z < (int)valorMax; z++){
          

                float u = Random.Range(0f, 1f);
                float v = Random.Range(0f, 1f);

                if (u + v > 1f)
            {
                u = 1f - u;
                v = 1f - v;
            }

            Vector3 randomPosition = vertexA + u * (vertexB - vertexA) + v * (vertexC - vertexA);
                        listaDeArvores.Add(new metaArvore(ValoresUniversais.calcularPerlingNoise(randomPosition.x,randomPosition.z,amplitude,frequencia), randomPosition));
                    }
           //     }
            }
        }
        public List<metaArvore> listaDeArvores = new List<metaArvore>();

        [HideInInspector]
        public List<GameObject> arvores_g = new List<GameObject>();


        public void instanciarArvores()
        {
            arvores_g.ForEach(x => DestroyImmediate(x.gameObject));
            arvores_g.Clear();


            int valor = 0;
            if (listaDeArvores.Count > 0)
            {
                foreach (var a in listaDeArvores)
                {
                    if (a.valor > ResistenciaDasArvores)
                    {
                      

                        GameObject aux_ = ListaDeArvores_data[valor];


                        GameObject aux = Instantiate(aux_, a.local + transform.position, Quaternion.Euler(Vector3.up *Random.Range(-90,90)), transform);
                        arvores_g.Add(aux);
                        valor = valor + 1 <= ListaDeArvores_data.Count-1 ? valor+1:0;
                    }
                }
            }
        }
        [HideInInspector]
        public bool ativar_desativar;
        [System.Serializable]
        public class metaArvore {
            public float valor;
            public Vector3 local;
            public GameObject obj;

            public metaArvore(float valor, Vector3 local)
            {
                this.valor = valor;
                this.local = local;
                //this.obj = obj;
            }
        }
        private void Start()
        {
            if(trl == null)
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

  
   
      

    }
    [CustomEditor(typeof(GerenciadorFloresta))]
    public class EditorGerenciadorFloresta : Editor
    {

        SerializedProperty frequencia, amplitude, escalaDensidade, ResistenciaDasArvores, ListaDeArvores_data;
        void OnEnable()
        {
            frequencia = serializedObject.FindProperty("frequencia");
            amplitude = serializedObject.FindProperty("amplitude");
            escalaDensidade = serializedObject.FindProperty("escalaDensidade");
            ResistenciaDasArvores = serializedObject.FindProperty("ResistenciaDasArvores");
            ListaDeArvores_data = serializedObject.FindProperty("ListaDeArvores_data");



            GerenciadorFloresta meuScript = (GerenciadorFloresta)target;
            if(meuScript.trl == null)
            {
                meuScript.trl = meuScript.gameObject.AddComponent<triangulador>();
            }
        }
        public override void OnInspectorGUI()
        {



            serializedObject.Update();

            
                  EditorGUILayout.PropertyField(ListaDeArvores_data, new GUIContent("Modelos de arvores"));
            GerenciadorFloresta meuScript = (GerenciadorFloresta)target;
            if (GUILayout.Button(" ativar / desativa   guizmo"))
            {
                meuScript.ativar_desativar = !meuScript.ativar_desativar;
                serializedObject.ApplyModifiedProperties();
                Repaint();
            }
          
        
            EditorGUILayout.PropertyField(frequencia, new GUIContent("Frquencia das arvores"));
            EditorGUILayout.PropertyField(amplitude, new GUIContent("ampliude das arvores"));
            EditorGUILayout.PropertyField(escalaDensidade, new GUIContent("escala De densidade das arvores"));


            EditorGUILayout.PropertyField(ResistenciaDasArvores, new GUIContent("Resistencia das arvores"));
            if (GUILayout.Button(" criar arvores"))
            {
                meuScript.percorrerMalha(meuScript.mesh);
            }

            if (GUILayout.Button("Adicionar Vertice"))
            {
             
                meuScript.AdicionarVertice();
            }
            if (GUILayout.Button("Adicionar Vertice"))
            {

                meuScript.removerVertice();
            }
            if (GUILayout.Button("Calcular area ?"))
            {
              
               calcularArea_();
            }
          

           
            if (GUILayout.Button("instanciar modelos de arvores"))
            {
                meuScript.instanciarArvores();
            }
            serializedObject.ApplyModifiedProperties();

        }
        private void OnSceneGUI()
        {
            GerenciadorFloresta meuScript = (GerenciadorFloresta)target;
            
            if (meuScript.ativar_desativar)
            {
                for(int x = 0; x <  meuScript.PontosFloresta.Count; x++)
                {
                    meuScript.PontosFloresta[x] = Handles.PositionHandle(meuScript.PontosFloresta[x], Quaternion.identity);
                    Handles.color = Color.red;
                    Handles.DrawSolidDisc(meuScript.PontosFloresta[x], Vector3.up,1);
                }
                if(meuScript.listaDeArvores.Count > 0)
                {
                    foreach (var a in meuScript.listaDeArvores)
                    {
                        if (a.valor > meuScript.ResistenciaDasArvores)
                        {
                            Handles.color = Color.blue;
                            Handles.DrawSolidDisc(a.local + meuScript.transform.position, Vector3.up, 1.5f);
                        }
                    }
                }
            }
        }
        public void calcularArea_()
        {
       GerenciadorFloresta meuScript = (GerenciadorFloresta)target;
          
            meuScript.triangular();
            meuScript.criarMalha(meuScript.ListaVertices); 

        }




     
    

    }
}
