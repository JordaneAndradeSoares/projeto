using System.Collections;
using System.Collections.Generic;
using teste;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace Ageral
{
    public class CriadorDeAreas : MonoBehaviour
    {


        public float tamanhoMaximoDeArea;
        [Range(0, 1)]
        public float resistencia;

        public float frequencia,Amplitude,escala;
     
        public triangulador trl;
     
        public bool AtivarGuizmos, SomentePontos, RenderizarMalha;

        [System.Serializable]
        public class pontos_c {
            public Vector3 local;
            public float valor;
            public pontos_c(Vector3 local , float valor)
            {
                this.local = local;
                this.valor = valor;
            }
        }

        public List<pontos_c> pontos_ = new List<pontos_c>();
        public List<Vector3> triangulos_ = new List<Vector3>();

        public Mesh mesh;
        public void CalcularArea_()
        {
            float posXinicial = (transform.position.x - tamanhoMaximoDeArea *escala/ 2) - transform.position.x;
            float posZinicial = (transform.position.z - tamanhoMaximoDeArea*escala / 2) - transform.position.z;
            pontos_.Clear();
            for (int x = 0; x < tamanhoMaximoDeArea; x++)
            {
                for (int y = 0; y < tamanhoMaximoDeArea; y++)
                {
                    float resultadoPN = ValoresUniversais.calcularPerlingNoise(posXinicial + x*escala, posZinicial + y* escala, Amplitude, frequencia);
                 
                        pontos_.Add(new pontos_c( new Vector3(posXinicial + x * escala, 0, posZinicial + y * escala),resultadoPN ));
                  
                }
            }  
                
        }
        public void CriarMalha()
        {
            triangulos_.Clear();

            foreach(pontos_c a in pontos_)
            {
                if (a.valor > resistencia)
                {
                    triangulos_.Add(a.local);
                }
            }
            
            
           triangulos_ = trl.triangular(triangulos_);
           CriarMalhaAPartirDeVertices(triangulos_);
        }
        void CriarMalhaAPartirDeVertices(List<Vector3> vertices)
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
            //   mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Atribuir a malha ao componente MeshFilter do GameObject
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;

            // Atribuir um material (pode ajustar conforme necessário)
           
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }
                meshRenderer.material = new Material(Shader.Find("Standard"));
           
        }

        

    }

    [CustomEditor(typeof(CriadorDeAreas))]
    public class EditorCriadorDeAreas : Editor
    {

        SerializedProperty tamanhoMaximoDeArea, frequencia, tolerancia, amplitude, escala;
        // SerializedProperty ultimoPontoZero;
        void OnEnable()
        {
            //   ultimoPontoZero = serializedObject.FindProperty("LPzero");
            tamanhoMaximoDeArea = serializedObject.FindProperty("tamanhoMaximoDeArea");
            frequencia = serializedObject.FindProperty("frequencia");
            tolerancia = serializedObject.FindProperty("resistencia");
            amplitude = serializedObject.FindProperty("Amplitude");
            escala = serializedObject.FindProperty("escala");

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CriadorDeAreas obj = (CriadorDeAreas)target;

            EditorGUILayout.PropertyField(frequencia, new GUIContent("Frquencia do perling noise"));
            EditorGUILayout.PropertyField(amplitude, new GUIContent("Amplitude do perling noise"));
            EditorGUILayout.PropertyField(tolerancia, new GUIContent("resistencia do perling noise"));

            EditorGUILayout.PropertyField(tamanhoMaximoDeArea, new GUIContent("Tamanho maximo da area"));
            EditorGUILayout.PropertyField(escala, new GUIContent("escala de cada ponto"));
            if (GUILayout.Button("Calcular Area"))
            {
                obj.CalcularArea_();
            }
            if (GUILayout.Button("Ativar Guizmos"))
            {
                obj.AtivarGuizmos = !obj.AtivarGuizmos;
            }
            if (GUILayout.Button("Somente pontos"))
            {
                obj.SomentePontos = !obj.SomentePontos;
            }
            if (GUILayout.Button("Renderizar Malha"))
            {
                obj.CriarMalha();
            }
            serializedObject.ApplyModifiedProperties();


        }
        private void OnSceneGUI()
        {

            CriadorDeAreas obj = (CriadorDeAreas)target;


            if (obj.AtivarGuizmos)
            {

                for (int x = 0; x < obj.pontos_.Count; x++)
                {
                   if(obj.pontos_[x].valor > obj.resistencia) { 

                        Handles.color = Color.red;
                        Handles.DrawSolidDisc(obj.pontos_[x].local + obj.transform.position, Vector3.up, obj.escala/4);
                      
                    
                    }
                }
            }
        }
        
    }
}

