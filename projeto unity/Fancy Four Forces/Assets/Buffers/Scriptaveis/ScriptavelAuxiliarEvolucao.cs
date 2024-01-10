using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{
    [CreateAssetMenu(fileName = "NovoAuxiliarEvolucao", menuName = "Buffers/AuxiliarEvolucao", order = 1)]
    public class ScriptavelAuxiliarEvolucao : ScriptableObject
    {
        public ScriptavelBuffer BufferData;
        public List<ScriptavelBuffer> Origens;
    }
}
