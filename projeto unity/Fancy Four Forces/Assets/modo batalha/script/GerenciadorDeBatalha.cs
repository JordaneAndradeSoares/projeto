using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static modoBatalha.Configuracao;
using TMPro;
using Unity.Plastic.Antlr3.Runtime.Misc;
using Buffers;
using Unity.VisualScripting.YamlDotNet.Core;

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
                auxH_.Habilidade.text = aux.data.bufferData.AtaqueBasico.NomeAtaqueBasico;
                auxH_.GB = this;
                auxH_.SH = null;
                auxH_.SAB = aux.data.bufferData.AtaqueBasico;
                habilidadesInstanciadas.Add(aux_ATkB);
            }
        }

        buffer_s ultimobuffer = new buffer_s();
        AuxUiHabilidade habilidadeUsada;
        public void EscolheuHabilidade_(AuxUiHabilidade a)
        {
            habilidadeUsada = a;


          //  proximo();
        }
        public List<buffer_s> alvos_ = new List<buffer_s>();
        bool flag1 = false;


        // 1 = inimigo ; -1 = aliado
        public void usarSeta(int x)
        {
            confg.vertical(x);
            confg.moverseta();
        }
  
        public void escolidoAlvo(buffer_s a)
        {
            flag1 = true;
            alvos_.Add(a);
        }
        /*
         ultimobuffer = quem esta usando a habilidade
        habilidadeUsada = habilidade que esta sendo apricada no alvo

        a = alvo;
         */
        public void aplicarHabilidadeEmAlvo(buffer_s a)
        {// habilidade
            if(habilidadeUsada.SH != null) {
                switch (habilidadeUsada.SH._Efeito)
                {
                    case(Efeito.Dano):
                        a.diminuirVida(ultimobuffer.danoBruto(habilidadeUsada.SH));
                        break;
                    case (Efeito.MudarStatus):

                        switch (habilidadeUsada.SH.___StatusAAlterar)
                        {
                            case StatusAAlterar.Velocidade:
                                a.modificarStatus(habilidadeUsada.SH.___StatusAAlterar, habilidadeUsada.SH.porcentagemDoEfeito);
                                break;

                            default:

                                break;
                        }

                        break;

                }
                    }
           
           
        }
        public void usandoHabilidade()
        {
            //escolher alvo
            if (flag1 == false)
            {
                // habilidade 
                if (habilidadeUsada.SH != null)
                {
                    int dir = 0;
                    switch (habilidadeUsada.SH._Efeito)
                    {

                        case (Efeito.Dano):
                            dir = 1;

                            break;

                        default:
                            dir = -1;
                            break;
                    }

                    switch (habilidadeUsada.SH._Alvo)
                    {
                        case (Alvo.Unico):
                            usarSeta(1);
                           // confg.vertical(1);
                          //  confg.moverseta();
                            break;
                        case (Alvo.Global):

                            if(dir > 1)
                            {
                                alvos_.AddRange(confg.inimigosL);
                            }
                            else
                            {
                                alvos_.AddRange(confg.aliadosL);
                            }
                            flag1 = true;

                            break;

                    }

                }
                // atk basico
                else
                {
                    usarSeta(1);
                //    confg.vertical(1);
               //     confg.moverseta();
                }               
               
             
            }
            // aplicar efeito
            else
            {
                if (alvos_.Count > 0)
                {
                    foreach (var a in alvos_)
                    {
                        aplicarHabilidadeEmAlvo(a);
                    }
                }
                proximo();
            }

        }
        private void proximo()
        {
            buffer_s temp = ordemBatalha[0];
            ordemBatalha.Remove(temp);
            ordemBatalha.Add(temp);
            definirQuemJoga();
            attMOlduraFIla();

            alvos_.Clear();
            habilidadeUsada = null;
            flag1 = false;
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
              //  Debug.Log("inimigo fez algo ?");
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
