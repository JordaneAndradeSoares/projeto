using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfiguracoesDoMundo
{
    public class GeracaoDeObjetosAbertos : MonoBehaviour
    {

        public Terrain terrain; // Arraste o Terrain para esta vari�vel
        public GameObject[] modelosDeArvores; // Lista de modelos de �rvores

        public int quantidadeDeArvores = 100; // Quantidade de �rvores a serem geradas
        public float inclinacaoMaxima = 30f; // �ngulo m�ximo do terreno em que �rvores ser�o geradas

        void Start()
        {
            if (terrain == null || modelosDeArvores.Length == 0)
            {
                Debug.LogError("Terrain ou modelos de �rvores n�o est�o configurados.");
                return;
            }

            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainSize = terrainData.size;

            for (int i = 0; i < quantidadeDeArvores; i++)
            {
                // Gere posi��es aleat�rias
                float randomX = Random.Range(0f, terrainSize.x) + transform.position.x;
                float randomZ = Random.Range(0f, terrainSize.z) + transform.position.z;

                // Obtenha a altura do terreno na posi��o
                float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + transform.position.y;
                Vector3 position = new Vector3(randomX, terrainHeight, randomZ);

                // Verifique se o �ngulo do terreno � menor que o �ngulo m�ximo
                Vector3 normal = terrain.terrainData.GetInterpolatedNormal(randomX / terrainSize.x, randomZ / terrainSize.z);
                float anguloTerreno = Vector3.Angle(normal, Vector3.up);
                Quaternion RotRandom = Quaternion.Euler(0, Random.Range(0, 360), 0);

                if (anguloTerreno <= inclinacaoMaxima)
                {
                    // Instancie uma �rvore aleat�ria
                    int indiceArvore = Random.Range(0, modelosDeArvores.Length);
                    Instantiate(modelosDeArvores[indiceArvore], position, RotRandom, transform);
                }
            }
        }
    }
}
