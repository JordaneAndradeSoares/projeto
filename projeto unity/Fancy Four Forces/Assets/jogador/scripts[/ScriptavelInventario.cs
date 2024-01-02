using Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jogador
{
    [CreateAssetMenu(fileName = "NovoInventario", menuName = "Jogador/NovoInventario", order = 1)]
    public class ScriptavelInventario : ScriptableObject
    {
       
        public List<Kernel> Inventario;
    }
}
