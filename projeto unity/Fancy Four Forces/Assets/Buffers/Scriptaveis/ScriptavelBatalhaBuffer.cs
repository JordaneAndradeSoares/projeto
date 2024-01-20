using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{
    [CreateAssetMenu(fileName = "BatalhaBuffer", menuName = "Buffers/batalhaBuffer", order = 1)]
    public class ScriptavelBatalhaBuffer : ScriptableObject
    {
        public List<Kernel> aliados;
        public List<Kernel> inimigos;
    }
}
