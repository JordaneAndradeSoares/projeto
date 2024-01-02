using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static modoBatalha.Configuracao;
using static UnityEngine.GraphicsBuffer;
using Buffers;
using System.Net.NetworkInformation;
using jogador;
using UnityEngine.UI;

namespace modoBatalha
{
    public class Configuracao : MonoBehaviour
    {
        public Transform aliados, inimigos,seta;
    
        public float espasamentoentreSi;
        public GameObject prefabVida,prefabNome;

        public ScriptavelBatalhaBuffer dataBatalha;

        public List<buffer_s> aliadosL = new List<buffer_s>();
        public List<buffer_s> inimigosL = new List<buffer_s>();

        public List<buffer_s> ordemBatalha = new List<buffer_s>();
        public GerenciadorDeBatalha gbatalha;
        [Space()]
        public RectTransform UI_CapacidadeMaxima,UI_valor_1, UI_valor_2;
        public float UI_tamanhoX_energiaMaxima;

        // aliados
        public float totalDeEnergiaAliada;
       
        
        // inimigos
        
        public float totalDeEnergiaInimiga;
       

        [System.Serializable]
        public class buffer_s {
            public GameObject obj;
            public Vector3 local;
            public Kernel data;
            public bool npc;
            public GerenciadorAttVida gvd;
            public buffer_s()
            {
                local = Vector3.zero;
                obj = null;
            }

            public float defesaFinal()
            {
                /*
                Debug.Log("calculo da defesa");                Debug.Log("defesa fisica = " +data.bufferData.DefesaFisica +" "+
                    "taxa de crescimento vezes o lvl = " + (data.bufferData.TaxaDeCrescimentoDaDefesaFisica * data.level)+
                    "  resultado  = "+ (data.bufferData.DefesaFisica * (1 + (data.bufferData.TaxaDeCrescimentoDaDefesaFisica * data.level)))

                    );
*/
                return (data.bufferData.DefesaFisica * (1 + (data.bufferData.TaxaDeCrescimentoDaDefesaFisica * data.level)));
            }
            public float danoBruto(ScriptavelHabilidades hbl)
            {
                float temp = 0;

                // dano normal
           
                temp = hbl.porcentagemDoEfeito *(1 + (data.bufferData.AtaqueFisico * data.bufferData.TaxaDeCrescimentoDoAtaqueBasico));
                // bonus de sinergia
//                Debug.Log("calculo do dano bruto");                Debug.Log("porcentagemDoEfeito = " + hbl.porcentagemDoEfeito + " " +                    "taxa de crescimento vezes o lvl = " + ((data.bufferData.AtaqueFisico * data.bufferData.TaxaDeCrescimentoDoAtaqueBasico)) +                    "  resultado  = " + temp);
                if (hbl._TipoDeAtaque == data.bufferData.AtaqueBasico._TipoDeAtaque)
                {
                    temp *= 1.2f;
                }

                return temp;
            }
            public void diminuirVida(float danoBruto)
            {

               

                float temp = danoBruto - defesaFinal();
              
              
                if(temp > 0)
                {

               //     Debug.Log("dano aplicado: " + temp + "foi perdido " + (temp/data.vida_maxima)+"%  da vida");
                    data.vida -= temp;
                }
            }

            public void modificarStatus(StatusAAlterar t, float valor)
            {
                switch (t) { case StatusAAlterar.Velocidade:
                     //   data.vida
                        break; }
            }
        }
        private void Start()
        {
            setaAliada = 0; setaInimiga = 0;
            if (dataBatalha)
            {
                List<buffer_s> tempList = new List<buffer_s>();
              
                for (int x= 0; x < dataBatalha.aliados.Count;x++)
                {
                   
                    if (dataBatalha.aliados[x].bufferData == null)
                        continue;
                    aliadosL[x].data = new Kernel(dataBatalha.aliados[x]);

                    aliadosL[x].obj = Instantiate(dataBatalha.aliados[x].bufferData.modelo_3D, aliados);
                    

                    GameObject gvd_ = Instantiate(prefabVida, aliadosL[x].obj.transform);
                    gvd_.transform.position = aliadosL[x].obj.transform.position + Vector3.up;
                    aliadosL[x].gvd = gvd_.GetComponent<GerenciadorAttVida>();

                    GameObject nome__ = Instantiate(prefabNome, aliadosL[x].obj.transform);
                    nome__.transform.position = aliadosL[x].obj.transform.position + Vector3.up * 2;
                    nome__.GetComponent<GerenciadorAttVida>().nome.text = "" +
                        aliadosL[x].data.bufferData.Nome + " [ Lvl." + aliadosL[x].data.level + "]";
                    
                   
                    tempList.Add(aliadosL[x]);
                }
                for (int x = 0; x < dataBatalha.inimigos.Count; x++)
                {
                 
                    if (dataBatalha.inimigos[x].bufferData == null)
                        continue;
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


                    //  temp_.att(dataBatalha.inimigos[x].retorno());


                    tempList.Add(inimigosL[x]);
                }


                for (int i = tempList.Count - 1; i >= 1; i--)
                {
                    // Encontra o maior elemento entre i e o início da lista.
                    int largestIndex = i;
                    for (int j = 0; j < i; j++)
                    {
                        if (tempList[largestIndex].data.bufferData.Velocidade * (1 + (tempList[largestIndex].data.bufferData.TaxaDeCrescimentoDaVelocidade * tempList[largestIndex].data.level))
                            <
                            tempList[j].data.bufferData.Velocidade *(1 + ( tempList[j].data.bufferData.TaxaDeCrescimentoDaVelocidade * tempList[j].data.level)))
                        {
                            largestIndex = j;
                        }                    }

                    // Troca o elemento em i com o maior elemento encontrado.
                    buffer_s temp = tempList[i];
                    tempList[i] = tempList[largestIndex];
                    tempList[largestIndex] = temp;
                }



                ordemBatalha.AddRange(tempList);

                foreach (var a in  aliadosL)
                {
                    if (!a.obj)
                        continue;
                    a.obj.transform.position = a.local;
                }
                foreach (var a in  inimigosL)
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

        public int setaAliada = 0, setaInimiga = 0,aliador_inimigo =-10;

        public void moverseta()
        {
            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.paraFrente)){
                horizontal(-1);
            }
            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.paraTras)){
                horizontal(1);
            }

         

            if (aliador_inimigo < 0)
            {
                seta.localPosition = aliadosL[setaAliada].local + (Vector3.up * 2);
                if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.confirmar))
                {
                    gbatalha.escolidoAlvo(aliadosL[setaAliada]);
                }
            }
            else
            {
                seta.localPosition = inimigosL[setaInimiga].local + (Vector3.up * 2);
                if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.confirmar))
                {
                    gbatalha.escolidoAlvo(inimigosL[setaInimiga]);
                }
            }

           

            }
        private void horizontal(int x)
        {
            // aliado
            if (aliador_inimigo < 0)
            {
                if (setaAliada + x < 4 && setaAliada + x > -1)
                   
                {
                    if (aliadosL[setaAliada+x].data.bufferData != null)
                    {
                        setaAliada += x;
                    }
                   
                }
               
             
            }
            else
            {
                if (setaInimiga +x < 4 && setaInimiga + x >-1)
                {
                    if (inimigosL[setaInimiga +x].data.bufferData != null)
                    {
                        setaInimiga +=x;
                    }
                   
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
            foreach(var a in gbatalha.ordemBatalha)
            {
                if (a.obj == null)
                    continue;
                a.gvd.vidaPerdida.transform.localPosition = new Vector3((a.data.vida / a.data.vida_maxima
                    ),
                    a.gvd.vidaPerdida.transform.localPosition.y, a.gvd.vidaPerdida.transform.localPosition.z);
            }

        }
        private void attEnergia()
        {
            float total_A = 0;
            float total_I = 0;

            foreach(var a in aliadosL)
            {
                if (a.data.bufferData == null)
                    continue;
                total_A += a.data.bufferData.Estamina * (1+(a.data.bufferData.TaxaDeCrescimentoDaEstamina * a.data.level));
            }
            totalDeEnergiaAliada = total_A;

            foreach (var a in inimigosL)
            {
                if (a.data.bufferData == null)
                    continue;
                total_I += a.data.bufferData.Estamina * (1+( a.data.bufferData.TaxaDeCrescimentoDaEstamina * a.data.level));
            }
            totalDeEnergiaInimiga = total_I;

        }
        private void mostrarEnergia()
        {
            Vector3 aa = UI_valor_1.anchoredPosition;
            aa.x = (gbatalha.EnergiaAtualAliada / totalDeEnergiaAliada) * UI_tamanhoX_energiaMaxima;
            UI_valor_1.anchoredPosition = aa;
        }
        private void Update()
        {
      //      moverseta();
            attvida();
            attEnergia();
            mostrarEnergia();
        }

    }
    [CustomEditor(typeof(Configuracao))]
    public class EditorMovimento : Editor
    {
        private void OnEnable()
        {
            Configuracao meuScript = (Configuracao)target;

            if(meuScript.aliadosL.Count == 0)
            {
                for(int x = 0; x < 4; x++)
                {

                    meuScript.aliadosL.Add(new buffer_s());
                }
            }

            if (meuScript.inimigosL.Count == 0)
            {
                for (int x = 0; x < 4; x++)
                {

                    meuScript.inimigosL.Add(new buffer_s());
                }
            }
        }
        private void OnSceneGUI()
        {
            Configuracao meuScript = (Configuracao)target;
            if (meuScript.aliados != null)
            {

                for (int x = 0; x < meuScript.aliadosL.Count; x++)
                {
                    meuScript.aliadosL[x].local = meuScript.aliados.position + (Vector3.right * x * meuScript.espasamentoentreSi);


                    Handles.color = Color.green;
                    Handles.DrawLine(meuScript.aliadosL[x].local, meuScript.aliadosL[x].local + Vector3.up);
                }

               
            }
            if (meuScript.inimigos != null)
            {
                for (int x = 0; x < meuScript.inimigosL.Count; x++)
                {
                    meuScript.inimigosL[x].local = meuScript.inimigos.position + (Vector3.right * x * meuScript.espasamentoentreSi);

                    Handles.color = Color.red;
                    Handles.DrawLine(meuScript.inimigosL[x].local, meuScript.inimigosL[x].local + Vector3.up);
                }
            }

            foreach(var a in meuScript.aliadosL)
            {
                if (!a.obj)
                    continue;
                a.obj.transform.position = a.local;
            }
            foreach(var a in meuScript.inimigosL)
            {
                if (!a.obj)
                    continue;
                a.obj.transform.position = a.local;
            }

        }
    }
}
