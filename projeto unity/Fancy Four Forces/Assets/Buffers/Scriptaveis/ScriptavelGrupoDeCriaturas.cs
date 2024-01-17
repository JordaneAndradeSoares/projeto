using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Buffers
{
    [CreateAssetMenu(fileName = "NovoConjunto", menuName = "Buffers/CriarConjunto", order = 1)]
    public class ScriptavelGrupoDeCriaturas : ScriptableObject
    {
        public List<ScriptavelBuffer> BufferData;

        public List<probabilidades> BufferDataProbabilidade;
        public float somatoriaProb_dia, somatoriaProb_noite;

        [System.Serializable]
        public class probabilidades
        {
            public ScriptavelBuffer data;
            [Range(0f, 1f)]
            public float probabilidade;

            public probabilidades(ScriptavelBuffer a, float b)
            {
                data = a;
                probabilidade = b;
            }
        }
        public void teste()
        {
            Debug.Log("a");
            float NEmedio = 0;

            for (int x = 0; x < BufferData.Count; x++)
            {
                NEmedio += BufferData[x].EstagioEvolutivo;
            }
            NEmedio /= BufferData.Count;
            float tempF_D = 0;
            float tempF_N = 0;
            for (int x = 0; x < BufferData.Count; x++)
            {
                float prob = BufferData[x].EstagioEvolutivo / NEmedio;


                //
                float diferenca = BufferData[x].EstagioEvolutivo - NEmedio;
                float valor = Mathf.Abs(diferenca) / NEmedio;
                prob = Mathf.Lerp(1, 0, valor);
                BufferDataProbabilidade.Add(new probabilidades(BufferData[x], prob));
                //
                prob = BufferData[x].EstagioEvolutivo < NEmedio ? prob * 2 : prob / 2;
                if (BufferData[x].criaturaNoturna)
                {
                    tempF_D += prob;
                }
                else
                {
                    tempF_N += prob;
                }

            }

            somatoriaProb_dia = tempF_D;
            somatoriaProb_noite = tempF_N;

        }
    }
      

    [CustomEditor(typeof(ScriptavelGrupoDeCriaturas))]
    public class EditorScriptavelGrupoDeCriaturas : Editor {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ScriptavelGrupoDeCriaturas meuScript = (ScriptavelGrupoDeCriaturas)target;
            if (GUILayout.Button("CriarProbabilidade") && meuScript.BufferDataProbabilidade.Count ==0)
            {
                meuScript.teste();
             //   meuScript.calcularProbabilidade();
            }
        }
    }
    

}
