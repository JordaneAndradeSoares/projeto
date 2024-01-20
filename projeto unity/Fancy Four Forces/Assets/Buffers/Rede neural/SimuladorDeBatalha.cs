using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace Buffers
{
    public class SimuladorDeBatalha : MonoBehaviour
    {

        /*
         * a rede deve analisar qual habilidade usar e em quem usar
         *
         *
         *
         */

        public ScriptavelBatalhaBuffer DataBatalha;
        public ClusterEvolucoes cle;
        public List<ScriptavelHabilidades> listaDeHabilidades;


        public List<Kernel> ordemDeCriaturas;
        public List<Kernel> LadoA, LadoB;
        public GerenciadorDeRedeNeural NeuralA, NeuralB;
        public List<float> saida;
        public bool QuemJoga;
        
        [Space]
      public int errosLadoA,errosLadoB;
        public int Jogos_, vitoriaA, vitoriaB;
        [Space]

        public float energiaAtualLadoA, energiaMaximaLadoA, energiaAtualLadoB, energiaMaximaLadoB;
        public void iniciar()
        {
            LadoA = new List<Kernel>();
            LadoB = new List<Kernel>();
            ordemDeCriaturas = new List<Kernel>();
            List<Kernel> temp = new List<Kernel>();

            for(int x= 0; x < DataBatalha.aliados.Count; x++)
            {
                DataBatalha.aliados[x].bufferData = cle.TodasAsEvolucoes[UnityEngine.Random.Range(0, cle.TodasAsEvolucoes.Count)].BufferData;
                DataBatalha.aliados[x].level = UnityEngine.Random.Range(5, 40);
                DataBatalha.aliados[x].habilidades.Clear();
                DataBatalha.aliados[x].habilidades.Add(listaDeHabilidades[UnityEngine.Random.Range(0, listaDeHabilidades.Count)]);
                DataBatalha.aliados[x].habilidades.Add(listaDeHabilidades[UnityEngine.Random.Range(0, listaDeHabilidades.Count)]);
            }
            for (int x = 0; x < DataBatalha.inimigos.Count; x++)
            {
                DataBatalha.inimigos[x].bufferData = cle.TodasAsEvolucoes[UnityEngine.Random.Range(0, cle.TodasAsEvolucoes.Count)].BufferData;
                DataBatalha.inimigos[x].level = UnityEngine.Random.Range(5, 40);
                DataBatalha.inimigos[x].habilidades.Clear();
                DataBatalha.inimigos[x].habilidades.Add(listaDeHabilidades[UnityEngine.Random.Range(0, listaDeHabilidades.Count)]);
                DataBatalha.inimigos[x].habilidades.Add(listaDeHabilidades[UnityEngine.Random.Range(0, listaDeHabilidades.Count)]);
            }

            foreach (var a in DataBatalha.aliados)
            {
                LadoA.Add((a));
                temp.Add(a);
                energiaMaximaLadoA += a.bufferData.Estamina + (a.bufferData.TaxaDeCrescimentoDaEstamina * a.level);
                energiaAtualLadoA = energiaMaximaLadoA;
            }
            foreach (var a in DataBatalha.inimigos)
            {
                LadoB.Add(a);
                temp.Add((a));

                energiaMaximaLadoB += a.bufferData.Estamina + (a.bufferData.TaxaDeCrescimentoDaEstamina * a.level);
                energiaAtualLadoB = energiaMaximaLadoB;

            }

            foreach(var a in temp)
            {
                a.velocidade = a.bufferData.Velocidade *
                       (1 + (a.bufferData.TaxaDeCrescimentoDaVelocidade * a.level));
            }

            while(temp.Count > 0)
            {
                var a = temp[0];

                foreach (var b in temp)
                {
                    if (b.velocidade > a.velocidade)
                    {
                        a = b;
                    }
                }
                ordemDeCriaturas.Add(a);
                temp.Remove(a);
            }

            Jogos_++;


        }
        public void terminouAJogada()
        {
            QuemJoga = !QuemJoga;
        }
        private void Start()
        {
            iniciar();
            NeuralA.carregar_();
            NeuralB.carregar_();
        }
        // 12 saidas { cada skil tem 4 opções de alvos
        // entradas ?
        /*
         *  a porcentagem da vida de cada adversario e aliado   7 
         *  a diferença de lvl de aliados e inimigos            7
         *  
         *  o que é eficiencia ? eficien é conseguir o melhor resulta com o menor gasto.
         *  
         *  eficiencia de uma habilidade seria causar a maior quantidade de dano com o menor gasto, no entanto
         *  tambem existem habilidades de apoio, o ideal seria vetorizar as possibilidades se dc a habilidade de apoio,
         *  mas isso criaria um loop se o alido tbm tivc uma habilidade de apoio. então devo pensar o contrario,
         *  calcular a probabilidade que eu tenho de vitoria contra as do meu aliado, caso o meu apoio crie melhor
         *  vantagem do que as que eu tenho, talves seja melhor dar apoio.
         *  
         *  no entanto isso tambem seria falho vejamos o exemplo:
         *  se causar mesmo que pequeno 1 de dano.
         *  mas meu aliado que é mais forte que eu causaria 2 de dano.
         *  
         *  caso eu de apoio para ele ele causara 2.5 de dano, que é menos do que se tvc escolido atacar.
         *  este exemplo é simples de entender com numeros de dano, mas não sera possivel fazer na pratica.
         *  
         *  o que me resta calcular baseandoc no (nvl do aliado aliados vs a media do lvl inimigo  ) vs
         *  (meu nvl vs media do nvl dos inimigos), caso o resultado seja maior que o dobro da minha diferença eu devo dar apoio
         * 
         *  
         */
        Kernel origem, destino ; ScriptavelHabilidades habilidade;
   
        public void aplicarHabilidade(Kernel origem, Kernel destino , int habilidade,bool a)
        {
            if(habilidade == 2)
            {
        float valorfinal = 0;
        valorfinal += origem.bufferData.AtaqueBasico.porcentagemDoEfeito * (origem.bufferData.AtaqueFisico + (origem.level * origem.bufferData.TaxaDeCrescimentoDoAtaqueBasico));
        
        destino.vida -= valorfinal;

                if (a)
                {
                    energiaAtualLadoA += origem.bufferData.AtaqueBasico.ValorDeRecarga;
                }
                else
                {
                    energiaAtualLadoB += origem.bufferData.AtaqueBasico.ValorDeRecarga;
                }

            }
            else
            {
                bool falha = false;
                if (a)
                {
                    if (energiaAtualLadoA < origem.habilidades[habilidade].GastoDeHabilidade)
                        falha = true;
                }
                else
                {
                    if (energiaAtualLadoB < origem.habilidades[habilidade].GastoDeHabilidade)
                        falha = true;
                }
                if (falha == false)
                {

                    if (origem.habilidades[habilidade]._Efeito == Efeito.Dano)
                    {
                        float valorfinal = 0;
                        valorfinal += origem.habilidades[habilidade].porcentagemDoEfeito * (origem.bufferData.AtaqueFisico + (origem.level * origem.bufferData.TaxaDeCrescimentoDoAtaqueBasico));

                        destino.vida -= valorfinal;
                    }
                    else
                    {
                        if (origem.habilidades[habilidade]._Efeito == Efeito.MudarStatus)
                        {
                            destino.level++;
                        }
                        else
                        {
                            destino.vida += 1;
                        }
                    }
                    if (a)
                    {
                        energiaAtualLadoA -= origem.habilidades[habilidade].GastoDeHabilidade;
                    }
                    else
                    {
                        energiaAtualLadoB -= origem.habilidades[habilidade].GastoDeHabilidade;
                    }
                }
            }

           

        }

        

        public void calcularResposta(bool a)
        {

            if (a)
            {
                for (int x = 0; x < 4; x++)
                {
                    try
                    {
                        NeuralA.entrada[x] = LadoB[x].vida / LadoB[x].vida_maxima;
                       
                    }
                    catch { NeuralA.entrada[x] = 0;
                     
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
                        if (LadoA[x] == ordemDeCriaturas[0])
                        {
                            offset++;

                        }

                        NeuralA.entrada[4 + x] = LadoA[x + offset].vida / LadoA[x + offset].vida_maxima;
                       
                    }
                    catch { NeuralA.entrada[4 + x] = 0;  }
                }

                // ataque basico

                NeuralA.entrada[8] = (energiaMaximaLadoA / energiaAtualLadoA) *2;

                //habilidades

                // habilidade 1                
                if(ordemDeCriaturas[0].habilidades[0]._Efeito == Efeito.Dano)
                {

                    for (int x = 0; x < 4; x++)
                    {
                        try
                        {
                            float tempf = (ordemDeCriaturas[0].habilidades[0].porcentagemDoEfeito * (ordemDeCriaturas[0].bufferData.AtaqueFisico +
                                (ordemDeCriaturas[0].level * ordemDeCriaturas[0].bufferData.TaxaDeCrescimentoDoAtaqueBasico)))
                                 / LadoB[x].vida;


                            NeuralA.entrada[9 + x] = tempf;
                        
                        }
                        catch { NeuralA.entrada[9 + x] = 0;  ; }
                    }
                }
                else
                {
                    float mediaLvl = 0;

                    foreach(var c in LadoB)
                    {
                        mediaLvl += c.level;
                    }
                    mediaLvl /= LadoB.Count;

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
                            if (LadoA[x] == ordemDeCriaturas[0])
                        {
                            offset_++;

                        }
                       
                            NeuralA.entrada[9 + x] = LadoA[x + offset_].level / mediaLvl / ordemDeCriaturas[0].level;
                          
                        }
                        catch
                        {
                             ;
                            NeuralA.entrada[9 + x] = 0;
                        }


                    }

                }
                // habilidade 2
                if (ordemDeCriaturas[0].habilidades[1]._Efeito == Efeito.Dano)
                {

                    for (int x = 0; x < 4; x++)
                    {
                        float tempf = 0;
                        try
                        {
                            tempf = (ordemDeCriaturas[0].habilidades[1].porcentagemDoEfeito * (ordemDeCriaturas[0].bufferData.AtaqueFisico +
                                (ordemDeCriaturas[0].level * ordemDeCriaturas[0].bufferData.TaxaDeCrescimentoDoAtaqueBasico)))
                                 / LadoB[x].vida;
                            
                        }
                        catch {   }

                        NeuralA.entrada[13 + x] = tempf;
                    }
                }
                else
                {
                    float mediaLvl = 0;

                    foreach (var c in LadoB)
                    {
                        mediaLvl += c.level;
                    }
                    mediaLvl /= LadoB.Count;

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
                            if (LadoA[x] == ordemDeCriaturas[0])
                            {
                                offset_++;

                            }

                            NeuralA.entrada[13 + x] = LadoA[x + offset_].level / mediaLvl / ordemDeCriaturas[0].level;
                          
                        }
                        catch
                        {
                            NeuralA.entrada[13 + x] = 0;
                             
                        }

                    }

                }

                NeuralA.processarEntrada(NeuralA.entrada, NeuralA.saida);
                saida = NeuralA.saida;
            }
            else
            {

                for (int x = 0; x < 4; x++)
                {
                    try
                    {
                        NeuralB.entrada[x] = LadoA[x].vida / LadoA[x].vida_maxima;
                    
                    }
                    catch
                    {
                        NeuralB.entrada[x] = 0;
                      
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
                        if (LadoB[x] == ordemDeCriaturas[0])
                        {
                            offset++;

                        }

                        NeuralB.entrada[4 + x] = LadoB[x + offset].vida / LadoB[x + offset].vida_maxima;
                   
                    }
                    catch
                    {
                        NeuralB.entrada[4 + x] = 0;
                        
                    }
                }

                // ataque basico

                NeuralB.entrada[8] = (energiaMaximaLadoB / energiaAtualLadoB) * 2;

                //habilidades

                // habilidade 1
                if (ordemDeCriaturas[0].habilidades[0]._Efeito == Efeito.Dano)
                {

                    for (int x = 0; x < 4; x++)
                    {
                        float tempf = 0;
                        try
                        {
                            tempf = (ordemDeCriaturas[0].habilidades[0].porcentagemDoEfeito * (ordemDeCriaturas[0].bufferData.AtaqueFisico +
                                (ordemDeCriaturas[0].level * ordemDeCriaturas[0].bufferData.TaxaDeCrescimentoDoAtaqueBasico)))
                                 / LadoA[x].vida;
                         
                        }
                        catch
                        {
                            
                        }

                        NeuralB.entrada[9+ x] = tempf;
                    }
                }
                else
                {
                    float mediaLvl = 0;

                    foreach (var c in LadoA)
                    {
                        mediaLvl += c.level;
                    }
                    mediaLvl /= LadoA.Count;

                    int offset_ = 0;
                    for (int x = 0; x < 4; x++)
                    {
                        if (offset_ + x >= 4)
                        {
                            NeuralB.entrada[9 + x] = 0;
                            break;
                        }
                        try
                        {
                            if (LadoB[x] == ordemDeCriaturas[0])
                            {
                                offset_++;

                            }

                            NeuralB.entrada[9 + x] = LadoB[x + offset_].level / mediaLvl / ordemDeCriaturas[0].level;
                         
                        }
                        catch
                        {
                            NeuralB.entrada[9 + x] = 0;
                            
                        }

                    }

                }

                // habilidade 2
                if (ordemDeCriaturas[0].habilidades[1]._Efeito == Efeito.Dano)
                {

                    for (int x = 0; x < 4; x++)
                    {
                        float tempf = 0;
                        try
                        {
                            tempf = (ordemDeCriaturas[0].habilidades[1].porcentagemDoEfeito * (ordemDeCriaturas[0].bufferData.AtaqueFisico +
                                (ordemDeCriaturas[0].level * ordemDeCriaturas[0].bufferData.TaxaDeCrescimentoDoAtaqueBasico)))
                                 / LadoA[x].vida;
                          
                        }
                        catch
                        {
                            
                        }

                        NeuralB.entrada[13 + x] = tempf;
                    }
                }
                else
                {
                    float mediaLvl = 0;

                    foreach (var c in LadoA)
                    {
                        mediaLvl += c.level;
                    }
                    mediaLvl /= LadoA.Count;

                    int offset_ = 0;
                    for (int x = 0; x < 4; x++)
                    {
                        if (offset_ + x >= 4)
                        {
                            NeuralB.entrada[13 + x] = 0;
                            break;
                        }
                        try
                        {
                            if (LadoB[x] == ordemDeCriaturas[0])
                            {
                                offset_++;

                            }
                      

                        NeuralB.entrada[13 + x] = LadoB[x + offset_].level / mediaLvl / ordemDeCriaturas[0].level;
                       
                        }
                        catch
                        {
                            NeuralB.entrada[13 + x] = 0;
                            
                        }

                    }

                }

                NeuralB.processarEntrada(NeuralB.entrada, NeuralB.saida);
                saida = NeuralB.saida;

            }
            saida = Activation(saida);

            bool tempEscolheu = false;
            int habilidade_ = 0;
            int alvo = 0;
            for(int x = 0; x < 4; x++)
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
            for(int x = 0; x < 4; x++)
            {
                if (tempEscolheu)
                    break;
                int tempI = 4 + x;
                if (saida[tempI] > 0)
                {
                    tempEscolheu = true;
                    habilidade_ =1;
                    alvo = x;
                    break;
                }
            }
            for(int x = 0; x < 4; x++)
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
            if (a)
            {
                if (alvo > LadoB.Count)
                    errosLadoA++;
            }
            else
            {
                if (alvo > LadoA.Count)
                    errosLadoB++;
            }
            aplicarHabilidade(ordemDeCriaturas[0], (a ? LadoB[alvo < LadoB.Count ? alvo:0] : LadoA[alvo < LadoA.Count ? alvo : 0]),habilidade_,a) ;
            terminouAJogada();
            jogadas++;
            
        }
        public float time_;
        float ctime;
        public int jogadas = 0;
        public void verificarGanhador()
        {
            LadoB.RemoveAll(x => x.vida <= 0);
            LadoA.RemoveAll(x => x.vida <= 0);

            if(LadoB.Count ==0|| LadoA.Count == 0)
            {
                temhosGanhador = true;
            }
        }
        bool temhosGanhador = false;



        public bool pause = false;
        private void Update()
        {
            if (false == false)
            {
                if (temhosGanhador == false)
                {

                    if (ctime > time_)
                    {
                        ctime = 0;
                        calcularResposta(QuemJoga);
                        verificarGanhador();
                    }
                    else
                    {
                        ctime += Time.deltaTime;
                    }
                }
                else
                {
                    //botar no perdedor uma mutação do vencedor;

                    if (LadoA.Count == 0)
                    {
                        Debug.Log("lado B ganhou !!");
                        vitoriaB++;
                        reproduzir(NeuralB, NeuralA);
                    }
                    else
                    {
                        vitoriaA++;
                        Debug.Log("lado A ganhou !!");
                        reproduzir(NeuralA, NeuralB);
                    }
                }

                if (jogadas > 50)
                {
                    pause = true;
                    jogadas = 0;

                    NeuralA.mutar(300);
                    NeuralB.mutar(300);
                    reiniciar();
                   
                }
            }
        }
        public void reproduzir(GerenciadorDeRedeNeural deA,GerenciadorDeRedeNeural paraB)
        {
            pause = true;
            temhosGanhador = false;
            deA.savlar();
            paraB.carregar_();
            paraB.mutar(500);
            reiniciar();
        }
        public void reiniciar()
        {
            iniciar();

            errosLadoB =0;
            errosLadoA = 0;
        }
        public List<float> Activation(List<float> input)
        {
            // Encontra o maior valor da lista
            float max = input.Max();

            // Cria uma nova lista com o mesmo tamanho da lista de entrada
            List<float> output = new List<float>();
            foreach(var a in input)
            {
                output.Add(0);
            }

            // Preenche a nova lista com 0 ou 1
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] == max)
                {
                    output[i] =1;
                    break;
                }
            }

            return output;
        }
    }
}
