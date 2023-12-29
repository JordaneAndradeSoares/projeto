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
        public float AtaqueEspecial ;
        public float DefesaEspecial ;
        public float AtaqueFisico ;
        public float DefesaFisica ;
        public float RecargaDeEnergiaDePontoZero ;
        public float CapacidadeDeEnergiaDePontoZero ;
        public float ModificadorDeAtaqueBasico;

        public float TaxaDeCrescimentoDaVidaMaxima ;
        public float TaxaDeCrescimentoDaVelocidade ; 
        public float TaxaDeCrescimentoDoAtaqueEspecial ;
        public float TaxaDeCrescimentoDaDefesaEspecial ;
        public float TaxaDeCrescimentoDoAtaqueBasico ;
        public float TaxaDeCrescimentoDaDefesaFisica ;
        public float TaxaDeCrescimentoDaRecargaDeEnergiaDePontoZero ;
        public float TaxaDeCrescimentoDaCapacidadeDeEnergiaDePontoZero ;
        public float TaxaDeCrescimentoDoModificadorDeAtaqueBasico;

        public ScriptavelBuffer evolucao;
        public bool criaturaNoturna;

        public GameObject modelo_3D;
        public Texture iconeMiniatura;
    }
}
