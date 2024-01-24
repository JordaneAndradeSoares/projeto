using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Buffers;
using UnityEngine.SceneManagement;
using Ageral;

namespace Buffers
{
    public class GerenciadorBuffers : MonoBehaviour
    {
        public ScriptavelBuffer bufferData;
        public NavMeshAgent agente;
        public DetectorDeColisor dtC;
        public  ScriptavelGrupoDeCriaturas b;
        public CriadorDeAreas1 cra;
        public float scalaRD;
        public float DistanciaParaSpawnar;
       
        public void iniciar()
        {
          GameObject temp =  Instantiate(bufferData.modelo_3D, transform);
            temp.transform.localPosition = Vector3.zero;
        }
        public bool olhando()
        {
            
            return !dtC.detectando;
        }
        public void andarAleatorio()
        {
            if(agente.remainingDistance <= 1)
            {
                agente.destination = cra.PontoAleatorio(); //new Vector3(Random.Range(-1f, 1f) * scalaRD, 0, Random.Range(-1f, 1f) * scalaRD) + transform.position;
            }
        }
        public bool andarParaAlvo()
        {
            agente.destination = dtC.alvo;

            return agente.remainingDistance <= 1.5f;// agente.stoppingDistance;
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
            if(dtC.gm != null){
            if(Vector3.Distance(dtC.gm.transform.position,transform.position) <1.5f(
 GeralInimigos.instact.iniciarBatalhaComDesvantagem(bufferData,b);            }
            }
        }
    }
}
