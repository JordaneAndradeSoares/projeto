using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{
    [CreateAssetMenu(fileName = "ClusterDeEvolucoes", menuName = "Buffers/ClusterEvolucao", order = 1)]
    public class ClusterEvolucoes : ScriptableObject
    {
        public List<ScriptavelAuxiliarEvolucao> TodasAsEvolucoes;
      
    }
}
