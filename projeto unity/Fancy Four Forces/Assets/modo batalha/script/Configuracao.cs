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
        public GameObject prefabVida;

        public ScriptavelBatalhaBuffer dataBatalha;

        public List<buffer_s> aliadosL = new List<buffer_s>();
        public List<buffer_s> inimigosL = new List<buffer_s>();

        public List<buffer_s> ordemBatalha = new List<buffer_s>();
        public GerenciadorDeBatalha gbatalha;

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
                    aliadosL[x].data = new Kernel(dataBatalha.aliados[x].bufferData);
                    aliadosL[x].obj = Instantiate(dataBatalha.aliados[x].bufferData.modelo_3D, aliados);
                    
                    aliadosL[x].data.habilidades = (dataBatalha.aliados[x].habilidades);
                    aliadosL[x].data.ataqueBasico = dataBatalha.aliados[x].ataqueBasico;

                    GameObject gvd_ = Instantiate(prefabVida, aliadosL[x].obj.transform);
                    gvd_.transform.position = aliadosL[x].obj.transform.position + Vector3.up;
                    aliadosL[x].gvd = gvd_.GetComponent<GerenciadorAttVida>();
                 
                    //  temp.att(  dataBatalha.aliados[x].retorno());
                   
                    tempList.Add(aliadosL[x]);
                }
                for (int x = 0; x < dataBatalha.inimigos.Count; x++)
                {
                 
                    if (dataBatalha.inimigos[x].bufferData == null)
                        continue;
                    inimigosL[x].data = new Kernel(dataBatalha.inimigos[x].bufferData);
                    inimigosL[x].obj = Instantiate(dataBatalha.inimigos[x].bufferData.modelo_3D, inimigos);

                    inimigosL[x].data.habilidades = (dataBatalha.inimigos[x].habilidades);
                    inimigosL[x].data.ataqueBasico = dataBatalha.inimigos[x].ataqueBasico;

                    GameObject gvd_ = Instantiate(prefabVida, inimigosL[x].obj.transform);
                    gvd_.transform.position = inimigosL[x].obj.transform.position + Vector3.up;
                    inimigosL[x].gvd = gvd_.GetComponent<GerenciadorAttVida>();
                    inimigosL[x].npc = true;


                   
                  //  temp_.att(dataBatalha.inimigos[x].retorno());
                   

                    tempList.Add(inimigosL[x]);
                }


                for (int i = tempList.Count - 1; i >= 1; i--)
                {
                    // Encontra o maior elemento entre i e o início da lista.
                    int largestIndex = i;
                    for (int j = 0; j < i; j++)
                    {
                        if (tempList[largestIndex].data.bufferData.Velocidade < tempList[j].data.bufferData.Velocidade)
                        {
                            largestIndex = j;
                        }
                    }

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

            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.paraEsquerda))
            {
                vertical(-1);
            }
            if (Input.GetKeyDown(GerenciadorDeTeclado.instanc.paraDireita))
            {
                vertical(1);
            }

            if (aliador_inimigo < 0)
            {
                seta.localPosition = aliadosL[setaAliada].local + (Vector3.up * 2);
            }
            else
            {
                seta.localPosition = inimigosL[setaInimiga].local + (Vector3.up * 2);
            }

            }
        private void horizontal(int x)
        {
            // aliado
            if (aliador_inimigo < 0)
            {
                if (setaAliada + x < 4 && setaAliada + x > -1)
                   
                {
                    if (aliadosL[setaAliada+x].data != null)
                    {
                        setaAliada += x;
                    }
                   
                }
               
             
            }
            else
            {
                if (setaInimiga +x < 4 && setaInimiga + x >-1)
                {
                    if (inimigosL[setaInimiga +x].data != null)
                    {
                        setaInimiga +=x;
                    }
                   
                }
              
               
              
            }
        }
        private void vertical(int x)
        {
            if (x > 0 && aliador_inimigo < 0)
                aliador_inimigo = 1;
            if (x < 0 && aliador_inimigo > 0)
                aliador_inimigo = -1;
        }
        private void attvida()
        {
            foreach(var a in aliadosL)
            {
                if (a.obj == null)
                    continue;
                a.gvd.vidaPerdida.transform.localPosition = new Vector3((a.data.vida / a.data.bufferData.VidaMaxima),
                    a.gvd.vidaPerdida.transform.localPosition.y, a.gvd.vidaPerdida.transform.localPosition.z);
            }

            foreach (var a in inimigosL)
            {
                if (a.obj == null)
                    continue;
                a.gvd.vidaPerdida.transform.localPosition = new Vector3((a.data.vida / a.data.bufferData.VidaMaxima),
                    a.gvd.vidaPerdida.transform.localPosition.y, a.gvd.vidaPerdida.transform.localPosition.z);
            }
        }
        private void Update()
        {
      //      moverseta();
      //      attvida();
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
