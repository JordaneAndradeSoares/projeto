using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Buffers;
namespace modoBatalha
{
    public class AuxUiHabilidade : MonoBehaviour
    {

        public TextMeshProUGUI Habilidade;
        public GerenciadorDeBatalha GB;
        public ScriptavelHabilidades SH;

        public void usarHabilidade()
        {
            GB.usandoHabilidade(SH);
        }
    }
}
