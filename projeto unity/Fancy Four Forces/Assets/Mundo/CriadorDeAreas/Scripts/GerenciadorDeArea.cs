using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ageral;
using Buffers;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor.TerrainTools;

namespace Areas
{
    public class GerenciadorDeArea : MonoBehaviour
    {
        [HideInInspector]
        public CriadorDeAreas1 _criradorDeAreas;
        public Terrain terreno;
        public ScriptavelGrupoDeCriaturas _grupoDeCriats;
        public List<CriaturasSpawnadas> spawns = new List<CriaturasSpawnadas>();
        public int quantidadeMaximaDeCriaturas;
        public GameObject ExemploModelo3D;
        public bool MostrarCirculoDeDistanciaParaSpawnar;
        public float DistanciaParaSpawnar;
        public float tempoEntreCadaSpawn;
        public GameObject jogador;

        public bool noite;

        float tempF;
        
        private void Update()
        {
            if (TestarDistancia())
            {
                if(tempF > tempoEntreCadaSpawn)
                {
                    tempF = 0;

                    if (spawns.Count < quantidadeMaximaDeCriaturas)
                    {
                        SpawnarCriatura();
                    }
                }
                tempF += Time.deltaTime;
            }
            else
            {
                foreach(var a in spawns)
                {
                    Destroy(a.obj);
                }
                spawns.Clear();
            }
        }
   
        public bool TestarDistancia()
        {
            if (!jogador)
                return false;
            Vector3 A = jogador.transform.position;
            A.y = A.z;

            Vector3 B = transform.position;
            B.y = B.z;

            return Vector2.Distance(A, B) < DistanciaParaSpawnar;

        }

        [System.Serializable]
        public class CriaturasSpawnadas
        {
            public GameObject obj;
            public Kernel knl;
           
        }
        public Vector3 posicaoAleatoria()
        {
            return _criradorDeAreas.PontoAleatorio();

            if (_criradorDeAreas.PontosFloresta.Count < 3)
                return Vector3.zero;

            Vector3 a = _criradorDeAreas.PontosFloresta[Random.Range(0, _criradorDeAreas.PontosFloresta.Count)] ;

            
            Vector3 b = _criradorDeAreas.PontosFloresta[Random.Range(0, _criradorDeAreas.PontosFloresta.Count)];
            Vector3 c = _criradorDeAreas.PontosFloresta[Random.Range(0, _criradorDeAreas.PontosFloresta.Count)];
            if (a == b)
            {
                while (a == b)
                {
                    b = _criradorDeAreas.PontosFloresta[Random.Range(0, _criradorDeAreas.PontosFloresta.Count)];
                }
            }

            if (c == b || c == a)
            {
                while (c == b || c == a)
                {
                    c = _criradorDeAreas.PontosFloresta[Random.Range(0, _criradorDeAreas.PontosFloresta.Count)];
                }
            }
            float distAB = Random.Range(0f, 100f)/100;
            float distABC = Random.Range(0f, 100f) / 100;
            Vector3 ab = Vector3.Lerp(a,b, distAB);
            Vector3 abc = Vector3.Lerp(ab,c, distABC) + transform.position;

            abc.y = terreno.SampleHeight(abc);
            return abc;
        }
        public ScriptavelBuffer EscolherBuffer()
        {

            if (_grupoDeCriats.BufferDataProbabilidade.Count > 0)
            {

                List<ScriptavelGrupoDeCriaturas.probabilidades> temp_ = _grupoDeCriats.BufferDataProbabilidade.FindAll(x => x.data == noite);
                if (temp_.Count == 0)
                    return null;
             
                float tempB = Random.Range(0f,noite? _grupoDeCriats.somatoriaProb_noite : _grupoDeCriats.somatoriaProb_dia);
                float tempC = 0;
                ScriptavelBuffer tempBuffer = temp_[0].data;

                foreach (var a in temp_)
                {
                    tempC += a.probabilidade;

                    if(tempB < tempC)
                    {
                        tempBuffer = a.data;
                        break;
                    }
                }
                return tempBuffer;

            }
            else
            {
                return _grupoDeCriats.BufferData[Random.Range(0, _grupoDeCriats.BufferData.Count)];
            }
            return null;
        }

        public void SpawnarCriatura()
        {
            Vector3 tempV = posicaoAleatoria();

            ScriptavelBuffer tempbuf = EscolherBuffer();

            GameObject tempG = Instantiate(ExemploModelo3D, transform); 
         tempG.transform.position = tempV ;

            GerenciadorBuffers tempGB = tempG.GetComponentInChildren<GerenciadorBuffers>();
            tempGB.bufferData = tempbuf;
            tempGB.b = _grupoDeCriats;
            tempGB.iniciar();
            tempGB.cra = _criradorDeAreas;
            CriaturasSpawnadas tempCS = new CriaturasSpawnadas();
            tempCS.obj = tempG;
            spawns.Add(tempCS);

        }

        public int qauntidadeMaximaDeCriaturas;
        public void spawnarEmMassa()
        {
            for(int x = 0; x < qauntidadeMaximaDeCriaturas; x++)
            {
                SpawnarCriatura();
            }
        }
        public void limaparCriaturas()
        {
            foreach(var a in spawns)
            {
                DestroyImmediate(a.obj);
            }
            spawns.Clear();
        }

        public bool salvar;

        private void OnDestroy()
        {
            if (salvar)
           savlar(spawns);
        }
        private void Start()
        {
            if(salvar)
                carregar();
            _criradorDeAreas = GetComponent<CriadorDeAreas1>();
        }
        public void savlar(List<CriaturasSpawnadas> s ) {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath;
            FileStream file = File.Create(path + "/SalvarDados" + gameObject.name + ".save");
        
            bf.Serialize(file, s);
            file.Close();

        }
        public List<CriaturasSpawnadas> carregar()
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath;
            FileStream file;

            if(File.Exists((path + "/SalvarDados" + gameObject.name + ".save"))){
                file = File.Open(path + "/SalvarDados" + gameObject.name + ".save", FileMode.Open);
                List<CriaturasSpawnadas> l = (List<CriaturasSpawnadas>)bf.Deserialize(file);
                file.Close();
                return l;
            }
            return null;
        }


    }

    [CustomEditor(typeof(GerenciadorDeArea))]
    public class EditorGerenciadorDeArea : Editor
    {
        private void OnEnable()
        {
            GerenciadorDeArea meuscript = (GerenciadorDeArea)target;
            if (!meuscript._criradorDeAreas)
            {
                CriadorDeAreas1 temp = meuscript.GetComponent<CriadorDeAreas1>();
                if(temp != null)
                {
                    meuscript._criradorDeAreas = temp;

                }
                else
                {
                    meuscript._criradorDeAreas = meuscript.gameObject.AddComponent<CriadorDeAreas1>();
                }
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GerenciadorDeArea meuscript = (GerenciadorDeArea)target;
            if (GUILayout.Button("Criar uma criatura"))
            {
                meuscript.SpawnarCriatura();
            }
            if (GUILayout.Button("Criar em massa as criatura"))
            {
                meuscript.spawnarEmMassa();
            }
            if (GUILayout.Button("Remover Criaturas"))
            {
                meuscript.limaparCriaturas();
            }
           
        }
        public void OnSceneGUI()
        {
            GerenciadorDeArea meuscript = (GerenciadorDeArea)target;

            if (meuscript.MostrarCirculoDeDistanciaParaSpawnar)
            {
                Handles.DrawWireDisc(meuscript.transform.position, Vector3.up, meuscript.DistanciaParaSpawnar);
            }
        }
    }

    }
