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
        public float somatoriaProb;

        [System.Serializable]
        public class probabilidades {
            public ScriptavelBuffer data;
            [Range(0f,1f)]
            public float probabilidade;

            public probabilidades(ScriptavelBuffer a,float b)
            {
                data = a;
                probabilidade = b;
            }
        }

    }

    [CustomEditor(typeof(ScriptavelGrupoDeCriaturas))]
    public class EditorScriptavelGrupoDeCriaturas : Editor {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ScriptavelGrupoDeCriaturas meuScript = (ScriptavelGrupoDeCriaturas)target;
            if (GUILayout.Button("CriarProbabilidade"))
            {

                float NEmedio = 0;
                foreach(var a in meuScript.BufferData)
                {
                    NEmedio += a.EstagioEvolutivo;
                }
                NEmedio /= meuScript.BufferData.Count;
                float tempF = 0;
                foreach(var a in meuScript.BufferData)
                {
                    float prob = a.EstagioEvolutivo / NEmedio;
                    //
                    float diferenca = a.EstagioEvolutivo - NEmedio ;
                    float valor = Mathf.Abs(diferenca) / NEmedio;
                    prob = Mathf.Lerp(1, 0, valor);
                    //
                    prob = a.EstagioEvolutivo < NEmedio ? prob *2: prob  / 2;
                    tempF += prob;
                    meuScript.BufferDataProbabilidade.Add(new ScriptavelGrupoDeCriaturas.probabilidades(a, prob));

                }
                meuScript.somatoriaProb = tempF;
            }
        }
    }
    

}
