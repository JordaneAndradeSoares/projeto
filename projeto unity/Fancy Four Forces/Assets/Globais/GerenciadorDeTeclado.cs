using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace ValoresGlobais
{
    public class GerenciadorDeTeclado : MonoBehaviour
    {

        public KeyCode paraFrente, paraTras, paraEsquerda, paraDireita, paraPular;

        //somente em combate
        public KeyCode confirmar;
        // MUNDO ABERTO
        public KeyCode atacar , abrirEquipe;
        
        public static GerenciadorDeTeclado instanc;
        private void Start()
        {
            if (instanc == null)
            {
                instanc = this;
            }
        }

    }
}
