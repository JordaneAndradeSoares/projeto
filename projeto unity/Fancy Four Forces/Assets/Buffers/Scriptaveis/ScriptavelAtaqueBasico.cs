using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{

    [CreateAssetMenu(fileName = "NovoAtaqueBasico", menuName = "Buffers/NovoAtaqueBasico", order = 1)]
    public class ScriptavelAtaqueBasico : ScriptableObject
    {
        public string NomeAtaqueBasico;

        public int _Hits = 1;
        public TipoDeAtaque _TipoDeAtaque;


        [Range(0f, 3f)]
        public float porcentagemDoEfeito;
    }
}
       
