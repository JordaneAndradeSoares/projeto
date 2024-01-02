using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static modoBatalha.Configuracao;
using TMPro;
using Unity.Plastic.Antlr3.Runtime.Misc;
using Buffers;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine.UIElements;

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




        public float EnergiaAtualInimiga;
        public float EnergiaAtualAliada;
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
                    if (EnergiaAtualAliada < a.GastoDeHabilidade)
                        continue;
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
        [Space()]
        [Space()]
        public buffer_s ultimobuffer = new buffer_s(-1,-1);
        public AuxUiHabilidade habilidadeUsada;
     
        public void EscolheuHabilidade_(AuxUiHabilidade a)
        {
            habilidadeUsada = null;
            habilidadeUsada = a;
            Debug.Log("clikou");

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

                        float danofinal = ultimobuffer.danoBruto(habilidadeUsada.SH);

                        switch (a.data.bufferData.TipoDeEfetividade)
                        {
                            case Efetividade.blindado:
                                switch (habilidadeUsada.SH._TipoDeAtaque)
                                {
                                    case TipoDeAtaque.Esmagamento:
                                        danofinal *= 2;
                                        break;
                                    case TipoDeAtaque.Corte:
                                        danofinal /= 2;
                                        break;
                                    case TipoDeAtaque.Perfurante:

                                        break;
                                }
                                break;
                            case Efetividade.liso:
                                switch (habilidadeUsada.SH._TipoDeAtaque)
                                {
                                    case TipoDeAtaque.Esmagamento:
                                        danofinal /= 2;
                                        break;
                                    case TipoDeAtaque.Corte:
                                        danofinal *= 2;
                                        break;
                                    case TipoDeAtaque.Perfurante:

                                        break;
                                }
                                break;
                        }
                        
                        a.diminuirVida(danofinal);
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
                    case (Efeito.Escudo):

                        a.darEscudo(habilidadeUsada.SH, ultimobuffer);
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

                   

                    if(habilidadeUsada.SH != null)
                    {
                        foreach (var a in alvos_)
                        {
                            aplicarHabilidadeEmAlvo(a);
                        }

                        EnergiaAtualAliada -= habilidadeUsada.SH.GastoDeHabilidade;
                    if(EnergiaAtualAliada < 0)
                        {
                            EnergiaAtualAliada = 0;
                        }
                    }
                    else
                    {

                        foreach (var a in alvos_)
                        {

                            for (int x = 0; x < habilidadeUsada.SAB._Hits; x++)
                            {


                                float danofinal = habilidadeUsada.SAB.porcentagemDoEfeito *ultimobuffer.data.bufferData.AtaqueFisico* (1+ ultimobuffer.data.level * ultimobuffer.data.bufferData.TaxaDeCrescimentoDoAtaqueBasico);


                            switch (a.data.bufferData.TipoDeEfetividade)
                                {
                                    case Efetividade.blindado:
                                        switch (habilidadeUsada.SAB._TipoDeAtaque)
                                        {
                                            case TipoDeAtaque.Esmagamento:
                                                danofinal *= 2;
                                                break;
                                            case TipoDeAtaque.Corte:
                                                danofinal /= 2;
                                                break;
                                            case TipoDeAtaque.Perfurante:

                                                break;
                                        }
                                        break;
                                    case Efetividade.liso:
                                        switch (habilidadeUsada.SAB._TipoDeAtaque)
                                        {
                                            case TipoDeAtaque.Esmagamento:
                                                danofinal /= 2;
                                                break;
                                            case TipoDeAtaque.Corte:
                                                danofinal *= 2;
                                                break;
                                            case TipoDeAtaque.Perfurante:

                                                break;
                                        }
                                        break;
                                }

                                a.diminuirVida(danofinal);
                            }
                        }
                        EnergiaAtualAliada += habilidadeUsada.SAB.ValorDeRecarga;
                        if(EnergiaAtualAliada > confg.totalDeEnergiaAliada)
                        {
                            EnergiaAtualAliada = confg.totalDeEnergiaAliada;
                        }

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

        public void inimigoEscolherHabilidade()
        {

            int indc = 0;
            int maxh = 0;
            float prob = 0;

            foreach(var a in ultimobuffer.data.habilidades)
            {
                if(a.GastoDeHabilidade < EnergiaAtualInimiga)
                {
                    maxh++;
                }
            }
            habilidadeUsada = new AuxUiHabilidade();
            habilidadeUsada.GB = this;

            if(Random.Range(0,maxh+2) > maxh)
            {
                //atk basico

                habilidadeUsada.SAB = ultimobuffer.data.bufferData.AtaqueBasico;
            }
            else
            {
                //habilidade
                prob = 1 / maxh;


                foreach (var a in ultimobuffer.data.habilidades)
                {
                    if (a.GastoDeHabilidade < EnergiaAtualInimiga)
                    {
                        if (Random.Range(0f, 1f) > prob)
                        {
                            habilidadeUsada.SH = a;
                            break;
                        }
                        
                    }
                }
                if(habilidadeUsada.SH == null)
                {
                    foreach (var a in ultimobuffer.data.habilidades)
                    {
                        if (a.GastoDeHabilidade < EnergiaAtualInimiga)
                        {
                           
                                habilidadeUsada.SH = a;
                                break;
                           

                        }
                    }
                }

            }


          
        }
        public void inimigoEscolherAlvo()
        {
           if(habilidadeUsada.SH != null)
            {
                switch (habilidadeUsada.SH._Efeito) {
                    case Efeito.Dano:
                        switch (habilidadeUsada.SH._Alvo)
                        {
                            case Alvo.Unico:
                                int indc = Random.Range(0, confg.aliadosL.Count);

                                alvos_.Add(ordemBatalha.FindAll(x => x.npc == false)[indc]);
                                break;
                            case Alvo.Global:

                                alvos_.AddRange(ordemBatalha.FindAll(x => x.npc == false));
                                
                                break;
                        }
                        break;
                    case Efeito.MudarStatus:
                        switch (habilidadeUsada.SH._Alvo)
                        {
                            case Alvo.Unico:

                                int indc = Random.Range(0, confg.inimigosL.Count);

                                alvos_.Add(ordemBatalha.FindAll(x => x.npc == true)[indc]);

                                break;
                            case Alvo.Global:
                                alvos_.AddRange(ordemBatalha.FindAll(x => x.npc == true));
                                break;
                        }
                        break;
                    case Efeito.Escudo:
                        switch (habilidadeUsada.SH._Alvo)
                        {
                            case Alvo.Unico:
                                int indc = Random.Range(0, confg.inimigosL.Count);

                                alvos_.Add(ordemBatalha.FindAll(x => x.npc == true)[indc]);
                                break;
                            case Alvo.Global:
                                alvos_.AddRange(ordemBatalha.FindAll(x => x.npc == true));
                                break;
                        }
                        break;
                }
               
             
            }
            else
            {
                
               
                        int indc = Random.Range(0, confg.aliadosL.Count);

                        alvos_.Add(ordemBatalha.FindAll(x => x.npc == false)[indc]);
               
            }
            if (alvos_.Count <= 0)
                Debug.Log("nenhum alvo foi escolido");
        }
        public void InimigousarHabilidadeNoAlvo()
        {
            if (habilidadeUsada.SH != null)
            {
                foreach (var a in alvos_)
                {
                    aplicarHabilidadeEmAlvo(a);
                }
            }
            else
            {
                foreach (var a in alvos_)
                {

                    for (int x = 0; x < habilidadeUsada.SAB._Hits; x++)
                    {


                        float danofinal = habilidadeUsada.SAB.porcentagemDoEfeito * ultimobuffer.data.bufferData.AtaqueFisico * (1 + ultimobuffer.data.level * ultimobuffer.data.bufferData.TaxaDeCrescimentoDoAtaqueBasico);


                        switch (a.data.bufferData.TipoDeEfetividade)
                        {
                            case Efetividade.blindado:
                                switch (habilidadeUsada.SAB._TipoDeAtaque)
                                {
                                    case TipoDeAtaque.Esmagamento:
                                        danofinal *= 2;
                                        break;
                                    case TipoDeAtaque.Corte:
                                        danofinal /= 2;
                                        break;
                                    case TipoDeAtaque.Perfurante:

                                        break;
                                }
                                break;
                            case Efetividade.liso:
                                switch (habilidadeUsada.SAB._TipoDeAtaque)
                                {
                                    case TipoDeAtaque.Esmagamento:
                                        danofinal /= 2;
                                        break;
                                    case TipoDeAtaque.Corte:
                                        danofinal *= 2;
                                        break;
                                    case TipoDeAtaque.Perfurante:

                                        break;
                                }
                                break;
                        }

                        a.diminuirVida(danofinal);
                    }
                }
                EnergiaAtualInimiga += habilidadeUsada.SAB.ValorDeRecarga;
                if (EnergiaAtualInimiga > confg.totalDeEnergiaInimiga)
                {
                    EnergiaAtualInimiga = confg.totalDeEnergiaInimiga;
                }
            }

        }
        public void inimigoAgir()
        {
            if (auxT > tempoQueOInimigoPensa)
            {
               
                    ultimobuffer = ordemBatalha[0];
                    auxT = 0;

                    inimigoEscolherHabilidade();
                    inimigoEscolherAlvo();
                InimigousarHabilidadeNoAlvo();
                    proximo();
                    Debug.Log("foi ?");
              
            }
            else { auxT += Time.deltaTime; }

        }

        private void Update()
        {
            if (confg.aliadosL.Count > 0 && confg.inimigosL.Count >0)
            {

                if (ordemBatalha.Count > 0)
                {
                    if (_statusgame == statusGame.player)
                    {
                        tBug.text = "jogador joga";

                        mostrarHabilidades();
                        if (habilidadeUsada != null)
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
            else
            {
                Debug.Log((confg.aliadosL.Count == 0 ? "inimigo " : "aliado ")+ "  Venceu !!");
            }

            foreach(var a  in ordemBatalha)
            {
                if (a.data.vida <= 0)
                {
                    Destroy(a.obj);
                    if (a.npc)
                    {
                        confg.inimigosL.RemoveAll(x => x.id_ == a.id_);
                    }
                    else
                    {
                        confg.aliadosL.RemoveAll(x => x.id_ == a.id_);
                    }
                
                }
            }
            ordemBatalha.RemoveAll(x => x.obj == null);
            confg.inimigosL.RemoveAll(x => x.obj == null);
            confg.aliadosL.RemoveAll(x => x.obj == null);

        }
    }
    public enum statusGame
    {
        player,maquina,carregando
    }
}
