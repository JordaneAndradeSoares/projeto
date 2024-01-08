using Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace jogador
{
    public class AttMatrizDeDados : MonoBehaviour
    {
        public GameObject PontoModelo,modelo;

        public GameObject evoluir;

        Kernel KernelAtual;

        public void Selecionado(Kernel a)
        {
            if(KernelAtual != a)
            {
                KernelAtual = a;
                
                if(modelo != null)
                {
                    Destroy(modelo);

                    modelo = Instantiate(a.bufferData.modelo_3D,PontoModelo.transform);
                }
                else
                {
                    modelo = Instantiate(a.bufferData.modelo_3D, PontoModelo.transform);
                }


                evoluir.SetActive(a.bufferData.evolui);

            }

        }
        public void Sacrificar()
        {
            Debug.Log("sacrificando");
        }
        public void Carregar()
        {
            Debug.Log("sacrificando");
        }

        private void OnDisable()
        {
            
        }
        private void OnEnable()
        {
            
        }
    }
}
