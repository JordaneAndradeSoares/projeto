using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GerenciadorDeRedeNeural : MonoBehaviour
{
    public List<float> entrada, saida;
    public List<instanciaMatriz> matriz;
    public config_Rede configuracoes;

    [System.Serializable]
    public struct instanciaMatriz {
        [SerializeField]
        public Neural_2_0 rede;
        public int tamanho;
         

    
    }
    public void instanciarRede()
    {
        for (int x = 0; x < matriz.Count; x++)
        {
            if (x == 0)
            {
                instanciaMatriz temp = matriz[x];
               
                temp.rede = new Neural_2_0( entrada.Count, temp.tamanho, configuracoes);
                matriz[x] = temp;
            }
            else
            {
                instanciaMatriz temp = matriz[x];
                temp.rede = new Neural_2_0(matriz[x - 1].tamanho, temp.tamanho, configuracoes);
                matriz[x] = temp;
            }
        }
    }
    public void processarEntrada(List<float> ent_, List<float> said_)
    {
        List<float> aux = new List<float>();
        aux.AddRange(ent_);
        foreach(instanciaMatriz mt in matriz)
        {
           aux = mt.rede.processar(aux);
        }
        said_.Clear();
        said_.AddRange(aux);
    }
    private void Start()
    {
     instanciarRede();

        if (carregar)
        {
            carregar = false;

            carregar_();
        }
    }
    float tt,tt_;

   
   /*
    private void Update()
    {
        // testar rede
        if (false == true)
        {
            if (tt_ + 1 < Time.time)
            {
                tt_ = Time.time;

                processarEntrada(entrada, saida);

            }

            if (tt + 4 < Time.time)
            {
                tt = Time.time;

                for (int x = 0; x < entrada.Count; x++)
                {
                    entrada[x] = Random.Range(-1000, 1000);
                }
            }
        }


      
    }*/
    public string nomeSerial;
    public bool serialize,carregar;

    public void savlar()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath;
        //FileStream file = File.Create(path + "/SalvarDados/redeNeuralModoBatalha"+gameObject.name + ".save");
    
        FileStream file = File.Create(Application.dataPath + @"/Buffers/Rede neural/Cerebro/RedeNeural.save");
        bf.Serialize(file, matriz);
        file.Close();

    }
    public List<instanciaMatriz> carregar_()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath;
        FileStream file;

        if (File.Exists(Application.dataPath + @"/Buffers/Rede neural/Cerebro/RedeNeural.save"))
        {
            file = File.Open(Application.dataPath + @"/Buffers/Rede neural/Cerebro/RedeNeural.save", FileMode.Open);
            List<instanciaMatriz> l = (List<instanciaMatriz>)bf.Deserialize(file);
            file.Close();
            return l;
        }
        Debug.Log("erro não uma rede neural");
        return new List<instanciaMatriz>();
    }


    public void mutar(int c)
    {
        foreach(var a in matriz)
        {
            foreach(var b in a.rede.multiplicador)
            {
              for(int x = 0; x < b.Count; x++)
                {
                    b[x] += Random.Range(-c,c);
                }
            }
        }
    }
}

[System.Serializable()]
public class serializarRede : ISerializable
{

    public List<List<List<float>>> neuroniosSalvos = new List<List<List<float>>>();
    
   


    public serializarRede() { }

   
    public serializarRede(SerializationInfo info, StreamingContext ctxt)
    {
        
        neuroniosSalvos = (List<List<List<float>>>)info.GetValue("rede", typeof(string));
       
      
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {

            info.AddValue("rede", neuroniosSalvos);
           
      
    } 
}