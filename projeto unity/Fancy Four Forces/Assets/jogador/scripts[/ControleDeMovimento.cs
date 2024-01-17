using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Buffers;
using ValoresGlobais;
using UnityEngine.SceneManagement;
using modoBatalha;
using System;
using UnityEngine.UIElements;
using UnityEditor.Overlays;

namespace jogador
{
    public class ControleDeMovimento : MonoBehaviour
    {

        private CharacterController controller;
        public GameObject olharPara;
     
        public Terrain terreno;
       
      
        float aux_pulo;
        public bool flag_pulo;
        public Camera cam;
        
        public GerenciadorBuffers bufferAlvo;
        bool flagEquipe;
        public ScriptavelInventario inventario;

        public GameObject prefabMolduraInventario;
        public GameObject FundoInventarioInventario;

        public GameObject abrirefecharInventario, AbrirEFrecharAMD, MostrarModelo3DAmd;

        public GameObject FundoSlotIMatrizDeDados, prefabSlotFundoSlotIMatrizDeDados;
        public AttMatrizDeDados AMD;



        public List<GameObject> MoldurasGerenciamentoDeEquipe = new List<GameObject>();
        public List<GameObject> MoldurasMatrizDeDados = new List<GameObject>();
        public Vector3 tamanho;
        public float distancia;
        public LayerMask camadaInimigo;

        public config configui;

        [System.Serializable]
        public struct config
        {
            public float velocidadeJogador, gravidade;
           
            public float massaJogador;
            public float alturaSalto,saltoMaximo;
        }

        // controladores de inspector

        

        void Start()
        {


            controller = GetComponent<CharacterController>();
            terreno = FindObjectOfType<Terrain>();
        }
        void Update()
        {
            movimentar();


            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.atacar))
            {
                if (bufferAlvo != null)
                {
                    iniciarCombate();
                }
            }

            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.abrirEquipe))
            {
                abrirEquipe();
            }
            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.AbrirMatrizDeDados))
            {
                AbrirAttMatrizDeDados();
            }

            //  tirarDoInventarioOsQueEstiveremEquipados();

        }
        public float GravidadeAtual,ForcaPuloAtual;
        public Vector3 moveDirection;
        public void movimentar()
        {
            // Obt?m entrada do jogador para movimento
            float horizontalInput = Input.GetKey(GerenciadorDeTeclado.instanc.paraDireita) ? 1 : 0;
            horizontalInput += Input.GetKey(GerenciadorDeTeclado.instanc.paraEsquerda) ? -1 : 0;

            float verticalInput = Input.GetKey(GerenciadorDeTeclado.instanc.paraFrente) ? 1 : 0;
            verticalInput += Input.GetKey(GerenciadorDeTeclado.instanc.paraTras) ? -1 : 0;



            // Calcula o vetor de movimento com base na entrada
          
                moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
            
            moveDirection *= configui.velocidadeJogador;

            moveDirection.Normalize();

            // Aplica a gravidade
            RaycastHit rh;

            Physics.Raycast(transform.position, Vector3.down, out rh);
            Vector3 FGrav = Vector3.up * configui.massaJogador * configui.gravidade;

            GravidadeAtual = FGrav.y;
            ForcaPuloAtual = aux_pulo;
            
            //iniciou pulo, ganha o minimo de forca para pular
            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.paraPular))
            {
                if (flag_pulo == false)
                {
                    float a;

                    a = Mathf.Sqrt(2 * configui.gravidade * configui.alturaSalto);
                    Debug.Log(a);
                    aux_pulo = configui.massaJogador * a;

                }
            }
            //enquanto persciona voce acumula forc do pulo
            if (Input.GetKey(GerenciadorDeTeclado.instanc.paraPular))
            {
              
                if (flag_pulo == false)
                {
                    aux_pulo = aux_pulo >configui.saltoMaximo ? configui.saltoMaximo : aux_pulo + (Time.deltaTime * configui.alturaSalto);
                }
            }
            // quando soltar altera a flag do pulo
            else
            {
                flag_pulo = true;
                if (aux_pulo <= 0)
                {

                    flag_pulo = false;

                }

            }
            // aplicando gravidade
            moveDirection.y -= FGrav.y;
            // aplicando forca do pulo
            if (flag_pulo)
            {
                moveDirection.y += aux_pulo ;
                aux_pulo -= FGrav.y * Time.deltaTime;

                
            }
         // forma grotesca de testar se já chegou no chao se sim forc pulo = 0;
         if(rh.distance < 1.2f && flag_pulo && aux_pulo < configui.gravidade)
            {
                aux_pulo = 0;
            }
    
         // gambiarra para gerar o modelo do jogador
            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                Vector3 a = transform.position + moveDirection;
                a.y = transform.position.y;
                olharPara.transform.position = Vector3.Lerp(olharPara.transform.position, a, Time.deltaTime * 10);
              
            }
            else
            {


            }
          

            controller.Move(moveDirection *configui.velocidadeJogador * Time.deltaTime);


        }
        public void iniciarCombate()
        {

            EntrarEmBatalha.instanc.inicarBatalhaComVantagem(bufferAlvo);
        }
        public void abrirEquipe()
        {
            flagEquipe = !flagEquipe;
            if (flagEquipe)
            {
                foreach(var a in inventario.Inventario)
                {

                    
                    GameObject b = Instantiate(prefabMolduraInventario, FundoInventarioInventario.transform);
                    auxUIInventario c = b.GetComponent<auxUIInventario>();
                    c.iniciar(a);

                    MoldurasGerenciamentoDeEquipe.Add(b);
                }
            }
            else
            {
                foreach(var a in MoldurasGerenciamentoDeEquipe)
                {
                    Destroy(a);
                }

                MoldurasGerenciamentoDeEquipe.Clear();
            }


            abrirefecharInventario.SetActive(flagEquipe);


        }
        public void AbrirAttMatrizDeDados()
        {
            AbrirEFrecharAMD.SetActive(!AbrirEFrecharAMD.activeSelf);
            MostrarModelo3DAmd.SetActive(AbrirEFrecharAMD.active);
            if (AbrirEFrecharAMD.activeSelf)
            {
                foreach (var a in inventario.Inventario)
                {
                    GameObject b = Instantiate(prefabSlotFundoSlotIMatrizDeDados, FundoSlotIMatrizDeDados.transform);
                    b.GetComponent<AuxAttMatrizDeDados>().iniciar(a, AMD);
                    MoldurasMatrizDeDados.Add(b);
                }
            }
            else
            {
                foreach(var a in MoldurasMatrizDeDados)
                {
                    Destroy(a);
                }
                MoldurasMatrizDeDados.Clear();
            }
        }
        public void tirarDoInventarioOsQueEstiveremEquipados()
        {
            foreach(var a in EntrarEmBatalha.instanc.equipes.aliados)
            {
               
               Destroy( MoldurasGerenciamentoDeEquipe.Find(x => x.GetComponent<auxUIInventario>().dados == a.origem));
            }
            MoldurasGerenciamentoDeEquipe.RemoveAll(x => x == null);
        }
    }

    [CustomEditor(typeof(ControleDeMovimento))]
    public class EditorMovimento : Editor {

        public SerializedProperty configui;
        void OnEnable()
        {
            configui = serializedObject.FindProperty("configui");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
           // EditorGUILayout.PropertyField(configui, new GUIContent("configuracao"));
            ControleDeMovimento meuScript = (ControleDeMovimento)target;
            GUILayout.Label("Gravidade :" + meuScript.GravidadeAtual);
            GUILayout.Label("Pulo :" + meuScript.ForcaPuloAtual);
            serializedObject.ApplyModifiedProperties();

        }
        private void OnSceneGUI()
        {
          
          

            ControleDeMovimento meuScript = (ControleDeMovimento)target;

            Handles.DrawWireCube(meuScript.olharPara.transform.position + meuScript.transform.position, meuScript.tamanho);

        }
        }

}