using Codice.Client.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ageral
{
    public class GerenciadorDeArea : MonoBehaviour
    {
        
        public List<gerenciadorDeEstradas> Ruas = new List<gerenciadorDeEstradas>();
        public List<Vector3> PontosEmVolta, todosPontosEmVolta,pontosInternos;
        gerenciadorDeEstradas RuaAlvo;
        public bool GuizmosAoRedor,GuizmosInterno,guizmosLinhasCaminho;
        public void verificarSeEstaFechado()
        {    
            RuaAlvo = Ruas[0].entrada.ConecatacoComEste;

            if (Ruas[0].saida.ConecatacoComEste == RuaAlvo)
            {

            }
            else
            {
                List<unidadeAlit> tempBL = new List<unidadeAlit>();
                tempBL.Add(new unidadeAlit(Ruas[0]));
                aaaaaaaaa = aliterar( new unidadeAlit(Ruas[0].saida.ConecatacoComEste), tempBL);
            }


            aaaaaaaaa[0].intercessaoA = RuaAlvo;
            aaaaaaaaa[0].indiceIntercessaoB =
                RuaAlvo.entrada.ConecatacoComEste == Ruas[0] ? 0 :
                RuaAlvo.saida.ConecatacoComEste == Ruas[0] ? RuaAlvo.pontosAuxiliares.Count - 1 :
                RuaAlvo.conecxoes.Exists(x => x.ConecatacoComEste == Ruas[0]) ?
                RuaAlvo.conecxoes.Find(x => x.ConecatacoComEste == Ruas[0]).meuIndiceEmQueEstaCOnectado :
                0;
            aaaaaaaaa.Add(new unidadeAlit(Ruas[0], Ruas[0].entrada.ConecatacoComEste, 0, Ruas[0].saida.ConecatacoComEste, Ruas[0].pontosAuxiliares.Count - 1
                ));

            Debug.Log(aaaaaaaaa.Count);

        }
        public List<unidadeAlit> aliterar(unidadeAlit atua , List<unidadeAlit> blkL)
        {
            blkL.Add(atua);
          if(atua.zero.entrada.ConecatacoComEste== RuaAlvo)
            {

                List<unidadeAlit> tempf = new List<unidadeAlit>();

                tempf.Add(new unidadeAlit(RuaAlvo, atua.zero.entrada.INdiceDeleEmQueEstaConectadoEmMim, atua.zero));
                tempf.Add(new unidadeAlit(atua.zero, atua.zero.entrada.ConecatacoComEste, atua.zero.entrada.meuIndiceEmQueEstaCOnectado));

                
              
                return tempf;
            }else
            if (atua.zero.saida.ConecatacoComEste == RuaAlvo)
            {
                List<unidadeAlit> tempf = new List<unidadeAlit>();

                tempf.Add(new unidadeAlit(RuaAlvo, atua.zero.saida.INdiceDeleEmQueEstaConectadoEmMim, atua.zero));
                tempf.Add(new unidadeAlit(atua.zero, atua.zero.saida.ConecatacoComEste, atua.zero.saida.meuIndiceEmQueEstaCOnectado));


                return tempf;
            }
            else
            if (atua.zero.conecxoes.Exists(x=>x.ConecatacoComEste == RuaAlvo)) 
            {
                List<unidadeAlit> tempf = new List<unidadeAlit>();
                var aa = atua.zero.conecxoes.Find(x=>x.ConecatacoComEste ==RuaAlvo);

                tempf.Add(new unidadeAlit(RuaAlvo, aa.INdiceDeleEmQueEstaConectadoEmMim, aa.ConecatacoComEste));
                tempf.Add(new unidadeAlit(atua.zero, aa.ConecatacoComEste, aa.meuIndiceEmQueEstaCOnectado));


                return tempf;
            }
            foreach(var a in atua.zero.conecxoes)
            {
                if(blkL.Exists(x=>x.zero == a.ConecatacoComEste))
                {
                    continue;
                }
                unidadeAlit b = new unidadeAlit(a.ConecatacoComEste);
                List<unidadeAlit> tempL = aliterar(b,blkL);

                if(tempL.Count > 0)
                {
                    tempL[tempL.Count - 1].intercessaoA = atua.zero;
                    tempL[tempL.Count - 1].indiceIntercessaoA = a.INdiceDeleEmQueEstaConectadoEmMim;
                    tempL.Add(new unidadeAlit(atua.zero,a.ConecatacoComEste,a.meuIndiceEmQueEstaCOnectado));

                    return tempL;
                }
            }

            return null;
        }
        public class unidadeAlit
        {
            public gerenciadorDeEstradas zero,intercessaoA,intercessaoB;
            // a onde zero esta conectando com as intercessoes
            public int indiceIntercessaoA,indiceIntercessaoB;
            public unidadeAlit (gerenciadorDeEstradas z )
            {
                zero = z;
            }
            public unidadeAlit(gerenciadorDeEstradas z,gerenciadorDeEstradas b, int b_i)
            {
                zero = z;
              
                intercessaoB = b;
                indiceIntercessaoB = b_i;
            }
            public unidadeAlit(gerenciadorDeEstradas z, int a_i, gerenciadorDeEstradas a)
            {
                zero = z;

                intercessaoA = a;
                indiceIntercessaoA = a_i;
            }
            public unidadeAlit(gerenciadorDeEstradas z , gerenciadorDeEstradas a , int a_i , gerenciadorDeEstradas b ,  int b_i)
            {
                zero = z;
                intercessaoA = a;
                indiceIntercessaoA = a_i;
                intercessaoB = b;
                indiceIntercessaoB = b_i;
            }


        }
        [SerializeField]
        public List<unidadeAlit> aaaaaaaaa  ;

        public int quantidadeEmVolta;
        public void calcularAoReddor()
        {
            todosPontosEmVolta = new List<Vector3>();
         
            foreach(var a in aaaaaaaaa)
            {
              for(int x = a.indiceIntercessaoA; x != a.indiceIntercessaoB; x  = a.indiceIntercessaoA < a.indiceIntercessaoB ?
                    x+1:x-1)
                {
                    todosPontosEmVolta.Add(a.zero.pontosAuxiliares[x]);
                }
            }
            PontosEmVolta = new List<Vector3>();
            for(int x =0; x < quantidadeEmVolta; x++)
            {
                Vector3 tempv = todosPontosEmVolta[Random.Range(0, todosPontosEmVolta.Count - 1)];
                PontosEmVolta.Add(tempv);
                todosPontosEmVolta.Remove(tempv);
            }
            pontosExternos.Clear();
            foreach(var a in PontosEmVolta)
            {
                pontosExternos.Add(new pontos_(a));
            }
            

        }

        public int quantidadeDePontos;
        public float distanciaDeCorte;
        public void adicionarPontosInternos()
        {

            //criar os pontos
            Vector3 meio = Vector3.zero;

            foreach(var a in todosPontosEmVolta)
            {
                meio += a;
            }
            meio /= todosPontosEmVolta.Count;
            pontosInternos.Clear();
            while (pontosInternos.Count < quantidadeDePontos) {
                int pontoI = Random.Range(0, todosPontosEmVolta.Count);
                float forca = Random.Range(0.1f, 0.75f);

                pontosInternos.Add(Vector3.Lerp(todosPontosEmVolta[pontoI], meio, forca));
            }

            List<Vector3> tempL = new List<Vector3>();
            foreach(var a in pontosInternos)
            {
                foreach(var b in pontosInternos)
                {
                    if (a == b)
                        continue;
                    float dist = Vector3.Distance(a, b);

                    if(dist < distanciaDeCorte)
                    {
                        tempL.Add(a);
                        break;
                    }
                }
            }
            pontosINternos.Clear();
          
            Debug.Log("vai remover : " +  tempL.Count + "   de  " + pontosInternos.Count);
            pontosInternos.RemoveAll(x=> tempL.Contains(x));
            Debug.Log("sobraram  " + pontosInternos.Count);

            foreach (var a in pontosInternos)
            {
                pontosINternos.Add(new pontos_(a));
            }
            pontosinternosBKP.Clear();
            pontosinternosBKP.AddRange( pontosINternos);

        }

        [System.Serializable]
        public class pontos_
        {
            public Vector3 origem;
            public List<Vector3> linhas = new List<Vector3>();
        public pontos_(Vector3 a)
            {
                origem = a;
            }
        
        }

        public List<pontos_> pontosINternos = new List<pontos_>(), pontosExternos = new List<pontos_>(), pontosinternosBKP = new List<pontos_>();
        public float rangeDistPulo;
        public void criarCaminhos()
        {
            ordemTrianguo.Clear();
            pontosINternos.Clear();
            pontosINternos.AddRange(pontosinternosBKP);

            foreach (var a in pontosINternos)
            { a.linhas.Clear(); }

            // adiciona as linhas internas
            foreach (var a in pontosINternos)
            {
                foreach (var b in pontosINternos)
                {
                    if (a == b)
                        continue;

                    // vai de A para B
                    pontos_ pontoAtual_ = a;
                    pontos_ proximoPonto = a;
                    float euriBase = Mathf.Infinity;
                    int contador = 0;
                    while (proximoPonto != b)
                    {
                        if (contador > 100)
                            break;
                        List<pontos_> tempListProximo = new List<pontos_>();
                        foreach (var c in pontosINternos)
                        {
                            if (c == pontoAtual_)
                                continue;

                            float euristica = Vector3.Distance(pontoAtual_.origem, c.origem);
                            if (euristica < euriBase)
                            {
                                euriBase = euristica;
                                tempListProximo.Add(c);
                            }
                        }
                        if (tempListProximo.Count > 0)
                        {
                            float tempdist = Mathf.Infinity;
                            foreach (var c in tempListProximo)
                            {
                                float eurist = Vector3.Distance(c.origem, b.origem);

                                if (eurist < tempdist)
                                {
                                    tempdist = eurist;
                                    proximoPonto = c;
                                }

                            }
                        }
                        else
                        {
                            Debug.Log("erro 587");
                            break;
                        }
                        pontoAtual_.linhas.Add(proximoPonto.origem);
                        pontoAtual_ = proximoPonto;

                    }

                }
            }

            // remove as linhas cruzadas
            for (int x = 0; x < pontosINternos.Count; x++)
            {
                pontos_ A = pontosINternos[x];
                for (int y = 0; y < pontosINternos.Count; y++)
                {
                    pontos_ B = pontosINternos[y];

                    if (A == B) { continue; }

                    for (int x_ = 0; x_ < A.linhas.Count; x_++)
                    {
                        for (int y_ = 0; y_ < B.linhas.Count; y_++)
                        {
                            Vector3 A_ = A.linhas[x_];
                            Vector3 B_ = B.linhas[y_];

                            if (A_ == B_ || A_ == B.origem || B_ == A.origem)
                                continue;

                            if (ValoresUniversais.LinhasCruzadas(A.origem, A_, B.origem, B_))
                            {
                                y_ = 0;

                                B.linhas.Remove(B_);

                            }
                        }
                    }
                }
            }


          
            
            // adiciona as entradas de dentro pra fora    
            List<pontos_> tempPE = new List<pontos_>();
            int indcz = pontosINternos.Count - 1;
            for (int x = 0; x < pontosExternos.Count; x++)
            {

                pontos_ tempP = new pontos_(pontosExternos[x].origem);
                float PI = Mathf.Infinity;
                Vector3 tempV = Vector3.zero;
                foreach (var a in pontosInternos)
                {
                    float tempDist = Vector3.Distance(tempP.origem, a);
                    if (tempDist < PI)
                    {
                        tempV = a;
                        PI = tempDist;
                    }
                }

                tempP.linhas.Add(tempV);


                tempPE.Add(tempP);
            }
            // remove as linhas cruzadas

            for (int x = 0; x < tempPE.Count; x++)
            {
                for (int z = 0; z < tempPE[x].linhas.Count; z++)
                {
                    for (int a = 0; a < pontosINternos.Count; a++)
                    {
                        for (int b = 0; b < pontosINternos[a].linhas.Count; b++)
                        {

                            if (tempPE[x].linhas[z] == pontosINternos[a].origem ||
                                tempPE[x].linhas[z] == pontosINternos[a].linhas[b])
                                continue;

                            if (ValoresUniversais.LinhasCruzadas(
                                tempPE[x].origem, tempPE[x].linhas[z], pontosINternos[a].origem,
                                pontosINternos[a].linhas[b]
                                ))
                            {
                               pontosINternos[a].linhas.RemoveAt(b);
                              b = 0;
                            }
                        }
                    }
                }
            }

            // remover linhas que cruzam fora
            if (flagmudar)
            {
                for (int x = 0; x < tempPE.Count; x++)
                {
                    for (int z = 0; z < tempPE[x].linhas.Count; z++)
                    {

                        for (int i = 0; i < todosPontosEmVolta.Count - 1; i++)
                        {
                            Vector3 a = todosPontosEmVolta[i];
                            Vector3 b = todosPontosEmVolta[i + 1];

                            if (Vector3.Distance(a, b) < distancia_removerAoRedor)
                            {
                                if (ValoresUniversais.LinhasCruzadas(tempPE[x].origem, tempPE[x].linhas[z], a, b))
                                {
                                    tempPE[x].linhas.RemoveAt(z);
                                    z = 0;
                                    break;
                                }
                            }
                        }

                    }
                }
            }
                    pontosINternos.AddRange(tempPE);

        }


        public bool flagmudar;
        public float distancia_removerAoRedor;
             
        public class removerCruz {
           public Vector3 A, A_, B, B_;
            public removerCruz(Vector3 a, Vector3 a_, Vector3 b, Vector3 b_)
            {
                A = a;
                A_ = a_;
                B = b;
                B_ = b_;
            }
        }
        public Mesh mesh;
        public bool gerarMalha;
        public void criarMalha()
        {
            ordemTrianguo.Clear();
            foreach(var a in pontosINternos)
            {
                foreach( var b in a.linhas)
                {
                    Vector3 orientacao = Vector3.Cross(b - a.origem, Vector3.up).normalized;

                    Vector3 AE = a.origem + orientacao - transform.position;
                    Vector3 AD = a.origem - orientacao - transform.position;

                    Vector3 BE = b + orientacao - transform.position;
                    Vector3 BD = b - orientacao- transform.position;

                    ordemTrianguo.Add(AE);
                    ordemTrianguo.Add(BE);
                    ordemTrianguo.Add(AD);

                    ordemTrianguo.Add(BE);
                    ordemTrianguo.Add(BD);
                    ordemTrianguo.Add(AD);


                }
            }
            gerarmalha();
        }
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public List<Vector3> ordemTrianguo = new List<Vector3>();
        public void gerarmalha()
        {
            mesh = new Mesh();

            // Atribuir os vértices à malha
            mesh.vertices = ordemTrianguo.ToArray();

            // Definir os triângulos (assumindo que os vértices estão em grupos de três para formar triângulos)
            int[] triangles = new int[ordemTrianguo.Count];
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
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
                if (meshFilter == null)
                {
                    meshFilter = gameObject.AddComponent<MeshFilter>();
                }
            }
            meshFilter.mesh = mesh;

            // Atribuir um material (pode ajustar conforme necessário)
            if (gerarMalha)
            {
                 meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }


        }

    }

    [CustomEditor(typeof(GerenciadorDeArea))]
    public class EditorGerenciadorDeArea  : Editor
    {
       // public SerializedProperty aaaaaaaaa;
        void OnEnable()
        {
          //  aaaaaaaaa = serializedObject.FindProperty("aaaaaaaaa");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
          //  EditorGUILayout.PropertyField(aaaaaaaaa, new GUIContent("aaaaaaaaaaaaaaaaaa"));
            GerenciadorDeArea meuScript = (GerenciadorDeArea)target;
            if (GUILayout.Button("Testar"))
            {
              meuScript.verificarSeEstaFechado();

            }
            if (GUILayout.Button("Criar arredor"))
            {

                meuScript.calcularAoReddor();
            }
            if (GUILayout.Button("Criar pontos internos"))
            {
                meuScript. adicionarPontosInternos();
                    }

            if (GUILayout.Button("Criar caminhos"))
            {
                meuScript.criarCaminhos();
            }
            if (GUILayout.Button( " renderizar malha"))
            {
               
                    meuScript.criarMalha();
                
               
            }


                serializedObject.ApplyModifiedProperties();
        }
        private void OnSceneGUI()
        {
            GerenciadorDeArea meuScript = (GerenciadorDeArea)target;
            if (meuScript.GuizmosAoRedor) { 
            if (meuScript.PontosEmVolta != null)
            {
                    foreach (var a in meuScript.PontosEmVolta)
                    {
                       
                            Handles.color = Color.red;
                            Handles.DrawSolidDisc(a, Vector3.up, 10);
                        
                    }
                }
            }
            if (meuScript.GuizmosInterno)
            {
                if(meuScript.pontosInternos.Count > 0)
                {
                    foreach (var a in meuScript.pontosInternos)
                    {

                        Handles.color = Color.blue;
                        Handles.DrawSolidDisc(a, Vector3.up, 10);

                    }
                }
            }

            if (meuScript.guizmosLinhasCaminho)
            {
                if (meuScript.pontosINternos.Count > 0)
                {
                    foreach(var a in meuScript.pontosINternos)
                    {
                        foreach(var b in a.linhas)
                        {
                            Handles.color = Color.red;
                            Handles.DrawLine(a.origem, b,5);
                        }
                    }
                }
                {

                }
            }
            }

        }
    }
