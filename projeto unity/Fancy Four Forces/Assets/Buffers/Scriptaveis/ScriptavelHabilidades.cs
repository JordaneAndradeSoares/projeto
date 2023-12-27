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
        public tiposNatureza ForcaDaNatureza;
        [Range(0f, 1f)]
        public float CustoDeHabilidade;
        [Tooltip("Por exemplo: o quanto de dano sera causado, o quanto de defesa sera dado ao aliado, ou o quando de debuff sera dado ao inimigo")]
        [Range(0f, 1f)]
        public float EficaciaHabilidade;

        public bool Buff_debuff;
        public int  direcaoDebuffBuff;
        [Tooltip("o efeito sera aplicado em toda a fileira")]
        public bool Fileira;
        [Tooltip("o efeito sera aplicado em todos")]
        public bool global;


    }

}
[CustomEditor(typeof(ScriptavelHabilidades))]
public class editorScriptavelHabilidade : Editor {
    public SerializedProperty NomeHabilidade, EficaciaHabilidade, ForcaDaNatureza, CustoDeHabilidade, Buff_aliado, Debuff_inimigo, Fileira, global;

    private void OnEnable()
    {
        NomeHabilidade = serializedObject.FindProperty("NomeHabilidade");
        ForcaDaNatureza = serializedObject.FindProperty("ForcaDaNatureza");
        CustoDeHabilidade = serializedObject.FindProperty("CustoDeHabilidade");
        EficaciaHabilidade = serializedObject.FindProperty("EficaciaHabilidade");

        Buff_aliado = serializedObject.FindProperty("Buff_aliado");
        Debuff_inimigo = serializedObject.FindProperty("Debuff_inimigo");

        Fileira = serializedObject.FindProperty("Fileira");
        global = serializedObject.FindProperty("global");
      


    }
    public override void OnInspectorGUI()
    {
        ScriptavelHabilidades meuScript = (ScriptavelHabilidades)target;
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        EditorGUILayout.PropertyField(NomeHabilidade, new GUIContent("O nome da Habilidade"));
        EditorGUILayout.PropertyField(ForcaDaNatureza, new GUIContent("O tipo da força usada"));
        EditorGUILayout.PropertyField(CustoDeHabilidade, new GUIContent("O quanto de energia de ponto zero sera usado"));
        EditorGUILayout.PropertyField(EficaciaHabilidade, new GUIContent("Este valor representa o o valor que sera aplicado"));

        if (GUILayout.Button("A Habilidade tem efeito em fileira ?  ( " + meuScript.Fileira + " )"))
        {
            meuScript.Fileira = !meuScript.Fileira;
        }
        if (GUILayout.Button("A Habilidade tem efeito global ?  ?  ( " + meuScript.global + " )"))
        {
            meuScript.global = !meuScript.global;
        }

        if (GUILayout.Button("A habilidade causa buff ?  ( " + (meuScript.direcaoDebuffBuff == 1 || meuScript.direcaoDebuffBuff == 3 ) ))
        {
            if (meuScript.direcaoDebuffBuff == 0)
            {
                meuScript.direcaoDebuffBuff = 1;
                meuScript.Buff_debuff = true;
            }
            if (meuScript.direcaoDebuffBuff == 1)
            {
                meuScript.direcaoDebuffBuff = 0;
                meuScript.Buff_debuff = false;
            }
            if (meuScript.direcaoDebuffBuff == 2)
            {
                meuScript.direcaoDebuffBuff = 3;
            }
            if (meuScript.direcaoDebuffBuff == 3)
            {
                meuScript.direcaoDebuffBuff = 2;
            }

        }
        serializedObject.ApplyModifiedProperties();
    }
        }

public enum tiposNatureza
{
    Gravidade, ForcaForte, ForcaFraca, Eletromagnetismo, nenhum
}




