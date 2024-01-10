using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buffers
{
    [System.Serializable]
    public class Kernel 
    {
        public ScriptavelBuffer bufferData;
        public int level;
        public float vida,vida_maxima;

        public List<escudo> escudos = new List<escudo>();
       
        public class escudo
        {
            public Kernel origem;
            public float escudo_, escudo_maximo;
            public int turnos;

            public escudo(Kernel origem, float escudo_,int turnos)
            {
                this.origem = origem;
                this.escudo_ = escudo_;
                this.escudo_maximo = escudo_;
                this.turnos = turnos;
            }
        }

        public List<ScriptavelHabilidades> habilidades = new List<ScriptavelHabilidades>();
       
        

        // falta as passivas

        public Kernel(ScriptavelKernel SK)
        {
       bufferData = SK.bufferData;
            level = SK.level ;
            vida = bufferData.VidaMaxima * (1 + (bufferData.TaxaDeCrescimentoDaVidaMaxima * level));
            vida_maxima = bufferData.VidaMaxima * (1 + (bufferData.TaxaDeCrescimentoDaVidaMaxima * level));
            habilidades = SK.habilidades;
    }
       
    }
}
