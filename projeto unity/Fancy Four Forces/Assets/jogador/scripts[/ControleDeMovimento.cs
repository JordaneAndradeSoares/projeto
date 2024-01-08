using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Buffers;
using ValoresGlobais;
using UnityEngine.SceneManagement;
using modoBatalha;

namespace jogador
{
    public class ControleDeMovimento : MonoBehaviour
    {

        private CharacterController controller;
        public GameObject olharPara;
        public float velocidadeJogador,gravidade;
        void Start()
        {


            controller = GetComponent<CharacterController>();
        }
        public float PuloMaximo, MultiplicadorDePulo;
        float aux_pulo;
        bool flag_pulo;

        public void movimentar()
        {
            // Obt?m entrada do jogador para movimento
            float horizontalInput = Input.GetKey(GerenciadorDeTeclado.instanc.paraDireita) ? 1 : 0;
            horizontalInput += Input.GetKey(GerenciadorDeTeclado.instanc.paraEsquerda) ? -1 : 0;

            float verticalInput = Input.GetKey(GerenciadorDeTeclado.instanc.paraFrente) ? 1 : 0;
            verticalInput += Input.GetKey(GerenciadorDeTeclado.instanc.paraTras) ? -1 : 0;



            // Calcula o vetor de movimento com base na entrada
            Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput));
            moveDirection *= velocidadeJogador;

            // Aplica a gravidade
            moveDirection.y -= gravidade;

            // Move o CharacterController

            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.paraPular))
            {
                if (flag_pulo == false)
                {
                    aux_pulo += gravidade;
                }
            }

            if (Input.GetKey(GerenciadorDeTeclado.instanc.paraPular))
            {
                if (flag_pulo == false)
                {
                    aux_pulo = aux_pulo > PuloMaximo + gravidade ? PuloMaximo + gravidade : aux_pulo + (Time.deltaTime * MultiplicadorDePulo);
                }
            }
            else
            {
                flag_pulo = true;
                if (aux_pulo <= 0)
                {

                    flag_pulo = false;

                }

            }

            if (flag_pulo)
            {
                moveDirection.y += aux_pulo;
                aux_pulo -= Time.deltaTime * gravidade;
            }
            moveDirection.Normalize();

            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                Vector3 a = transform.position + moveDirection;
                a.y = transform.position.y;
                olharPara.transform.position = Vector3.Lerp(olharPara.transform.position, a, Time.deltaTime * 10);
              
            }
            else
            {


            }


            controller.Move(moveDirection * velocidadeJogador * Time.deltaTime);


        }
        public GerenciadorBuffers bufferAlvo;
        
        public void iniciarCombate()
        {

            EntrarEmBatalha.instanc.inicarBatalhaComVantagem(bufferAlvo);
        }
       
        bool flagEquipe;
        public ScriptavelInventario inventario;

        public GameObject prefabMolduraInventario;
        public GameObject FundoInventarioInventario;

        public GameObject abrirefecharInventario,AbrirEFrecharAMD , MostrarModelo3DAmd;

        public GameObject FundoSlotIMatrizDeDados, prefabSlotFundoSlotIMatrizDeDados;
        public AttMatrizDeDados AMD;

        public List<GameObject> MoldurasGerenciamentoDeEquipe = new List<GameObject>();
        public List<GameObject> MoldurasMatrizDeDados = new List<GameObject>();
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

        void Update()
        {
            movimentar();
          

            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.atacar))
            {
                if(bufferAlvo != null)
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
        public Vector3 tamanho;
        public float distancia;
        public LayerMask camadaInimigo;

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
        private void OnSceneGUI()
        {
            ControleDeMovimento meuScript = (ControleDeMovimento)target;

            Handles.DrawWireCube(meuScript.olharPara.transform.position + meuScript.transform.position, meuScript.tamanho);

        }
        }

}