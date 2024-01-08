using Buffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace jogador
{
    public class AttMatrizDeDados : MonoBehaviour
    {
        public GameObject PontoModelo,modelo;

        public GameObject evoluir, _KernelEvoluir;
        public PopUpAMD evoluir_, extrair_,KernelEvoluir_;
        Kernel KernelAtual;
        public ClusterEvolucoes CLE;

        public AuxPopUpAMD MostrarLvlDoSelecionado;
        public AuxPopUpAMD MostrarNome;
        public AuxPopUpAMD MostrarQuantidadeDePoeiraEstelar;


        public ScriptavelInventario inventario;
        public ControleDeMovimento CM;

        public void Selecionado(Kernel a)
        {
            if(KernelAtual != a)
            {
                KernelAtual = a;
                
                if(modelo != null)
                {
                    Destroy(modelo);

                    modelo = Instantiate(a.bufferData.modelo_3D,PontoModelo.transform);
                }
                else
                {
                    modelo = Instantiate(a.bufferData.modelo_3D, PontoModelo.transform);
                }


                evoluir.SetActive(a.level < 100);
                _KernelEvoluir.SetActive(a.bufferData.evolui && a.level > 19);
                evoluir_.AUH = a;
                extrair_.AUH = a;
                KernelEvoluir_.AUH = a;
            }
            attDados();

        }
        public void Sacrificar()
        {
            if (KernelAtual != null)
            {
                inventario.PoeiraEstelar += Arredondar(Mathf.Pow(KernelAtual.level, 2) / 2);
                inventario.Inventario.Remove(KernelAtual);
                KernelAtual = null;
                Debug.Log("sacrificando");
            }
            attDados();
        }
        public void Carregar()
        {
            if (KernelAtual != null)
            {
                if (inventario.PoeiraEstelar > Arredondar(Mathf.Pow(KernelAtual.level, 2)))
                {
                    inventario.PoeiraEstelar -= Arredondar(Mathf.Pow(KernelAtual.level, 2));
                    KernelAtual.level++;
                }
            }
            Debug.Log("carregar");
            attDados();
        }

        public void attDados()
        {
            CM.AbrirAttMatrizDeDados();
            CM.AbrirAttMatrizDeDados();


            MostrarLvlDoSelecionado.gameObject.SetActive(KernelAtual != null);
            if(KernelAtual != null)
            {
                MostrarLvlDoSelecionado.tmp.text = "[Lvl." + KernelAtual.level + "]";
                MostrarNome.tmp.text = KernelAtual.bufferData.Nome;
                _KernelEvoluir.SetActive(KernelAtual.bufferData.evolui && KernelAtual.level > 19);
            }
            MostrarQuantidadeDePoeiraEstelar.tmp.text = "[PE: " + @ArredondarS(inventario.PoeiraEstelar)+ "]";
    }
        
        public void KernelEvoluir()
        {
            if (KernelAtual != null)
            {
                if (inventario.PoeiraEstelar > Arredondar(Mathf.Pow(KernelAtual.level, 2)))
                {
                    inventario.PoeiraEstelar -= Arredondar(Mathf.Pow(KernelAtual.level, 2));
                    KernelAtual.level -= 19;
                    ScriptavelBuffer temp = null;
                    foreach (var ain in CLE.TodasAsEvolucoes)
                    {
                        if (ain.Origens.Contains(KernelAtual.bufferData))
                        {
                            temp = ain.BufferData;
                            break;
                        }
                    }
                    KernelAtual.bufferData = temp;

                }
                attDados();
            }
        }
        public float Arredondar(float a)
        {


            if (a < 1000)
            {

                return MathF.Round( a);
            }
            else
            {
                int resultado = (int)(a / 1000);


                if (resultado < 1000)
                {
                    return resultado +(a - (resultado * 1000));
                }

                int milhao = (int)((float)resultado / 1000);

                return milhao + (int)((float)resultado - ((float)milhao * 1000f));

            }
        }
        public string ArredondarS(float a)
        {

          
            if (a < 1000)
            {
                // Arredonda o número para inteiro
                Int64 resultado = (int)Math.Round(a);

                // Retorna o número arredondado
                return resultado.ToString();
            }
            else
            {
                Int64 resultado = (Int64)(a / 1000);


                if (resultado < 1000)
                {
                    return resultado + "K e" + (int)(a - (resultado * 1000));
                }

                Int64 milhao = (Int64)((float)resultado / 1000);

                return milhao.ToString() + "M e" + (Int64)((float)resultado - ((float)milhao * 1000f)) + "K";

            }
        }


        private void OnDisable()
        {
            
        }
        private void OnEnable()
        {
            attDados();
        }
    }
}
