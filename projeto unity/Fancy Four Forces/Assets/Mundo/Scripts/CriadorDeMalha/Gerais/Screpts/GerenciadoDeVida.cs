using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ageral
{
    public class GerenciadoDeVida : MonoBehaviour
    {
        public float vida;
     
        public bool vivo()
        {
            return vida > 0;
        }
        public void diminuirVida(float a)
        {
            vida -= a;
        }
        private void Update()
        {
            if(!vivo())
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}