using Buffers;
using modoBatalha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
namespace jogador
{
    public class AuxPopUpAMD : MonoBehaviour
    {
       public TextMeshProUGUI tmp;

      
        public void mostrar(Kernel a, bool Carregar)
        {
            if (Carregar)
            {
                tmp.text = "Para evoluir Voce Gastara ";

                tmp.text += Mathf.Pow(2, a.level);
            }
            else
            {
                tmp.text = "Voce recebera ";

                tmp.text += Mathf.Pow(2, a.level)/2;
            }
            tmp.text += " de Poeira estelar";
        }


    }
}
