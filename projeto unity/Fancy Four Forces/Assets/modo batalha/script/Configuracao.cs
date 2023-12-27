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


        public ScriptavelBatalhaBuffer dataBatalha;

        public List<buffer_s> aliadosL = new List<buffer_s>();
        public List<buffer_s> inimigosL = new List<buffer_s>();

        public List<buffer_s> ordemBatalha = new List<buffer_s>();
       
        [System.Serializable]
        public class buffer_s {
            public GameObject obj;
            public Vector3 local;
            public ScriptavelBuffer data;
            public buffer_s()
            {
                local = Vector3.zero;
                obj = null;
            }
        }
        private void Start()
        {
            if(dataBatalha)
            {
                List<buffer_s> tempList = new List<buffer_s>();
              
                for (int x= 0; x < dataBatalha.aliados.Count;x++)
                {
                   aliadosL[x].obj = Instantiate(dataBatalha.aliados[x].modelo_3D, aliados);
                    aliadosL[x].data = dataBatalha.aliados[x];
                    tempList.Add(aliadosL[x]);
                }

                for (int x = 0; x < dataBatalha.inimigos.Count; x++)
                {
                    inimigosL[x].obj = Instantiate(dataBatalha.inimigos[x].modelo_3D, inimigos);
                    inimigosL[x].data = dataBatalha.inimigos[x];
                    tempList.Add(inimigosL[x]);
                }


                for (int i = tempList.Count - 1; i >= 1; i--)
                {
                    // Encontra o maior elemento entre i e o início da lista.
                    int largestIndex = i;
                    for (int j = 0; j < i; j++)
                    {
                        if (tempList[largestIndex].data.Velocidade < tempList[j].data.Velocidade)
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
        private void Update()
        {
            moverseta();
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
