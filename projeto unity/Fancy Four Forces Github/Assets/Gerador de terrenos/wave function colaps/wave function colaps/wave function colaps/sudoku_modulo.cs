using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "moduloSDK", menuName = "sudoku/moduloSDK", order = 1)]
public class sudoku_modulo :ScriptableObject
{ 
   
    public GameObject modulo;
    public int probabilidade;
    public bool vantagemPL;
    public List<sudoku_modulo> somente_esquerda, somente_direita, somente_emcima, somente_embaixo;
   
}
