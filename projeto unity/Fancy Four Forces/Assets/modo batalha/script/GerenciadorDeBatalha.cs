using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static modoBatalha.Configuracao;
using TMPro;
using Unity.Plastic.Antlr3.Runtime.Misc;
using Buffers;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Codice.CM.Common;
using System.Linq;
using log4net;

namespace modoBatalha
{
    public enum dificuldade
    {
        aleatoria,tatica,Neural
    }
    public class GerenciadorDeBatalha : MonoBehaviour
    {
        public Configuracao confg;

        public dificuldade _dificuldade;

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
                // caso seja uma sequencia de aliados isso vai limpar a tela 
                foreach (var a in habilidadesInstanciadas)
                {
                    Destroy(a);
                }
                habilidadesInstanciadas.Clear();

                ultimobuffer = aux;
                // mostrar as habilidades
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

            confg.Gseta.definirOrigem(ultimobuffer.obj.transform.position );
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
            Debug.Log("inimigo esta usando uma habilidae");
            if(habilidadeUsada.SH != null) {
                switch (habilidadeUsada.SH._Efeito)
                {
                    case(Efeito.Dano):
                      
                        float danofinal = ultimobuffer.danoBruto(habilidadeUsada.SH);
                        Debug.Log("a habilidade deu dano de " + danofinal);
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
                        Debug.Log("a habilidade alterou algum status");
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
                        Debug.Log("a habilidade gera um escudo");
                        a.receberEscudo(habilidadeUsada.SH, ultimobuffer.data);
                      //  a.darEscudo(habilidadeUsada.SH, ultimobuffer);
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
                            dir = 2;

                            break;

                        default:
                            dir = -1;
                            break;
                    }

                    switch (habilidadeUsada.SH._Alvo)
                    {
                        case (Alvo.Unico):
                            usarSeta(dir);
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

        public void inimigoEscolherHabilidade(int h)
        {


         
            habilidadeUsada = new AuxUiHabilidade();
            habilidadeUsada.GB = this;

            if (h == 2)
            {
                //atk basico

                habilidadeUsada.SAB = ultimobuffer.data.bufferData.AtaqueBasico;
            }
            else
            {
                //habilidade
                habilidadeUsada.SH = ultimobuffer.data.habilidades[h];

                if (habilidadeUsada.SH.GastoDeHabilidade > EnergiaAtualInimiga)
                {
                    habilidadeUsada.SH = null;
                    habilidadeUsada.SAB = ultimobuffer.data.bufferData.AtaqueBasico;

                }
            }

          
        }
        public void inimigoEscolherAlvo(int h)
        {
            if(habilidadeUsada.SAB != null)
            {

            }
            else
            {
                if (habilidadeUsada.SH._Efeito == Efeito.Dano)
                {
                    if (habilidadeUsada.SH._Alvo == Alvo.Global)
                    {

                        alvos_.AddRange(confg.aliadosL);
                    }
                    else
                    {
                        try
                        {
                            alvos_.Add(confg.aliadosL[h]);
                        }
                        catch { alvos_.Add(confg.aliadosL[0]); }
                    }
                }
                else
                {
                    if (habilidadeUsada.SH._Alvo == Alvo.Global)
                    {
                        alvos_.AddRange(confg.inimigosL);

                    }
                    else
                    {
                        try
                        {
                            alvos_.Add(confg.inimigosL[h]);
                        }
                        catch { alvos_.Add(confg.inimigosL[0]); }
                    }
                }
            }
        }
        public void InimigousarHabilidadeNoAlvo()
        {
            // habilidade
            if (habilidadeUsada.SH != null)
            {
                foreach (var a in alvos_)
                {
                    aplicarHabilidadeEmAlvo(a);
                }
                EnergiaAtualInimiga -= habilidadeUsada.SH.GastoDeHabilidade;
                if (EnergiaAtualInimiga < 0)
                {
                    EnergiaAtualInimiga = 0;
                }
            }
            // ataque basico
            else
            {
                Debug.Log("inimigo usou um ataque basico, a habilidade posui " + habilidadeUsada.SAB._Hits + " hits");
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
                Debug.Log("a energia inimiga foi aumentada  !!");
                EnergiaAtualInimiga += habilidadeUsada.SAB.ValorDeRecarga;
                if (EnergiaAtualInimiga > confg.totalDeEnergiaInimiga)
                {
                    EnergiaAtualInimiga = confg.totalDeEnergiaInimiga;
                }
            }

        }
        public GerenciadorDeRedeNeural NeuralA;
        public void inimigoAgir()
        {
            if (auxT > tempoQueOInimigoPensa)
            {
                if (dificuldade.aleatoria == _dificuldade)
                {

                    ultimobuffer = ordemBatalha[0];
                    auxT = 0;
                    // inicio

                    for (int x = 0; x < 4; x++)
                    {
                        try
                        {
                      
                            NeuralA.entrada[x] = confg.aliadosL[x].data.vida / confg.aliadosL[x].data.vida_maxima;

                        }
                        catch
                        {
                            NeuralA.entrada[x] = 0;

                        }
                    }
                    int offset = 0;
                    for (int x = 0; x < 4; x++)
                    {
                        try
                        {


                            if (offset + x >= 4)
                            {
                                break;
                            }
                            if (confg.inimigosL[x] == ordemBatalha[0])
                            {
                                offset++;

                            }

                            NeuralA.entrada[4 + x] = confg.inimigosL[x + offset].data.vida / confg.inimigosL[x + offset].data.vida_maxima;

                        }
                        catch { NeuralA.entrada[4 + x] = 0; }
                    }

                    // ataque basico

                    NeuralA.entrada[8] = (confg.totalDeEnergiaInimiga / EnergiaAtualInimiga) * 2;

                    //habilidades

                    // habilidade 1                
                    if (ordemBatalha[0].data.habilidades[0]._Efeito == Efeito.Dano)
                    {

                        for (int x = 0; x < 4; x++)
                        {
                            try
                            {
                                float tempf = (ordemBatalha[0].data.habilidades[0].porcentagemDoEfeito * (ordemBatalha[0].data.bufferData.AtaqueFisico +
                                    (ordemBatalha[0].data.level * ordemBatalha[0].data.bufferData.TaxaDeCrescimentoDoAtaqueBasico)))
                                     / confg.aliadosL[x].data.vida;


                                NeuralA.entrada[9 + x] = tempf;

                            }
                            catch { NeuralA.entrada[9 + x] = 0; ; }
                        }
                    }
                    else
                    {
                        float mediaLvl = 0;

                        foreach (var c in confg.aliadosL)
                        {
                            mediaLvl += c.data.level;
                        }
                        mediaLvl /= confg.aliadosL.Count;

                        int offset_ = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            if (offset_ + x >= 4)
                            {
                                NeuralA.entrada[9 + x] = 0;
                                break;
                            }
                          
                            try
                            {
                                if (confg.inimigosL[x] == ordemBatalha[0])
                                {
                                    offset_++;

                                }

                                NeuralA.entrada[9 + x] = confg.inimigosL[x + offset_].data.level / mediaLvl / ordemBatalha[0].data.level;

                            }
                            catch
                            {
                                ;
                                NeuralA.entrada[9 + x] = 0;
                            }


                        }

                    }
                    // lado a = inimigos
                    // habilidade 2
                    if (ordemBatalha[0].data.habilidades[1]._Efeito == Efeito.Dano)
                    {

                        for (int x = 0; x < 4; x++)
                        {
                            float tempf = 0;
                            try
                            {
                                tempf = (ordemBatalha[0].data.habilidades[1].porcentagemDoEfeito * (ordemBatalha[0].data.bufferData.AtaqueFisico +
                                    (ordemBatalha[0].data.level * ordemBatalha[0].data.bufferData.TaxaDeCrescimentoDoAtaqueBasico)))
                                     / confg.aliadosL[x].data.vida;

                            }
                            catch { }

                            NeuralA.entrada[13 + x] = tempf;
                        }
                    }
                    else
                    {
                        float mediaLvl = 0;

                        foreach (var c in confg.aliadosL)
                        {
                            mediaLvl += c.data.level;
                        }
                        mediaLvl /= confg.aliadosL.Count;

                        int offset_ = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            if (offset_ + x >= 4)
                            {
                                NeuralA.entrada[13 + x] = 0;
                                break;
                            }
                            try
                            {
                                if (confg.inimigosL[x].data == ordemBatalha[0].data)
                                {
                                    offset_++;

                                }

                                NeuralA.entrada[13 + x] = confg.inimigosL[x + offset_].data.level / mediaLvl / ordemBatalha[0].data.level;

                            }
                            catch
                            {
                                NeuralA.entrada[13 + x] = 0;

                            }

                        }

                    }

                    NeuralA.processarEntrada(NeuralA.entrada, NeuralA.saida);
                    List<float> saida= Activation(NeuralA.saida);
                    bool tempEscolheu = false;
                    int habilidade_ = 0;
                    int alvo = 0;
                    for (int x = 0; x < 4; x++)
                    {
                        if (tempEscolheu)
                            break;
                        int tempI = x;
                        if (saida[tempI] > 0)
                        {
                            tempEscolheu = true;
                            habilidade_ = 0;
                            alvo = x;
                            break;
                        }

                    }
                    for (int x = 0; x < 4; x++)
                    {
                        if (tempEscolheu)
                            break;
                        int tempI = 4 + x;
                        if (saida[tempI] > 0)
                        {
                            tempEscolheu = true;
                            habilidade_ = 1;
                            alvo = x;
                            break;
                        }
                    }
                    for (int x = 0; x < 4; x++)
                    {
                        if (tempEscolheu)
                            break;
                        int tempI = 8 + x;
                        if (saida[tempI] > 0)
                        {

                            habilidade_ = 2;
                            alvo = x;
                            break;
                        }

                    }

                    // fim
                    Debug.Log("Inimigo usou a habilidade " + habilidade_ + "   no alvo " + alvo);   
                    inimigoEscolherHabilidade(habilidade_);
                    inimigoEscolherAlvo(alvo);
                    InimigousarHabilidadeNoAlvo();
                }
                
                    proximo();
                    Debug.Log("foi ?");
              
            }
            else { auxT += Time.deltaTime; }

        }
        public List<float> Activation(List<float> input)
        {
            // Encontra o maior valor da lista
            float max = input.Max();

            // Cria uma nova lista com o mesmo tamanho da lista de entrada
            List<float> output = new List<float>();
            foreach (var a in input)
            {
                output.Add(0);
            }

            // Preenche a nova lista com 0 ou 1
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] == max)
                {
                    output[i] = 1;
                    break;
                }
            }

            return output;
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
          
                if(confg.aliadosL.Count == 0)
                {
                    SceneManager.LoadScene("SampleScene");
                    // voltar para a tela anterior
                }
                else
                {
                    foreach (var a in confg.dataBatalha.inimigos)
                    {

                        inventario.Inventario.Add(new Kernel(a));
                        Debug.Log("Buffer " + a.bufferData.Nome + "  foi adicionado no inventario");
                    }

          

                

                    SceneManager.LoadScene("SampleScene");


                }
             
            }

            foreach(var a  in ordemBatalha)
            {
                if (a.data.vida <= 0)
                {
                     Destroy(a.obj,1f);
                    if (a.npc)
                    {
                //   confg.inimigosL.RemoveAll(x => x.id_ == a.id_);
                    }
                    else
                    {
                  //  confg.aliadosL.RemoveAll(x => x.id_ == a.id_);
                    }
                
                }
            }

          
             ordemBatalha.RemoveAll(x => x.obj == null);
             confg.inimigosL.RemoveAll(x => x.obj == null);
             confg.aliadosL.RemoveAll(x => x.obj == null);
          //  ordemBatalha.RemoveAll(x => x.data.vida <= 0);
          //  confg.inimigosL.RemoveAll(x => x.data.vida <= 0);
          //  confg.aliadosL.RemoveAll(x => x.data.vida <= 0);
        }

        public ScriptavelInventario inventario;
    }
    
    public enum statusGame
    {
        player,maquina,carregando
    }
}
