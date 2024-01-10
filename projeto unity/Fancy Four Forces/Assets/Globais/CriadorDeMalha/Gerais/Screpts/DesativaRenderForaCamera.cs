using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfiguracoesDoMundo
{
    public class DesativaRenderForaCamera : MonoBehaviour
    {
  
     
        public Renderer rd;
        public Renderer[] gms;
     
        private void Start()
        {
            gms = GetComponentsInChildren<Renderer>();
            List<Renderer> childRenderersOnly = new List<Renderer>();
            foreach (Renderer renderer in gms)
            {
                if (renderer.gameObject != gameObject)
                {
                    childRenderersOnly.Add(renderer);
                }
            }
            gms = childRenderersOnly.ToArray();



        }
        bool ativ;
        private void Update()
        {
           
            bool aux = rd.isVisible;
            
            if (ativ != rd.isVisible)
            {
            ativ = aux;
                mudarRender(ativ);  
            }

        }
        
        private void mudarRender(bool aux)
        {

            foreach(Renderer go in gms)
            {
                go.enabled = aux;
            }
           
        }


    } 


}
