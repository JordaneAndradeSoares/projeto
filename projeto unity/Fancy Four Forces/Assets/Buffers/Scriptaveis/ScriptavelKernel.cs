using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Buffers
{
    [CreateAssetMenu(fileName = "KernelDeBuffer", menuName = "Buffers/NovaKernelDeBuffer", order = 2)]

    public class ScriptavelKernel : ScriptableObject
    {
        public ScriptavelBuffer bufferData;
        public int level;
        public float vida, vida_maxima;
        public List<ScriptavelHabilidades> habilidades;
       

        public void iniciar(Kernel a)
        {
            bufferData = a.bufferData;
          level = a.level;
            vida = a.vida;
            vida_maxima = a.vida_maxima;

            habilidades.Clear();
       habilidades.AddRange(a.habilidades);
           
    }
     

    }
}
