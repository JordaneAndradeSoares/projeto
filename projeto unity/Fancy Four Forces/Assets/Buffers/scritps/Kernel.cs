using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{
    [System.Serializable]
    public class Kernel 
    {
        public ScriptavelBuffer bufferData;
        public int level;
        public float vida,vida_maxima;
        public List<ScriptavelHabilidades> habilidades = new List<ScriptavelHabilidades>();
        public ScriptavelHabilidades ataqueBasico;

        // falta as passivas

        public Kernel(ScriptavelBuffer bufferData)
        {
            level = 0;
            vida = bufferData.VidaMaxima;
            
            this.bufferData = bufferData;
        }
        public void att(Kernel a)
        {

        }
    }
}
