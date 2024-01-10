
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{
    [CreateAssetMenu(fileName = "NovoInventario", menuName = "Jogador/NovoInventario", order = 1)]
    public class ScriptavelInventario : ScriptableObject
    {
       
        public List<Kernel> Inventario;
        public float PoeiraEstelar;
    }
}
