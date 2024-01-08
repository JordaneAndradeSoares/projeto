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

                tmp.text += Arredondar(Mathf.Pow( a.level,2));
               
            }
            if (Carregar == BotoesAMD.carregar)
            {
                tmp.text = "Para Carregar Voce Gastara ";

                tmp.text += Arredondar(Mathf.Pow(a.level, 2));

            }
            if (Carregar == BotoesAMD.descarregar)
            {
                tmp.text = "Voce recebera ";

                tmp.text += Arredondar( Mathf.Pow( a.level,2)/2);
            }
            tmp.text += " de Poeira estelar";
        }
        public string Arredondar(float a)
        {

            return "" +a;

            if (a < 1000)
            {
                // Arredonda o número para inteiro
               int resultado = (int)Math.Round(a);

                // Retorna o número arredondado
                return resultado.ToString();
            }
            else 
            { 
                int resultado = (int)(a / 1000);
               

                if (resultado < 1000)
                {
                    return resultado + "K e "+ (int)(a- (resultado * 1000));
                }

                int milhao = (int)((float)resultado / 1000);

                return milhao+ "M e " + (int)((float)resultado - ((float)milhao * 1000f))+"K";

            }
        }


    }
}
