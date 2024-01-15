using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using System.IO;

namespace ValoresGlobais
{
    public class GerenciadorDeTeclado : MonoBehaviour
    {

        public KeyCode paraFrente, paraTras, paraEsquerda, paraDireita, paraPular;

        //somente em combate
        public KeyCode confirmar;
        // MUNDO ABERTO
        public KeyCode atacar , abrirEquipe,AbrirMatrizDeDados;
        
        public static GerenciadorDeTeclado instanc;
        private void Start()    
        {
          
            Debug.Log(File.Exists
                (Application.dataPath + @"/Buffers/Rede neural/Cerebro/RedeNeural.save"));
            if (instanc == null)
            {
                instanc = this;
            }
        }

    }
}
