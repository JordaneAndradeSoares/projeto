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
        public ScriptavelAtaqueBasico SAB;
        public void usarHabilidade()
        {
            GB.EscolheuHabilidade_(this);
        }
    }
}
