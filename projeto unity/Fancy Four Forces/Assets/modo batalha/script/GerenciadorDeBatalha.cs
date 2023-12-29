using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static modoBatalha.Configuracao;
using TMPro;
using Unity.Plastic.Antlr3.Runtime.Misc;

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
         

        }

        public statusGame _statusgame = new statusGame();
        private void definirQuemJoga()
        {
            _statusgame = ordemBatalha[0].npc ? statusGame.maquina: statusGame.player;
        }
        public List<GameObject> habilidadesInstanciadas = new List<GameObject>();

        public List<GameObject> MolduraTextuaFila = new List<GameObject>();

        buffer_s ultimobuffer = new buffer_s();

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
                }

                GameObject aux_ATkB = Instantiate(botaoHabilidadePrefab, Fundo_Habilidades.transform);
                AuxUiHabilidade auxH_ = aux_ATkB.GetComponent<AuxUiHabilidade>();
                auxH_.Habilidade.text = aux.data.ataqueBasico.NomeHabilidade;
                habilidadesInstanciadas.Add(aux_ATkB);
            }
        }
        private void ocultarHabilidades()
        {
            foreach(var a in habilidadesInstanciadas)
            {
                Destroy(a);
            }
            habilidadesInstanciadas.Clear();
        }

        private void attMOlduraFIla()
        {
            foreach(var a in MolduraTextuaFila)
            {
                Destroy(a);
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                attMOlduraFIla();
            }
            if (ordemBatalha.Count >0)
            {
                if(_statusgame == statusGame.player)
                {
                    tBug.text = "jogador joga";

                    mostrarHabilidades();
                }
                else
                {
                    tBug.text = "maquina joga";

                    ocultarHabilidades();
                
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
