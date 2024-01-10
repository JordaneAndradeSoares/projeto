using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDk_estruturaGerada : MonoBehaviour
{
    public sudoku_modulo sdkM;
    void Start()
    {
        GetComponentInParent<gerenciadorWFC>().botaremSDK(this);
    }

  
}
