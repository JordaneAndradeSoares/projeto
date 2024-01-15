using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfiguracoesDoMundo
{
    public class GeracaoDeObjetosAbertos : MonoBehaviour
    {

        public Terrain terrain; // Arraste o Terrain para esta variável
        public GameObject[] modelosDeArvores; // Lista de modelos de árvores

        public int quantidadeDeArvores = 100; // Quantidade de árvores a serem geradas
        public float inclinacaoMaxima = 30f; // Ângulo máximo do terreno em que árvores serão geradas

        void Start()
        {
            if (terrain == null || modelosDeArvores.Length == 0)
            {
                Debug.LogError("Terrain ou modelos de árvores não estão configurados.");
                return;
            }

            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainSize = terrainData.size;

            for (int i = 0; i < quantidadeDeArvores; i++)
            {
                // Gere posições aleatórias
                float randomX = Random.Range(0f, terrainSize.x) + transform.position.x;
                float randomZ = Random.Range(0f, terrainSize.z) + transform.position.z;

                // Obtenha a altura do terreno na posição
                float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + transform.position.y;
                Vector3 position = new Vector3(randomX, terrainHeight, randomZ);

                // Verifique se o ângulo do terreno é menor que o ângulo máximo
                Vector3 normal = terrain.terrainData.GetInterpolatedNormal(randomX / terrainSize.x, randomZ / terrainSize.z);
                float anguloTerreno = Vector3.Angle(normal, Vector3.up);
                Quaternion RotRandom = Quaternion.Euler(0, Random.Range(0, 360), 0);

                if (anguloTerreno <= inclinacaoMaxima)
                {
                    // Instancie uma árvore aleatória
                    int indiceArvore = Random.Range(0, modelosDeArvores.Length);
                    Instantiate(modelosDeArvores[indiceArvore], position, RotRandom, transform);
                }
            }
        }
    }
}
