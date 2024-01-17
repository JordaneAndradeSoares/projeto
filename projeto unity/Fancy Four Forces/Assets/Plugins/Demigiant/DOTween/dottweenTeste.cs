using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using DG;

public class dottweenTeste : MonoBehaviour
{
    public GameObject alvo;
    public float tempo;
    public bool iniciar;
    // Construtor


    // Atualiza o objeto
    void Update()
    {
        // Move o objeto de uma posição para outra
        // Tween.To(_transform, 1f, new Vector3(10, 10, 10), Easing.Linear);
        if (iniciar)
        {
            iniciar = false;

            transform.DOMove(alvo.transform.position, tempo);
    } }
}
