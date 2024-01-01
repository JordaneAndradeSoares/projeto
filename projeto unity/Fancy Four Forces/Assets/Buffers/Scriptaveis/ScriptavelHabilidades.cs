using Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Buffers
{
    [CreateAssetMenu(fileName = "NovaHabilidade", menuName = "Buffers/NovaHabilidade", order = 1)]
    public class ScriptavelHabilidades : ScriptableObject
    {
        public string NomeHabilidade;
        public bool HabilidadeAvancada;

        public TipoDeAtaque _TipoDeAtaque;
        public Efeito _Efeito;
        public Alvo _Alvo;

        [Range(0f,3f)]
        public float porcentagemDoEfeito;
    }
    public enum TipoDeAtaque { Nenhum, Corte , Esmagamento , Perfurante }
    public enum Efeito { Nenhum,Dano,MudarStatus}
    public enum Alvo {  Nenhum, Unico,Global}


}







