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
        public List<ScriptavelHabilidades> habilidadesDasCriaturas;
        public int lvlMin, lvlMax;
        public void iniciarBatalhaComDesvantagem(ScriptavelBuffer s, ScriptavelGrupoDeCriaturas bd)
        {


            equipes.inimigos[0].bufferData = s;
            equipes.inimigos[0].level = Random.Range(lvlMin, lvlMax);
            equipes.inimigos[0].completarVida();

            for(int x= 0; x < 2; x++)
            {
                equipes.inimigos[0].habilidades.Add(habilidadesDasCriaturas[Random.Range(0, habilidadesDasCriaturas.Count)]);
            }

            for (int x = 1; x < 4; x++)
            {
                float rng = Random.Range(0f,1f);
                int indc =(int)( bd.BufferData.Count * rng);
                if(indc > bd.BufferData.Count)
                    indc = bd.BufferData.Count-1;

                if (indc < 0)
                    indc = 0;
                Debug.Log("rng  = " + rng + "    " + bd.BufferData[indc]);
                equipes.inimigos[x].bufferData = bd.BufferData[indc];
                equipes.inimigos[x].level = Random.Range(lvlMin, lvlMax);
                equipes.inimigos[x].completarVida();
                for (int c = 0;c < 2; c++)
                {
                    equipes.inimigos[x].habilidades.Add(habilidadesDasCriaturas[Random.Range(0, habilidadesDasCriaturas.Count)]);
                }
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
