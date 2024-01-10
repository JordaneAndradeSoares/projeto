using Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.UIElements;
using UnityEngine;

namespace jogador
{
    public class PegarVantagem : MonoBehaviour
    {
        public ControleDeMovimento cm;
        public TagField tasg;
        private void OnTriggerEnter(Collider other)
        {
           
            if (other.gameObject.CompareTag("Inimigo"))
            {
             
                cm.bufferAlvo = other.GetComponent<GerenciadorBuffers>();
            }
        }
        private void OnTriggerExit(Collider other)
        {
         
            if (other.gameObject.CompareTag("Inimigo"))
            {
                if (cm.bufferAlvo == other.GetComponent<GerenciadorBuffers>())
                {
                    cm.bufferAlvo = null;
                }
            }
        }
    }
}
