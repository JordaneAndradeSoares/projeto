using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jogador
{
    public class TranslacaoEmObj : MonoBehaviour
    {
        public GameObject alvo;
        public float velocidade;
        void Update()
        {
               
                transform.RotateAround(alvo.transform.position, Vector3.up,velocidade);
            // transform.Rotate(alvo.transform.position, velocidade * Time.deltaTime);
        }
    }
}
