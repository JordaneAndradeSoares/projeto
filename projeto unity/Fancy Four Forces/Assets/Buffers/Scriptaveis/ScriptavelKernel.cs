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
        public List<ScriptavelHabilidades> habilidades;
        public ScriptavelHabilidades ataqueBasico;

        public void iniciar(Kernel a)
        {
            bufferData = a.bufferData;
          level = a.level;
       habilidades.AddRange(a.habilidades);
            ataqueBasico = a.ataqueBasico ;
    }
     

    }
}
