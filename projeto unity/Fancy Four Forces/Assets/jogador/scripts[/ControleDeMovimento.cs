using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Buffers;

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
        public void detectarInimigo()
        {
           RaycastHit[] hits = Physics.BoxCastAll(olharPara.transform.position,
               tamanho, olharPara.transform.position - transform.position,Quaternion.identity,distancia,camadaInimigo);

            if(hits.Length > 0)
            {

                GerenciadorBuffers gb = null;

                foreach(var a in hits)
                {
                    if(a.collider.GetComponent<GerenciadorBuffers>() != null)
                    {
                        gb = a.collider.GetComponent<GerenciadorBuffers>();
                        break;
                    }
                }
                if(gb != null)
                {
                    bufferAlvo = gb;
                }

            }
        
        }
        public void iniciarCombate()
        {

        }
        void Update()
        {
            movimentar();
            detectarInimigo();

            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.atacar))
            {
                if(bufferAlvo != null)
                {
                    iniciarCombate();
                }
            }

        }
        public Vector3 tamanho;
        public float distancia;
        public LayerMask camadaInimigo;
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