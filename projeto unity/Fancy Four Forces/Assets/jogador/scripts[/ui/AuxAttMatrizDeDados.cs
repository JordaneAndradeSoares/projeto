using Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jogador
{
    public class AuxAttMatrizDeDados : MonoBehaviour
    {
        public Kernel dados;
        public RawImage iconeTextura;
        public AttMatrizDeDados AMD;
        public void iniciar(Kernel a , AttMatrizDeDados b)
        {
            dados = a;

            AMD = b;
            iconeTextura.texture = dados.bufferData.iconeMiniatura;
           
        }

        public void selecionado()
        {
            AMD.Selecionado(dados);
        }
    }
}
