using Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace modoBatalha
{
    public class AuxMostrarResumo : MonoBehaviour
    {
        public TextMeshProUGUI txtA, txtB;
        public void mostrar(AuxUiHabilidade a)
        {
            string tA = " ";
            string tB = "";

       


            // habilidade 
            if (a.SH != null)
            {
                tA += "A Habilidade ";
                tB += "A Habilidade Gastará ";

                switch (a.SH._Efeito)
                {

                    case (Efeito.Dano):
                        tA += "Causara Dano ";

                        break;
                    case (Efeito.MudarStatus):
                        tA += "Aumentara o status ";
                        break;
                    case (Efeito.Escudo):
                        tA += "Dara um Escudo ";
                        break;

                }

                switch (a.SH._Alvo)
                {
                    case (Alvo.Unico):
                        tA += "No alvo em";
                        break;
                    case (Alvo.Global):
                        tA += "Em cada alvo em";
                        break;

                }
                // dano

                switch (a.SH._Efeito)
                {
                    case (Efeito.Dano):

                        float danofinal = a.GB.ultimobuffer.danoBruto(a.SH);

                        tA +="  "+ danofinal;
                        break;
                    case (Efeito.MudarStatus):

                        switch (a.SH.___StatusAAlterar)
                        {
                            case StatusAAlterar.Velocidade:

                                tA += "um valor ai";
                                break;


                        }

                        break;
                    case (Efeito.Escudo):
                        tA += "  " +a.SH.porcentagemDoEfeito *
                     (a.GB.ultimobuffer.data.bufferData.AtaqueFisico * (a.GB.ultimobuffer.data.bufferData.TaxaDeCrescimentoDoAtaqueBasico *
                     a.GB.ultimobuffer.data.level));
                        break;


                }

                tB += "  " + a.SH.GastoDeHabilidade;

            }
            // atk basico
            else
            {
                float danofinal = a.SAB.porcentagemDoEfeito * a.GB.ultimobuffer.data.bufferData.AtaqueFisico * (1 + a.GB.ultimobuffer.data.level * a.GB.ultimobuffer.data.bufferData.TaxaDeCrescimentoDoAtaqueBasico);

                tA += "O ataque basico causara dano no alvo de " + danofinal;

                tB += "o Ataque ira recuperar ";
                tB += a.SAB.ValorDeRecarga;
            }
            txtA.text = tA;
            txtB.text = tB +" de energia";


        }
        
    }
}
