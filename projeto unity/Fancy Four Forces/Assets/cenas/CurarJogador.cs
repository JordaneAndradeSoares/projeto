using Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurarJogador : MonoBehaviour
{
  public GameObject jogador;
    public int dist ;
    public float quantidadeDeRecuperacaoPorSegundo;
     
    public ScriptavelInventario invent;
    public float tt;
    public void recuperar()
    {
        if (tt > 1)
        {
            foreach (var a in invent.Inventario)
            {
                a.vida = a.vida > a.vida_maxima ? a.vida_maxima : a.vida + (quantidadeDeRecuperacaoPorSegundo);
            }
            tt = 0;
        }
        else
        {
            tt += Time.deltaTime;
        }
    }
    void Update()
    {
        
        if(Vector3.Distance(transform.position,jogador.transform.position) < dist)
        {
            recuperar();
        }
    }
}
