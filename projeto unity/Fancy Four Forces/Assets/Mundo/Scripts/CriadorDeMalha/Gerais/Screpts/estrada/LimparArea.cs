using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using teste;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ageral
{
    public class LimparArea : MonoBehaviour
    {/*
        public List<Vector3> ListaVertices = new List<Vector3>();
        public GerenciadorFloresta grf;
        public triangulador trl;
        public float TamanhoParaRemover;

        public void adicionarVertice()
        {
            ListaVertices.Add(transform.position);
        }
        public Mesh mesh;
        public void GerarMalha()
        {
            criarMalha(trl.triangular(ListaVertices));
        }
        public bool renderMalha;
        public void criarMalha(List<Vector3> vertices)
        {
            mesh = new Mesh();

            mesh.vertices = vertices.ToArray();

            int[] triangles = new int[vertices.Count];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = i;
            }

     
            mesh.triangles = triangles;

         
            mesh.RecalculateBounds();


         

            // Atribuir a malha ao componente MeshFilter do GameObject
            
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;
            if (tempCollider == null)
            {
                tempCollider = gameObject.AddComponent<MeshCollider>();
            }
            tempCollider.sharedMesh = meshFilter.sharedMesh;
           

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
        public void removerArvores()
        {
            // interar todas as arvores
            // pegar a distancia mais procima da malha na arvore
            //se for pequena remover a arvore
        
            grf.arvores_g.RemoveAll(x => x == null);
            foreach (GameObject a in grf.arvores_g)
            {
                if (ArovreNaMalha(a))
                {
                    DestroyImmediate(a);
                }

            }
            DestroyImmediate(tempCollider);


        }
       public  MeshFilter meshFilter;
        public MeshCollider tempCollider;
        bool ArovreNaMalha(GameObject arvore)
        {
            bool aux = false;
          
            for(int x= 0; x < ordemTrianguo.Count / 3; x++)
            {
                if (trl.PontoDentroTriangulo(arvore.transform.position, ordemTrianguo[x * 3], ordemTrianguo[(x * 3)+1], ordemTrianguo[(x*3) + 2]))
                {
                    aux = true;
                    break;
                }
            }
            return aux;

        }
        [HideInInspector]
        public bool AtivadoDesativadoGuizmos;

        public List<Vector3> ordemTrianguo = new List<Vector3>();
        public void criarMalhaParaRemover(List<Vector3> lista)
        {
            ordemTrianguo.Clear();
            for (int x = 0; x < lista.Count - 1; x++)
            {

                Vector3 orientacao = Vector3.Cross(lista[x + 1] - lista[x], Vector3.up);
                orientacao.Normalize();
                // Vector3 pontoMeioA = lista[x];
                Vector3 pontoMeioA = lista[x];
                Vector3 pontoMeioB = lista[x + 1];



                // Vector3 pontoA = pontoMeioA - (orientacao ) ;
                Vector3 pontoA_E = x == 0 ? pontoMeioA - (orientacao * TamanhoParaRemover) : ordemTrianguo[ordemTrianguo.Count - 1];
                Vector3 pontoB_E = pontoMeioB - (orientacao * TamanhoParaRemover);
                Vector3 pontoA_D = x == 0 ? pontoMeioA + (orientacao * TamanhoParaRemover) : ordemTrianguo[ordemTrianguo.Count - 3];
                Vector3 pontoB_D = pontoMeioB + (orientacao * TamanhoParaRemover);

                ordemTrianguo.Add(pontoA_E);
                ordemTrianguo.Add(pontoB_E);
                ordemTrianguo.Add(pontoA_D);

                ordemTrianguo.Add(pontoB_D);
                ordemTrianguo.Add(pontoA_D);
                ordemTrianguo.Add(pontoB_E);


            }
            ordemTrianguo.Reverse();

            for (int x = 0; x < ordemTrianguo.Count; x++)
            {
                ordemTrianguo[x] -= transform.position;
            }
            criarMalha(ordemTrianguo);

        }
        public void limparMalha()
        {
            mesh = new Mesh();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter = new MeshFilter();
        }
        public void TestarERemover(GameObject P)
        {
            // aresta AB para P  !=  aresta AC para P
           /*
            for(int x = 0; x < ordemTrianguo.Count /3; x++)
            {
                if (P == null)
                    break;
                Vector3 A = ordemTrianguo[x * 3] + transform.position;
                Vector3 B = ordemTrianguo[(x * 3 ) +1] + transform.position;
                Vector3 C = ordemTrianguo[(x * 3 ) +2] + transform.position;
             

                float MaxDist = Vector3.Distance(A, C);
                MaxDist = MaxDist < Vector3.Distance(A, B) ? Vector3.Distance(A, B) : MaxDist;
                if (Vector3.Distance(A, P.transform.position) < MaxDist)
                {
                    float ab = ValoresUniversais.Orientacao3(A, B, P.transform.position);
                    float ac = ValoresUniversais.Orientacao3(A, C, P.transform.position);
                    if (ab == ac || ac == 0)
                        continue;
                    DestroyImmediate(P);
                 
                }


            }
          

          
        }

    }
    [CustomEditor(typeof(LimparArea))]
    public class EditorLimparArea : Editor {

        public override void OnInspectorGUI()
        {
            LimparArea meuScript = (LimparArea)target;
            base.OnInspectorGUI();


            if (GUILayout.Button("Adicionar Vertice"))
            {

                meuScript.adicionarVertice();
            }
            if (GUILayout.Button("Gerar malha"))
            {

                meuScript.GerarMalha();
            }
            if (GUILayout.Button("Remover arvores"))
            {

                meuScript.removerArvores();
            }

            if (GUILayout.Button("Ativar  /  desativar guizomos"))
            {

                meuScript.AtivadoDesativadoGuizmos = !meuScript.AtivadoDesativadoGuizmos;
            }

            }

        private void OnSceneGUI()
        {
            LimparArea meuScript = (LimparArea)target;
            if (meuScript.AtivadoDesativadoGuizmos) { 
            for (int x = 0; x < meuScript.ListaVertices.Count; x++)
            {
                meuScript.ListaVertices[x] = Handles.PositionHandle(meuScript.ListaVertices[x], Quaternion.identity);
                Handles.color = Color.red;
                Handles.DrawSolidDisc(meuScript.ListaVertices[x], Vector3.up, 1);

                    if (x < meuScript.ListaVertices.Count - 1)
                    {
                        float dist = Vector3.Distance(meuScript.ListaVertices[x], meuScript.ListaVertices[x + 1]);

                        Handles.ArrowHandleCap(0, meuScript.ListaVertices[x],
                           Quaternion.LookRotation(meuScript.ListaVertices[x + 1] - meuScript.ListaVertices[x]), dist, EventType.Repaint);
                    }
                }
            }
            for(int x= 0; x < meuScript.ordemTrianguo.Count/3; x++)
            {
                Handles.color = Color.red;
                Handles.DrawSolidDisc(meuScript.ordemTrianguo[x * 3] + meuScript.transform.position, Vector3.up, 1);
                Handles.color = Color.white;
                Handles.DrawSolidDisc(meuScript.ordemTrianguo[(x*3) +1] + meuScript.transform.position, Vector3.up, 1);

                Handles.color = Color.blue;
                Handles.DrawLine(meuScript.ordemTrianguo[x * 3] + meuScript.transform.position, meuScript.ordemTrianguo[(x *3) +1] + meuScript.transform.position);
            }
        }


        }
        */
    }
}
