using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "NovoProblema", menuName = "Quests/Problema", order = 1)]

public class Problema : ScriptableObject
{
    public Problema proximoProblema;
    public List<struct_DerrotarMonstro> monstros = new List<struct_DerrotarMonstro>();
    public List<struct_ColetarItem> itens  = new List<struct_ColetarItem>();

    [System.Serializable]
    public struct struct_DerrotarMonstro {
        public DerrotarMonstro mosntro;
        public int quantidade;
    }
    [System.Serializable]
    public struct struct_ColetarItem
    {
        public GeralIten item;
        public int quantidade;
    }


}


