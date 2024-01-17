using Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jogador
{
    public class auxUIInventario : MonoBehaviour
    {
        public Kernel dados;

        public bool Equipado;
        public int slot;

        public RawImage iconeTextura;
        public ScriptavelBatalhaBuffer SBB;
        private void Start()
        {

            if (Equipado)
            {
              
                iniciar(new Kernel(SBB.aliados[slot]));
            }
        }

        public void iniciar(Kernel a)
        {
            dados = a;

            iconeTextura.texture = dados.bufferData.iconeMiniatura;
            // aparecer a textura do buffer
        }
        // somente selecionar a onde na equipe ele vai estar
        public void selecionado()
        {
           
                Debug.Log("selecionou o slot da equipe");
                EntrarEmBatalha.instanc.selecionarSlotDeEquipe(this);

               
           
        }
        public void DefinirNaEquipe()
        {
            Debug.Log("selecionou inventario");
            EntrarEmBatalha.instanc.selecionarInventario(this);
        }
    }
}
