using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{
    public class AttBuffersCript : MonoBehaviour
    {
        public ClusterEvolucoes cle;
        private void Start()
        {
            
            foreach(var a in cle.TodasAsEvolucoes) {
                a.BufferData.EstagioEvolutivo = a.Origens[0].EstagioEvolutivo + 1;
            }
        }
    }
}
