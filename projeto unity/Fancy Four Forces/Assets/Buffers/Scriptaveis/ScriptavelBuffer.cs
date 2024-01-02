using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Buffers
{
    [CreateAssetMenu(fileName = "NovoBuffer", menuName = "Buffers/novoBuffer", order = 1)]
    public class ScriptavelBuffer : ScriptableObject
    {
        public string Nome;
        public int Numero;

        public float VidaMaxima;
        public float Velocidade;

        public float AtaqueFisico;
        public float DefesaFisica;
        public float Estamina;


        public float TaxaDeCrescimentoDaVidaMaxima ;
        public float TaxaDeCrescimentoDaVelocidade ;

        public float TaxaDeCrescimentoDoAtaqueBasico;
        public float TaxaDeCrescimentoDaDefesaFisica;
        public float TaxaDeCrescimentoDaEstamina;

        public ScriptavelAtaqueBasico AtaqueBasico;
        public TipoDeAtaque TipoDeEfetividade;

        public bool evolui;

        public bool criaturaNoturna;

        public GameObject modelo_3D;
        public Texture iconeMiniatura;
    }
}
