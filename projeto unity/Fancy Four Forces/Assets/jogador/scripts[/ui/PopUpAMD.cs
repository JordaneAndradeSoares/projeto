using Buffers;
using modoBatalha;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace jogador
{
    public class PopUpAMD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        public GameObject prefabResumo;
        public Kernel AUH;
        public GameObject resumo;
        public bool Carregar;

        public void abrir()
        {
         
            if (resumo == null)
            {
                resumo = Instantiate(prefabResumo, this.transform.parent);
                resumo.transform.position = this.transform.position;
                resumo.GetComponent<AuxPopUpAMD>().mostrar(AUH, Carregar);


            }

        }
    
        public void fechar()
        {
            if (resumo != null)
            {
                Destroy(resumo);
                resumo = null;
            }
            Debug.Log("fechando");

        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            abrir();


        }

        public void OnPointerExit(PointerEventData eventData)
        {
            fechar();
        }
    }
}
