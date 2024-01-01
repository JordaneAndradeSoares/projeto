using Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
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
        [Space()]
        [Space()]
        [Tooltip("somente para efeitos de mudar status")]
        public StatusAAlterar ___StatusAAlterar;
        [Space()]
        [Space()]
        [Tooltip("caso seja alterar status pode botar o valor chegio")]
        [Range(0f,3f)]
        public float porcentagemDoEfeito;

        [Range(0f, 999f)]
        public int GastoDeHabilidade;
    }
    public enum TipoDeAtaque { Nenhum, Corte , Esmagamento , Perfurante }
    public enum Efeito { Nenhum,Dano,MudarStatus}
    public enum StatusAAlterar
    {
        Nenhum,Velocidade
    }
    public enum Alvo {  Nenhum, Unico,Global}


}







