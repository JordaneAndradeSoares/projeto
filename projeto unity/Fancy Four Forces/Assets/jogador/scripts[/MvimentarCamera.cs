using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace jogador
{
    public class MvimentarCamera : MonoBehaviour
    {
        public GameObject controlador;
        public GameObject foco;
        private Vector3 offset;
        float y_;

      

        public float velocidadeRotacao;
        public float scalaY;
        public float minY,maxY;
        private void Update()
        {
            float movimentoMouseX = Input.GetAxis("Mouse X");

            float movimentoMouseY = Input.GetAxis("Mouse Y");

            controlador.  transform.RotateAround(controlador.transform.position, Vector3.up, movimentoMouseX * velocidadeRotacao);

            offset = controlador.transform.position;



            y_ += scalaY * movimentoMouseY;

            if(y_ > maxY)
                y_ = maxY;
            if(y_ < minY)
                y_ = minY;
            foco.transform.position = offset + (Vector3.up * y_);

        }

    }
}
