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

        public List<habilidade_inf> EfeitosHabilidades = new List<habilidade_inf>();

        public habilidade_inf temp = new habilidade_inf();

        public tiposNatureza ForcaDaNatureza;     
        public tipoDeHabilidade TipoDeHabilidade;
        public ativacoes Aplicacao;
        public TipoDeBuffDebuff tipoDeBuffDebuff;

        
        public float CustoDeHabilidade;
       
        public float EficaciaHabilidade;

        public int pagina = 0;

        public bool criandoHabilidade;
    }
    [System.Serializable]
    public class habilidade_inf {
        public tiposNatureza ForcaDaNatureza;
        [Tooltip("o que a habilidade faz ? dano, buff , debuff")]
        public tipoDeHabilidade TipoDeHabilidade;
        [Tooltip("A forma que o efeito sera aplicado no alvo")]
        public ativacoes Aplicacao;

     
        public float CustoDeHabilidade;
        [Tooltip("Por exemplo: o quanto de dano sera causado, o quanto de defesa sera dado ao aliado, ou o quando de debuff sera dado ao inimigo")]
       
        public float EficaciaHabilidade;
        public TipoDeBuffDebuff TipoDeBuffDebuff;

    }


}


[CustomEditor(typeof(ScriptavelHabilidades))]
public class editorScriptavelHabilidade : Editor {

    public SerializedProperty EfeitosHabilidades , criandoHabilidade, NomeHabilidade;

    public SerializedProperty ForcaDaNatureza , TipoDeHabilidade, Aplicacao, tipoDeBuffDebuff, CustoDeHabilidade, EficaciaHabilidade;
    private void OnEnable()
    {
        NomeHabilidade = serializedObject.FindProperty("NomeHabilidade");
        EfeitosHabilidades = serializedObject.FindProperty("EfeitosHabilidades");

        criandoHabilidade = serializedObject.FindProperty("criandoHabilidade");

        ForcaDaNatureza = serializedObject.FindProperty("ForcaDaNatureza");
        TipoDeHabilidade = serializedObject.FindProperty("TipoDeHabilidade");
        Aplicacao = serializedObject.FindProperty("Aplicacao");
        tipoDeBuffDebuff = serializedObject.FindProperty("tipoDeBuffDebuff");
        CustoDeHabilidade = serializedObject.FindProperty("CustoDeHabilidade");
        EficaciaHabilidade = serializedObject.FindProperty("EficaciaHabilidade");


    }
    public override void OnInspectorGUI()
    {
        ScriptavelHabilidades meuScript = (ScriptavelHabilidades)target;
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.PropertyField(NomeHabilidade, new GUIContent("O nome da Habilidade"));
        EditorGUILayout.PropertyField(EfeitosHabilidades, new GUIContent("O nome da Habilidade"));

        if (meuScript.criandoHabilidade == false)
        {
            if (GUILayout.Button("Adicionar Habilidade"))
            {
                meuScript.criandoHabilidade = !meuScript.criandoHabilidade;
            }
        }
        else
        {

            EditorGUILayout.Space();
            GUILayout.Label("~ criando efeito ~");
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(ForcaDaNatureza, new GUIContent("O tipo da for�a da natureza"));
            EditorGUILayout.PropertyField(Aplicacao, new GUIContent("A forma que ira atingir o seu alvo"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(TipoDeHabilidade, new GUIContent("O que a habilidade faz ?"));
            if (meuScript.TipoDeHabilidade != tipoDeHabilidade.Dano)
            {
                EditorGUILayout.PropertyField(tipoDeBuffDebuff, new GUIContent("O que o " + meuScript.TipoDeHabilidade + " vai mudar "));

            }
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(CustoDeHabilidade, new GUIContent("Custo da energia de ponto zero"));
            EditorGUILayout.PropertyField(EficaciaHabilidade, new GUIContent("A forma que ira atingir o seu alvo"));

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Finalizar"))
            {
                meuScript.criandoHabilidade = false;
                habilidade_inf temp_ = new habilidade_inf();

                temp_.ForcaDaNatureza = meuScript.ForcaDaNatureza;

                temp_.TipoDeHabilidade = meuScript.TipoDeHabilidade;

                temp_.Aplicacao = meuScript.Aplicacao;


                temp_.CustoDeHabilidade = meuScript.CustoDeHabilidade;

                temp_.EficaciaHabilidade = meuScript.EficaciaHabilidade;
                temp_.TipoDeBuffDebuff = meuScript.tipoDeBuffDebuff;


                meuScript.EfeitosHabilidades.Add(temp_);

            }
        }





            serializedObject.ApplyModifiedProperties();
    }
        }
//for�as da natureza
public enum tiposNatureza
{
    Gravidade, ForcaForte, ForcaFraca, Eletromagnetismo, nenhum
}
// o metodo que vai ser aplicado o efeito
public enum ativacoes
{
    unico,fileira, global
}
// o tipo que o efeito �
public enum tipoDeHabilidade {
Dano,Buff,Debuff
}
// o que o buff/debuff vai alterar
public enum TipoDeBuffDebuff { 
    Velocidade,DanoFisico,Escudo,DanoEspecial,Nada
}






