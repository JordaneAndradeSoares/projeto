using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace modoBatalha
{
    public class MoverSeta : MonoBehaviour
    {
        public GameObject Direc;
        public float tamanho;
        public void definirDestino(Vector3 a)
        {
            Direc.transform.position = a;

            Vector3 tempT = transform.localScale;
            tempT.z = Vector3.Distance(Direc.transform.position, transform.position) / tamanho;
            transform.localScale = tempT;
        }
        public void definirOrigem(Vector3 a)
        {
            transform.position = a;
        }
    }
}
