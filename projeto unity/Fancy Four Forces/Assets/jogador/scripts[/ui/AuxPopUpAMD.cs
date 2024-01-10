using Buffers;
using modoBatalha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace jogador
{
    public class AuxPopUpAMD : MonoBehaviour
    {
       public TextMeshProUGUI tmp;

      
        public void mostrar(Kernel a, BotoesAMD Carregar)
        {
            if (Carregar == BotoesAMD.evoluir)
            {
                tmp.text = "Para evoluir Voce Gastara ";

                tmp.text += a.level * 10;
               
            }
            if (Carregar == BotoesAMD.carregar)
            {
                tmp.text = "Para Carregar Voce Gastara ";

                tmp.text += a.level * 10;

            }
            if (Carregar == BotoesAMD.descarregar)
            {
                tmp.text = "Voce recebera ";

                tmp.text += a.level * 5;
            }
            tmp.text += " de Poeira estelar";
        }
        public string Arredondar(float a)
        {
            return " "+ a;
        }


    }
}
