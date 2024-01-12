using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Neural_2_0 
{
    public List<float> saida,memoria;

    public List<List<float>> multiplicador;
    public minhaconfig config;
    public struct minhaconfig {

        public bool recorrencia;
        public bool independencia;
        public int indice_independencia;
        public void configurar(config_Rede con)
        {
            recorrencia = con.recorrencia;
            independencia = con.independencia;
        }
    }

    public Neural_2_0(int entradas,int tamanhoDoNeuronio, config_Rede conf)
    {
      
        multiplicador = new List<List<float>>();
        saida = new List<float>();
        memoria = new List<float>();
       ;
        for(int x= 0; x < tamanhoDoNeuronio; x++)
        {
            saida.Add(0);
            memoria.Add(0);
        }

        for (int y = 0; y < entradas; y++)
        {
            multiplicador.Add(new List<float>());
         
            for (int x = 0; x < tamanhoDoNeuronio; x++)
            {

                multiplicador[y].Add(Random.Range(-1.1f, 1.1f));
            }
        }
        config.configurar(conf);

        if (config.independencia)
        {
            config.indice_independencia = (int)Random.Range(0, tamanhoDoNeuronio);
        }

    }

    public List<float> processar(List<float> valor)
    {
        List<float> resultador = new List<float>(new float[multiplicador[1].Count]);

        for (int x = 0; x < valor.Count; x++)
        {
            for (int y = 0; y < multiplicador[x].Count; y++)
            {
                resultador[y] = multiplicador[x][y] * valor[x];
            }
        }
        if (config.independencia)
        {
            resultador[config.indice_independencia] = 0;
        }
        if (config.recorrencia)
        {
            List<float> somaF_F = new List<float>();
            List<float> ativacao_ = new List<float>();

            somaF_F = soma( saida , resultador);
            ativacao_ = ativacao(somaF_F);

            memoria = ( multiplicacao(ativacao_, memoria));

            memoria =clampim( soma(memoria, multiplicacao(ativacao_, Tanh(somaF_F))));

          
           
           
            resultador = clampim( ( multiplicacao(ativacao_, Tanh(memoria))));

          
         
        }


        saida.Clear();
        saida.AddRange(resultador);

        return resultador;
    }
    public List<float> ativacao(List<float> values)
    {
       
            List<float> result = new List<float>();
            float sum = (values.Max());

           

          
            foreach (float v in values)
            {
                float probability = v / sum;
                result.Add(probability);
            }

            return result;
        }
    public List<float> Tanh(List<float> entrada)
    {
        List<float> saida = new List<float>();

        foreach (float valor in entrada)
        {
            // Aplicar a função tanh
            
            float ativado = System.MathF.Tanh(valor);
            saida.Add(ativado);
        }

        return saida;
    }
    public List<float> soma (List<float> valorA,List<float> valorB)
    {
        List<float> aux = new List<float>();
        if(valorA.Count != valorB.Count)
        {
            Debug.Log(valorA.Count + " A  " + valorB.Count + "   B");
        }
        for(int x = 0; x < valorA.Count; x++)
        {
            aux.Add(valorA[x] + valorB[x]);
        }
        return aux;
    }
    public List<float> multiplicacao(List<float> valorA, List<float> valorB)
    {
        List<float> aux = new List<float>();
    
        for (int x = 0; x < valorA.Count; x++)
        {
          
            
            if(valorB[x] == 0 || valorA[x] == 0)
            {
                aux.Add(0);
            }
            else
            {
                aux.Add(valorA[x] * valorB[x]);
            }
          
        }
        return aux;
    }
    public List<float> clampim ( List<float> aux_)
    {
        List<float> aux = new List<float>();

        for (int x = 0; x < aux_.Count; x++)
        {
            aux.Add(System.MathF.Round( Mathf.Clamp(aux_[x], -1000,1000),4));
        }
        return aux;
    }
}
[System.Serializable]
public class config_Rede {
    public bool recorrencia;
    public bool independencia;
}
