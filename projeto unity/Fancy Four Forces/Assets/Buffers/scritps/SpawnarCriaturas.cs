using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ValoresGlobais;

namespace Buffers
{
    public class SpawnarCriaturas : MonoBehaviour
    {

        public float amplitude, frequencia, ruptura;
        public float escala;
        public int tamanho;

        public List<Vector3> pontos = new List<Vector3>();

        public List<ScriptavelBuffer> buffersspawn;
        public GameObject prefabmonstro;
        public bool guizmo_;
        public void criarpontos()
        {
            pontos.Clear();

            for (int x = 0; x < tamanho; x++)
            {
                for (int z = 0; z < tamanho; z++)
                {
                    Vector3 temp = transform.position;

                    temp.x += x * escala;
                    temp.z += z * escala;

                    if(ValoresUniversais.calcularPerlingNoise(temp.x, temp.z, amplitude, frequencia) >ruptura)
                    {
                        pontos.Add(temp);
                    }

                }
            }
        }

        // *** arrumar dps / spawnar as criaturas indo direto no ScriptavelBuffer>modelo



    }



}
