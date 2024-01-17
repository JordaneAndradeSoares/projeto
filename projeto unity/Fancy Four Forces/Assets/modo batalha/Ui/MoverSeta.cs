using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using DG;
namespace modoBatalha
{
    public class MoverSeta : MonoBehaviour 
    {
        
        public GameObject Direc;
        public float tamanho;

        public GameObject particula;

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
      public   float tempoentrepontos,tempoDeVida;
        float f;

        public ParticleSystem temp;
        private void Update()
        {
           
            temp.startLifetime = Vector3.Distance(transform.position, Direc.transform.position) / temp.startSpeed;
            particula.transform.LookAt(Direc.transform.position);
        }
    }
}
