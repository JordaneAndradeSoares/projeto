using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static modoBatalha.Configuracao;
using TMPro;
using Unity.Plastic.Antlr3.Runtime.Misc;
using Buffers;

namespace modoBatalha
{
    public class GerenciadorDeBatalha : MonoBehaviour
    {
        public Configuracao confg;

        public List<buffer_s> ordemBatalha = new List<buffer_s>();
        public TextMeshProUGUI tBug;


        // ui habilidades
        public GameObject botaoHabilidadePrefab;
        public GameObject Fundo_Habilidades;

        public GameObject MolduraPrefab;
        public GameObject ListaHorizontalGO;
     
        public void iniciar(List<buffer_s> a )
        {
            ordemBatalha.AddRange(a);
            definirQuemJoga();
            attMOlduraFIla();

        }

        public statusGame _statusgame = new statusGame();
        private void definirQuemJoga()
        {
            _statusgame = ordemBatalha[0].npc ? statusGame.maquina: statusGame.player;
        }
        public List<GameObject> habilidadesInstanciadas = new List<GameObject>();

        public List<GameObject> MolduraTextuaFila = new List<GameObject>();

        private void mostrarHabilidades()
        {

            buffer_s aux = ordemBatalha[0];
            if (aux != ultimobuffer)
            {

                ultimobuffer = aux;
                foreach (var a in aux.data.habilidades)
                {
                    GameObject aux_ = Instantiate(botaoHabilidadePrefab, Fundo_Habilidades.transform);
                    habilidadesInstanciadas.Add(aux_);
                    AuxUiHabilidade auxH = aux_.GetComponent<AuxUiHabilidade>();
                    auxH.Habilidade.text = a.NomeHabilidade;
                    auxH.GB = this;
                    auxH.SH = a;
                }

                GameObject aux_ATkB = Instantiate(botaoHabilidadePrefab, Fundo_Habilidades.transform);
                AuxUiHabilidade auxH_ = aux_ATkB.GetComponent<AuxUiHabilidade>();
                auxH_.Habilidade.text = aux.data.ataqueBasico.NomeHabilidade;
                auxH_.GB = this;
                auxH_.SH = aux.data.ataqueBasico;
                habilidadesInstanciadas.Add(aux_ATkB);
            }
        }

        buffer_s ultimobuffer = new buffer_s();
        ScriptavelHabilidades habilidadeUsada;
        public void EscolheuHabilidade_(ScriptavelHabilidades a)
        {
            habilidadeUsada = a;


          //  proximo();
        }
        public List<buffer_s> alvos_ = new List<buffer_s>();
        public void usandoHabilidade()
        {
          
            if (alvos_.Count > 0)
            {
                foreach(var a in alvos_)
                {
                    aplicarHabilidadeEmAlvo(a);
                }
            }
        }
        public void aplicarHabilidadeEmAlvo(buffer_s a)
        {

        }
        private void proximo()
        {
            buffer_s temp = ordemBatalha[0];
            ordemBatalha.Remove(temp);
            ordemBatalha.Add(temp);
            definirQuemJoga();
            attMOlduraFIla();
        }
        private void ocultarHabilidades()
        {
            buffer_s aux = ordemBatalha[0];
            if (aux != ultimobuffer)
            {

                ultimobuffer = aux;
                foreach (var a in habilidadesInstanciadas)
                {
                    Destroy(a);
                }
                habilidadesInstanciadas.Clear();
             
            }
        }

        private void attMOlduraFIla()
        {
           for(int a =0; a < MolduraTextuaFila.Count; a++)
            {
                Destroy(MolduraTextuaFila[a]);
            }
            MolduraTextuaFila.Clear();

            foreach(var a in ordemBatalha)
            {
                GameObject ab = Instantiate(MolduraPrefab, ListaHorizontalGO.transform);
                AuxiliarUiFila ac = ab.GetComponent<AuxiliarUiFila>();

                ac.foto_.texture = a.data.bufferData.iconeMiniatura;
                MolduraTextuaFila.Add(ab);
            }

        }

        public float tempoQueOInimigoPensa=2;
        float auxT=0;
        public void inimigoAgir()
        {
            if (auxT > tempoQueOInimigoPensa)
            {
                auxT = 0 ;
                Debug.Log("inimigo fez algo ?");
                proximo();
            }
            else { auxT += Time.deltaTime; }

        }

        private void Update()
        {
           
            if (ordemBatalha.Count >0)
            {
                if(_statusgame == statusGame.player)
                {
                    tBug.text = "jogador joga";

                    mostrarHabilidades();
                if(habilidadeUsada!= null)
                    {
                        usandoHabilidade();
                    }
                }
                else
                {
                    tBug.text = "maquina joga";

                    ocultarHabilidades();

                    inimigoAgir();


                }

            }
            else
            {
                tBug.text = " erro";
            }
        }
    }
    public enum statusGame
    {
        player,maquina,carregando
    }
}
