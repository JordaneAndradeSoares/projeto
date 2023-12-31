using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{
    public class DetectorDeColisor : MonoBehaviour
    {
        public string tag;

        public bool detectando;
        public Vector3 alvo;
       
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag(tag))
            {
                alvo = other.transform.position;
                detectando = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {

            if (other.gameObject.CompareTag(tag))
            {
                detectando = false;
            }
        }
    }
}
