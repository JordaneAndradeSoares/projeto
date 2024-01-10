using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static modoBatalha.Configuracao;
using static UnityEngine.GraphicsBuffer;
using Buffers;
using System.Net.NetworkInformation;
using ValoresGlobais;
using UnityEngine.UI;
using TMPro;
using Codice.Client.BaseCommands;
using PlasticGui.WorkspaceWindow.Items.LockRules;
using UnityEngine.SceneManagement;

namespace modoBatalha
{
    public class Configuracao : MonoBehaviour
    {
 
   
        public Transform aliados, inimigos, seta;

        public float espasamentoentreSi,AlturaNomes;
        public GameObject prefabVida, prefabNome,prefabEscudoBarra,prefabBolaDeEscudo,
            ModeloSeta;

        public MoverSeta Gseta;




        public ScriptavelBatalhaBuffer dataBatalha;

        public List<buffer_s> aliadosL = new List<buffer_s>();
        public List<buffer_s> inimigosL = new List<buffer_s>();

        public List<buffer_s> ordemBatalha = new List<buffer_s>();
        public GerenciadorDeBatalha gbatalha;
        [Space()]
        public RectTransform UI_CapacidadeMaxima, UI_valor_1, UI_valor_2;
        public float UI_tamanhoX_energiaMaxima;

        // aliados
        public float totalDeEnergiaAliada;


        // inimigos

        public float totalDeEnergiaInimiga;



        [System.Serializable]
        public class buffer_s
        {
            public GameObject obj;
            public Vector3 local;
            public Kernel data;
            public bool npc;
            public GerenciadorAttVida gvd; //    gerenciador de vida
           

            public GameObject PreFabBarraDeEscudo;

            public GameObject nome_;
            public float ruido;
            public int id_;

            public float velocidade;
            public buffer_s(int id, float ruido_)
 
            {
                local = Vector3.zero;
                obj = null;

                ruido = ruido_;

                id_ = id;
            }
       
           
            public float defesaFinal()
            {
 
               
                 Debug.Log("calculo da defesa");     
                Debug.Log("defesa fisica = " +data.bufferData.DefesaFisica +" "+
                     "taxa de crescimento vezes o lvl = " + (data.bufferData.TaxaDeCrescimentoDaDefesaFisica * data.level)+
                     "  resultado  = "+ (data.bufferData.DefesaFisica * (1 + (data.bufferData.TaxaDeCrescimentoDaDefesaFisica * data.level)))

                     );

                
                return (data.bufferData.DefesaFisica * (1 + (data.bufferData.TaxaDeCrescimentoDaDefesaFisica * data.level))) + ruido;
 
                
                    return (data.bufferData.DefesaFisica * (data.bufferData.TaxaDeCrescimentoDaDefesaFisica * data.level));
              
            }
            public void receberEscudo(ScriptavelHabilidades hbl,Kernel origem)
            {
              

                data.escudos.Add(new Kernel.escudo(origem, hbl.porcentagemDoEfeito *
                    (origem.bufferData.AtaqueFisico * (origem.bufferData.TaxaDeCrescimentoDoAtaqueBasico *
                    origem.level)), hbl.___DuracaoDeTurno));
                GameObject temp_ = Instantiate(PreFabBarraDeEscudo, this.gvd.GetComponent<GerenciadorAttVida>().Lista_.transform);

                data.escudos[data.escudos.Count - 1].Ges = temp_;

            }
            public float danoBruto(ScriptavelHabilidades hbl)
            {
                float temp = 0;

                // dano normal

                temp = hbl.porcentagemDoEfeito * data.bufferData.AtaqueFisico * (1 + (data.level* data.bufferData.TaxaDeCrescimentoDoAtaqueBasico));
                // bonus de sinergia
            
                if (hbl._TipoDeAtaque == data.bufferData.AtaqueBasico._TipoDeAtaque)
                {
                    temp *= 1.2f;
                }
             
                return temp+ruido;
            }
 
 
            public void diminuirVida(float danoBruto)
            {



                float temp = danoBruto - defesaFinal();



                if (temp > 0)
                {

                  Debug.Log("dano aplicado: " + temp + "foi perdido " + ((temp/data.vida_maxima))+"%  da vida");
                    try
                    {
                        foreach (var a in data.escudos)
                        {
                            if (a.escudo_ > temp)
                            {
                                a.escudo_ -= temp;
                                temp = 0;
                                break;
                            }
                            else
                            {

                                temp -= a.escudo_;
                                a.escudo_ = 0;
                            }

                            if (temp <= 0)
                            {
                                temp = 0;
                                break;
                            }
                        }
                    }
                    catch { }
                    foreach(var a in data.escudos)
                    {
                        if(a.escudo_ <= 0)
                        {
                            Destroy(a.Ges);
                        }
                    }
                    data.escudos.RemoveAll(x => x.escudo_ <= 0);

                    data.vida -= temp;
                    if (data.vida < 0)
                        data.vida = 0;
                }
                else
                {
                    Debug.Log("não foi aplicado dano, o FINAL foi de " + temp);
                }

                if (temp > 0)
                {
                    if (data.escudos.Count == 0)
                    {

                        data.vida -= temp;
                    }
                    else
                    {
                       
                        foreach (var a in data.escudos)
                        {
                            if (a.escudo_ > temp)
                            {
                                a.escudo_ -= temp;
                                break;
                            }
                            else
                            {
                                temp -= a.escudo_;
                                a.escudo_ = 0;
                            }

                        }
                        data.escudos.RemoveAll(x=>x.escudo_ == 0);

                    }
                } 

            }

            public void modificarStatus(StatusAAlterar t, float valor)
            {
                switch (t)
                {
                    case StatusAAlterar.Velocidade:
                        //   data.vida
                        break;
                }
            }
          
        }
        private void Start()
        {
            
            aliadosL.Clear();
            inimigosL.Clear();
            setaAliada = 0; setaInimiga = 0;
            if (dataBatalha)
            {
                List<buffer_s> tempList = new List<buffer_s>();

                for (int x = 0; x < dataBatalha.aliados.Count; x++)
                {

                    
                    aliadosL.Add(new buffer_s(x, UnityEngine.Random.Range(-3f, 3f)));
                    aliadosL[x].data = new Kernel(dataBatalha.aliados[x]);

                    aliadosL[x].obj = Instantiate(dataBatalha.aliados[x].bufferData.modelo_3D, aliados);


                    GameObject gvd_ = Instantiate(prefabVida, aliadosL[x].obj.transform);
                    gvd_.transform.position = aliadosL[x].obj.transform.position + Vector3.up;
                    aliadosL[x].gvd = gvd_.GetComponent<GerenciadorAttVida>();


                    GameObject nome__ = Instantiate(prefabNome, aliadosL[x].obj.transform);
                    nome__.transform.position = aliadosL[x].obj.transform.position + Vector3.up * 2*((dataBatalha.aliados.Count-x) * AlturaNomes);
                    nome__.GetComponent<GerenciadorAttVida>().nome.text = "" +
                        aliadosL[x].data.bufferData.Nome + " [ Lvl." + aliadosL[x].data.level + "]";

                    aliadosL[x].PreFabBarraDeEscudo = prefabEscudoBarra;

                    aliadosL[x].nome_ = nome__;



                   

                    tempList.Add(aliadosL[x]);
                }
                for (int x = 0; x < dataBatalha.inimigos.Count; x++)
                {


                    inimigosL.Add(new buffer_s(x, UnityEngine.Random.Range(-3f, 3f)));
                    inimigosL[x].data = new Kernel(dataBatalha.inimigos[x]);
                    inimigosL[x].obj = Instantiate(dataBatalha.inimigos[x].bufferData.modelo_3D, inimigos);



                    GameObject gvd_ = Instantiate(prefabVida, inimigosL[x].obj.transform);
                    gvd_.transform.position = inimigosL[x].obj.transform.position + Vector3.up;
                    inimigosL[x].gvd = gvd_.GetComponent<GerenciadorAttVida>();

                  


                    inimigosL[x].npc = true;


                    GameObject nome__ = Instantiate(prefabNome, inimigosL[x].obj.transform);
                    nome__.transform.position = inimigosL[x].obj.transform.position + Vector3.up * 2;
                    nome__.GetComponent<GerenciadorAttVida>().nome.text = "" +
                        inimigosL[x].data.bufferData.Nome + " [Lvl." + inimigosL[x].data.level + "]";
                    inimigosL[x].nome_ = nome__;

                    inimigosL[x].PreFabBarraDeEscudo = prefabEscudoBarra;


                    //  temp_.att(dataBatalha.inimigos[x].retorno());


                    tempList.Add(inimigosL[x]);
                }

                /*
                   for (int i = tempList.Count - 1; i >= 1; i--)
                   {
                       // Encontra o maior elemento entre i e o início da lista.
                       int largestIndex = i;
                       for (int j = 0; j < i; j++)
                       {
                           if (tempList[largestIndex].data.bufferData.Velocidade * (1 + (tempList[largestIndex].data.bufferData.TaxaDeCrescimentoDaVelocidade * tempList[largestIndex].data.level))
                               <
                               tempList[j].data.bufferData.Velocidade * (1 + (tempList[j].data.bufferData.TaxaDeCrescimentoDaVelocidade * tempList[j].data.level)))
                           {
                               largestIndex = j;
                           }
                       }

                       // Troca o elemento em i com o maior elemento encontrado.
                       buffer_s temp = tempList[i];
                       tempList[i] = tempList[largestIndex];
                       tempList[largestIndex] = temp;
                   }
                */

                foreach (var a in tempList)
                {
                    Debug.Log(a.data.bufferData.Nome);
                    a.velocidade = a.data.bufferData.Velocidade *
                        (1 + (a.data.bufferData.TaxaDeCrescimentoDaVelocidade * a.data.level));
                }
                while (tempList.Count > 0)
                {
                    var a = tempList[0];

                    foreach(var b in tempList)
                    {
                        if(b.velocidade > a.velocidade)
                        {
                            a = b;
                        }
                    }
                    ordemBatalha.Add(a);
                    tempList.Remove(a);
                }



                foreach (var a in aliadosL)
                {
                    if (!a.obj)
                        continue;
                    a.obj.transform.position = a.local;
                }
                foreach (var a in inimigosL)
                {
                    if (!a.obj)
                        continue;
                    a.obj.transform.position = a.local;
                }


                gbatalha.iniciar(ordemBatalha);


                Vector3 tempTVLM = UI_CapacidadeMaxima.sizeDelta;
                UI_valor_2.sizeDelta = tempTVLM;
                UI_tamanhoX_energiaMaxima = tempTVLM.x;

                attEnergia();

                gbatalha.EnergiaAtualAliada = totalDeEnergiaAliada;
                gbatalha.EnergiaAtualInimiga = totalDeEnergiaInimiga;

            }

           

        }

        public int setaAliada = 0, setaInimiga = 0, aliador_inimigo = -10;

        public void moverseta()
        {
            ModeloSeta.SetActive( true);
            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.paraFrente))
            {
                horizontal(-1);
            }
            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.paraTras))
            {
                horizontal(1);
            }



            if (aliador_inimigo < 0)
            {
                if (setaAliada > aliadosL.Count - 1)
                    setaAliada = 0;
                seta.position = aliadosL[setaAliada].obj.transform.position + (Vector3.up * 2);
                if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.confirmar))
                {
                    gbatalha.escolidoAlvo(aliadosL[setaAliada]);
                    ModeloSeta.SetActive (false);
                }
            }
            else
            {
                if (setaInimiga > inimigosL.Count - 1)
                    setaInimiga = 0;
                seta.position = inimigosL[setaInimiga].obj.transform.position + (Vector3.up * 2);
                if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.confirmar))
                {
                    gbatalha.escolidoAlvo(inimigosL[setaInimiga]);
                    ModeloSeta.SetActive( false);
                }
            }
            Gseta.definirDestino(seta.position);



        }
        private void horizontal(int x)
        {
            // aliado
            if (aliador_inimigo < 0)
            {
                if (setaAliada +x < aliadosL.Count && setaAliada + x > -1)
                {
                    try
                    {
                        if (aliadosL[setaAliada + x].data.bufferData != null)
                        {
                            setaAliada += x;
                        }
                    }
                    catch
                    {
                        setaAliada = 0;
                    }

                }
                else
                {
                    setaAliada = 0;
                }



            }
            else
            {
                if (setaInimiga + x < inimigosL.Count && setaInimiga + x > -1)
                {
                    try
                    {
                        if (inimigosL[setaInimiga + x].data.bufferData != null)
                        {
                            setaInimiga += x;
                        }
                    }
                    catch { setaInimiga = 0; }

                }
                else
                {
                    setaInimiga = 0;
                }




            }
        }
        public void vertical(int x)
        {
           
            if (x > 0 && aliador_inimigo < 0)
                aliador_inimigo = 1;
            if (x < 0 && aliador_inimigo > 0)
                aliador_inimigo = -1;
        }
        private void attvida()
        {
 
 
          
 
            foreach (var a in gbatalha.ordemBatalha)
            {
                if (a.obj == null)
                    continue;
 
                a.gvd.vidaPerdida.transform.localPosition = new Vector3((a.data.vida / a.data.vida_maxima
                    ),
                    a.gvd.vidaPerdida.transform.localPosition.y, a.gvd.vidaPerdida.transform.localPosition.z);

                if(a.data.escudos.Count > 0)
                {
                   foreach(var b in a.data.escudos)
                    {
                        GerenciadorAttVida ges = b.Ges.GetComponent<GerenciadorAttVida>();

                        ges.vidaPerdida.transform.localPosition = new Vector3((b.escudo_ / b.escudo_maximo
                  ),
                  ges.vidaPerdida.transform.localPosition.y, ges.vidaPerdida.transform.localPosition.z);
                    }
                }
            }

        }
        private void attEnergia()
        {
            float total_A = 0;
            float total_I = 0;

            foreach (var a in aliadosL)
            {
                if (a.data.bufferData == null)
                    continue;
                total_A += a.data.bufferData.Estamina * (1 + (a.data.bufferData.TaxaDeCrescimentoDaEstamina * a.data.level));
            }
            totalDeEnergiaAliada = total_A;

            foreach (var a in inimigosL)
            {
                if (a.data.bufferData == null)
                    continue;
                total_I += a.data.bufferData.Estamina * (1 + (a.data.bufferData.TaxaDeCrescimentoDaEstamina * a.data.level));
            }
            totalDeEnergiaInimiga = total_I;

        }
        private void mostrarEnergia()
        {
            Vector3 aa = UI_valor_1.anchoredPosition;
            aa.x = (gbatalha.EnergiaAtualAliada / totalDeEnergiaAliada) * UI_tamanhoX_energiaMaxima;
            UI_valor_1.anchoredPosition = aa;
        }
        private void attPosicao()
        {
            for (int x = 0; x < aliadosL.Count; x++)
            {
                try
                {
                    aliadosL[x].obj.transform.position = aliados.transform.position + (Vector3.right * x * espasamentoentreSi);

                    aliadosL[x].nome_.transform.position = aliadosL[x].obj.transform.position + Vector3.up * 2 + (Vector3.up * (aliadosL.Count - x) * AlturaNomes);
                }
                catch
                {
                }
            }

            for (int x = 0; x < inimigosL.Count; x++)
            {
                try
                {
                    inimigosL[x].obj.transform.position = inimigos.transform.position + (Vector3.right * x * espasamentoentreSi);


                    inimigosL[x].nome_.transform.position = (inimigosL[x].obj.transform.position + Vector3.up * 2) + (Vector3.up * (inimigosL.Count - x) * AlturaNomes);
                }
                catch { }

 
              
 
            }
        }
        private void Update()
        {
            //      moverseta();
            attvida();
            attEnergia();
            mostrarEnergia();
            attPosicao();
        }


        public float tentarFugir = 30;
        public void TentarFugir()
        {
            if(gbatalha._statusgame == statusGame.player)
            {
                bool conseguio = UnityEngine.Random.Range(0, 100) > tentarFugir;
                if (!conseguio)
                {
                    tentarFugir += 8;

                }
                else
                {
                    SceneManager.LoadScene("SampleScene");
                }

            }
        }
    }
}
