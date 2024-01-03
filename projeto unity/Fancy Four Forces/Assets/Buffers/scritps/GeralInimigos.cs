using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buffers
{
    public class GeralInimigos : MonoBehaviour
    {
        public static GeralInimigos instact;
        public ScriptavelBatalhaBuffer equipes;

        public void iniciarBatalhaComDesvantagem(ScriptavelBuffer s, ScriptavelGrupoDeCriaturas bd)
        {

            equipes.inimigos[0].bufferData = s;
            equipes.inimigos[0].level = Random.Range(1, 20);

            for (int x = 1; x < 4; x++)
            {
                equipes.inimigos[x].bufferData = bd.BufferData[Random.Range(0,bd.BufferData.Count)];
                equipes.inimigos[x].level = Random.Range(1, 20);

            }
            SceneManager.LoadScene("ModoDeBatalha");
        }
            void Start()
        {
        
            if(GeralInimigos.instact == null)
            {
                GeralInimigos.instact = this;
            }
        }

    }
}
