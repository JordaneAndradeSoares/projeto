using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace modoBatalha
{
    public class MstrarResumo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
     
        public GameObject prefabResumo;
        public AuxUiHabilidade AUH;
        public GameObject resumo;

   
        public void abrir()
        {
            if (resumo == null)
            {
                resumo = Instantiate(prefabResumo,this.transform.parent.transform.parent.transform.parent);
                resumo.transform.position = this.transform.position;
                resumo.GetComponent<AuxMostrarResumo>().mostrar(AUH);


            }
          
        }
        public void fechar()
        {
            if (resumo != null)
            {
                Destroy(resumo);
                resumo = null;
            }

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
