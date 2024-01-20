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
                equipes.aliados[slot_] = buffer_;
                //equipes.aliados[slot_].bufferData = buffer_.bufferData;
               // equipes.aliados[slot_].level = buffer_.level;
              

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
                Debug.Log("foi colocado o " + origemInventadoSelecionado.dados.bufferData.Nome + " no slot " + origemEquipado.slot);
                origemInventadoSelecionado = null; origemEquipado = null;
             
            }
        }

        private void Start()
        {
            if(EntrarEmBatalha.instanc == null) { EntrarEmBatalha.instanc = this; }
        }
     
        public  void inicarBatalhaComVantagem(GerenciadorBuffers bd)
        {

            equipes.inimigos[0].bufferData = bd.bufferData;
            equipes.inimigos[0].level = Random.Range(1, 20);
            equipes.inimigos[0].completarVida();

            for (int x = 1; x < 4; x++)
            {
                float rng = Random.Range(0f, 1f);
                int indc = (int)(bd.b.BufferData.Count * rng);
                if (indc > bd.b.BufferData.Count)
                    indc = bd.b.BufferData.Count - 1;

                if (indc < 0)
                    indc = 0;
                Debug.Log("rng  = " + rng + "    " + bd.b.BufferData[indc]);
                equipes.inimigos[x].bufferData = bd.b.BufferData[indc];
                equipes.inimigos[x].level = Random.Range(1, 20);
                equipes.inimigos[x].completarVida();

            }


            SceneManager.LoadScene("ModoDeBatalha");

        }
    }
}
