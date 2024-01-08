using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Animations;


namespace modoBatalha
{
    public class teste : MonoBehaviour
    {

        public teste proximoTeste,anteriorTeste;
        Animator rig;
        public Transform direcao;
        public float tempo;
        private void Start()
        {
            if (transform.parent.GetComponent<teste>())
            {
                transform.parent.GetComponent<teste>().proximoTeste = this;
                anteriorTeste = transform.parent.GetComponent<teste>();
            }
        
            
            
        }
        void Update()
        {
        if(proximoTeste != null && anteriorTeste != null)  {


                 Quaternion rot = Quaternion.Slerp(anteriorTeste.transform.rotation, proximoTeste.transform.rotation, tempo);
                //  Quaternion rot = Quaternion.Lerp(anteriorTeste.transform.rotation, Quaternion.LookRotation(proximoTeste.transform.position - transform.position),0.5f);
            
                transform.rotation =  Quaternion.Lerp(transform.rotation,rot,0.5f);

            }
        if(proximoTeste == null)
            {

                //transform.rotation = Quaternion.LookRotation(direcao.position, Vector3.up);
            }
      
         
        }
     
    }
}
