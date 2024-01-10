using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ageral
{
    public class InstanciadorDeConstrucao : MonoBehaviour
    {
        [SerializeField]
      public class modelos
        {
            public GameObject prefab;
            public Vector3 tamanho;
        }

        public List<modelos> modelosList;
        public void gerar()
        {
            // pegar uma rua
            // escolher um modelo
            // verificar pelo ponto de origem em direção ao tamanho perpendicular a rua colide com outra rua
            // depis verificar se colide com algum collider
            // se liberado instanciar o modelo

            // mover o pivo em direcao ao final da rua seguindo o tamanho dot do modelo + a distancia entre construcoes
        }

        public float distanciaEntreConstrucoes;

        public GerenciadorDeArea area;
    }

    [CustomEditor(typeof(InstanciadorDeConstrucao))]
    public class EditorgerenciadorDeINstancias : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            InstanciadorDeConstrucao meuScript = (InstanciadorDeConstrucao)target;
            if (GUILayout.Button("Testar"))
            { 
            
            }
            }
    }
}
