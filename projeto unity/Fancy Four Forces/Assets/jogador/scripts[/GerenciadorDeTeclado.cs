using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace jogador
{
    public class GerenciadorDeTeclado : MonoBehaviour
    {

        public KeyCode paraFrente, paraTras, paraEsquerda, paraDireita, paraPular;

        // fora de modo combate
        public KeyCode atacar;

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
