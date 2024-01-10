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
                inventario.PoeiraEstelar +=(KernelAtual.level * 10);
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
                if (inventario.PoeiraEstelar >= KernelAtual.level * 10)
                {
                    inventario.PoeiraEstelar -= KernelAtual.level * 10;
                    KernelAtual.level++;
                }
                else
                {
                    Debug.Log(" " + inventario.PoeiraEstelar + " vs " + KernelAtual.level * 10);
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
            MostrarQuantidadeDePoeiraEstelar.tmp.text = "[PE: " + inventario.PoeiraEstelar+ "]";
    }
        
        public void KernelEvoluir()
        {
            if (KernelAtual != null)
            {
                if (inventario.PoeiraEstelar >= KernelAtual.level * 10)
                {
                    Debug.Log("evoluir gastou " + KernelAtual.level * 10);
                    inventario.PoeiraEstelar -= KernelAtual.level * 10;
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
      

        private void OnDisable()
        {
            
        }
        private void OnEnable()
        {
            attDados();
        }
    }
}
