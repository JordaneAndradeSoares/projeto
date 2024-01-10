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


        [System.Serializable]
        public class probabilidades {
            public ScriptableObject data;
            [Range(0f,1f)]
            public float probabilidade;

            public probabilidades(ScriptableObject a,float b)
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
                foreach(var a in meuScript.BufferData)
                {
                    meuScript.BufferDataProbabilidade.Add(new ScriptavelGrupoDeCriaturas.probabilidades(a,
                       Random.Range(0f,1f) ));

                }
            }
        }
    }
    

}
