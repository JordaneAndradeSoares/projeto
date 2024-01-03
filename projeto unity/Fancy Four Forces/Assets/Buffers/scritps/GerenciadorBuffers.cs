using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Buffers;
using UnityEngine.SceneManagement;

namespace Buffers
{
    public class GerenciadorBuffers : MonoBehaviour
    {
        public ScriptavelBuffer bufferData;
        public NavMeshAgent agente;
        public DetectorDeColisor dtC;
        public  ScriptavelGrupoDeCriaturas b;
        public float scalaRD;
        public bool olhando()
        {
            
            return !dtC.detectando;
        }
        public void andarAleatorio()
        {
            if(agente.remainingDistance <= 1)
            {
                agente.destination = new Vector3(Random.Range(-1f, 1f) * scalaRD, 0, Random.Range(-1f, 1f) * scalaRD) + transform.position;
            }
        }
        public bool andarParaAlvo()
        {
            agente.destination = dtC.alvo;

            return agente.remainingDistance <=1;
        }
        public void iniciabatalha()
        {
            GeralInimigos.instact.iniciarBatalhaComDesvantagem(bufferData,b);
            
        }
        public void Update()
        {
            if(olhando())
            {
                andarAleatorio();
            }
            else
            {
                if(andarParaAlvo())
                {
                    iniciabatalha();
                }
            }
        }
    }
}
