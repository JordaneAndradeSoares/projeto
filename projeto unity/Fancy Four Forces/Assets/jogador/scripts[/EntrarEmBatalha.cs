using Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jogador
{
    public class EntrarEmBatalha : MonoBehaviour
    {

        public static EntrarEmBatalha instanc;

        public ScriptavelBatalhaBuffer equipes;
        
        public void adicionarAEquipe(bool aliado,Kernel buffer_ , int slot_)
        {
            if (aliado)
            {

                equipes.aliados[slot_].bufferData = buffer_.bufferData;
                equipes.aliados[slot_].level = buffer_.level;
                equipes.aliados[slot_].origem = buffer_;

            }
            else
            {
                equipes.inimigos[slot_].bufferData = buffer_.bufferData;
                equipes.inimigos[slot_].level = buffer_.level;
            }
        }

        // slot equipado

        auxUIInventario origemEquipado;

        public void selecionarSlotDeEquipe(auxUIInventario a)
        {
            if (a == origemEquipado)
            {
                origemEquipado = null;
            }
            else
            {
                origemEquipado = a;
            }
            casar();
        }
        // slot de inventario

        auxUIInventario origemInventadoSelecionado;
        public void selecionarInventario(auxUIInventario a )
        {
            if (a == origemInventadoSelecionado)
            {
                origemInventadoSelecionado = null;
            }
            else
            {
                origemInventadoSelecionado = a;
            }

            casar();
        }

        public void casar()
        {
            if(origemInventadoSelecionado != null && origemEquipado != null)
            {
                origemInventadoSelecionado.Equipado = true;

                origemEquipado.iconeTextura.texture = origemInventadoSelecionado.dados.bufferData.iconeMiniatura;

                adicionarAEquipe(true, origemInventadoSelecionado.dados, origemEquipado.slot);

                origemInventadoSelecionado = null; origemEquipado = null;
            }
        }

        private void Start()
        {
            if(EntrarEmBatalha.instanc == null) { EntrarEmBatalha.instanc = this; }
        }
        public  void inicarBatalhaComVantagem(GerenciadorBuffers a)
        {
            SceneManager.LoadScene("ModoDeBatalha");

        }
    }
}
