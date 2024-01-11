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
        public void teste()
        {
            Debug.Log("a");
            float NEmedio = 0;

            for(int x = 0; x < BufferData.Count; x++)
            {
                NEmedio += BufferData[x].EstagioEvolutivo;
            }
            NEmedio /= BufferData.Count;
            float tempF = 0;
            for (int x = 0; x < BufferData.Count; x++)
            {
                float prob = BufferData[x].EstagioEvolutivo / NEmedio;

               
                //
                float diferenca = BufferData[x].EstagioEvolutivo - NEmedio;
                float valor = Mathf.Abs(diferenca) / NEmedio;
                prob = Mathf.Lerp(1, 0, valor);
                //
                prob = BufferData[x].EstagioEvolutivo < NEmedio ? prob * 2 : prob / 2;
             
                tempF += prob;
                BufferDataProbabilidade.Add(new probabilidades(BufferData[x], prob));

            }

            somatoriaProb = tempF;

        }
        public void calcularProbabilidade()
        {
            
            float NEmedio = 0;
            
            foreach (var a in BufferData)
            {
                NEmedio += a.EstagioEvolutivo;
            }
            NEmedio /= BufferData.Count;
            float tempF = 0;
            Debug.Log(BufferData.Count + NEmedio);
            foreach (var a in BufferData)
            {
                float prob = a.EstagioEvolutivo / NEmedio;
                //
                float diferenca = a.EstagioEvolutivo - NEmedio;
                float valor = Mathf.Abs(diferenca) / NEmedio;
                prob = Mathf.Lerp(1, 0, valor);
                //
                prob = a.EstagioEvolutivo < NEmedio ? prob * 2 : prob / 2;
                Debug.Log(prob);

                tempF += prob;
                BufferDataProbabilidade.Add(new probabilidades(a, prob));

            }

            somatoriaProb = tempF;
            
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
