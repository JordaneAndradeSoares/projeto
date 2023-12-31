using Codice.CM.Common;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Explorer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static Buffers.ScriptavelPassivas;

namespace Buffers
{
    [CreateAssetMenu(fileName = "NovaPassiva", menuName = "Buffers/NovaPassiva", order = 1)]

    public class ScriptavelPassivas : ScriptableObject
    {
        public string NomeDaPassiva;
        public List<condicionais> CondicoesDeAtivacao = new List<condicionais>();
        public List<Efeitos_> EfeitosAplicaveis = new List<Efeitos_>();
        public duracao DuracaoDoEfeito;
        public TiposDeAlvo alvo;

        [System.Serializable]
        public class condicionais
        {


            public condicao condicaoDeAtivacao;
            public tiposNatureza CondicaoAtivacaoTipoDeHabilidade;
            public ScriptavelHabilidades CondicaoUsarHabilidade_ReceberHabilidade;
        }

        [System.Serializable]
        public class Efeitos_
        {
            public efeito TipoDeEfeito;
            public ScriptavelHabilidades tipoDeHabilidadeEmModificarEfeitoDaCondicao;
            public statusCriatura TipoDeStatusEmBonusDeStatus;
            public tiposNatureza tipoNatureza;

            public valores valor;

        }
        [System.Serializable]
        public class valores
        {
            public FormaDeValor formato;
            public float quantidade;
        }
        [System.Serializable]
        public class duracao
        {
            public tiposDeDuracao FormatoDaDuracao;
            public float quantidade;
        }

        public int cond_;
        public condicionais tempCond;
        public Efeitos_ tempEfeit;

        public ScriptavelHabilidades tempHabilidade;
        public float quantidade_;

        public duracao Tempduracao_;




    }
    public enum condicao
    {
        Nenhum,
        existir,
        iniciarPartida,
        UsarHabilidade,
        UsarTipoDeHabilidade,
        receberHabilidade,
        ACadaTurno,
    }
    public enum efeito
    {
        Nenhum,
        BonusDeStatus,
        CausarDano,
        GanharEscudo,
        ModificarEfeitoDaCondicao, // ex: modificar o quanto vai ser dado/recebido  um escudo
        BonusDeTipoDehabilidade,
        GanharVida,
        Intarguetavel
    }
    public enum FormaDeValor
    {nenhum,
        AumentarEmX,
        AumentarEmXporcento,
        AumentarEmXporcentoDoValorDeEfeitoAplicadoTotal,
        AumentarEmXporcentoDeVidaFaltante,
        
    }
    public enum statusCriatura
    {
        Nenhum, Vida, velocidade
    }
    public enum tiposDeDuracao {Nenhum, turno, semDuracao }
    public enum TiposDeAlvo {Nenhum, EmSiMesmo, Flexivel, Fileira, Global }

  

    [CustomEditor(typeof(ScriptavelPassivas))]
    public class EditorScriptavelPassivas : Editor
    {

              public SerializedProperty tempCond, tempHabilidade, Tempduracao_,
            CondicoesDeAtivacao, quantidade_, EfeitosAplicaveis, DuracaoDoEfeito, alvo;
        private void OnEnable()
        {
            CondicoesDeAtivacao = serializedObject.FindProperty("CondicoesDeAtivacao");
            tempCond = serializedObject.FindProperty("tempCond");
            tempHabilidade = serializedObject.FindProperty("tempHabilidade");
            quantidade_ = serializedObject.FindProperty("quantidade_");

            EfeitosAplicaveis = serializedObject.FindProperty("EfeitosAplicaveis");
            Tempduracao_ = serializedObject.FindProperty("Tempduracao_");
            
  DuracaoDoEfeito = serializedObject.FindProperty("DuracaoDoEfeito");
            alvo = serializedObject.FindProperty("alvo");
        }


        public override void OnInspectorGUI()
        {

            serializedObject.Update();
         

            ScriptavelPassivas meuScript = (ScriptavelPassivas)target;
            GUILayout.Label("     ~~~~~ criador de  Passivas   ~~~~~");
            EditorGUILayout.Space();

            switch (meuScript.cond_) {
                case 0:
                    EditorGUILayout.Space();
                  
                    if (GUILayout.Button("Iniciar criação de habilidade "))
                    {
                        meuScript.tempCond = new condicionais();
                    
                        meuScript.tempCond.CondicaoAtivacaoTipoDeHabilidade = tiposNatureza.nenhum; // ~triste triste
                        meuScript.cond_ = 1;
                        meuScript.CondicoesDeAtivacao.Clear();

                        meuScript.tempEfeit = new Efeitos_();
                        meuScript.quantidade_ = 0;
                        
                    }
                        break;
                case 1:
                    EditorGUILayout.Space();
                    GUILayout.Label("Condição de ativação  " + meuScript.tempCond.condicaoDeAtivacao);
                  

                    if (meuScript.tempCond.condicaoDeAtivacao != condicao.Nenhum)
                    {
                        switch (meuScript.tempCond.condicaoDeAtivacao) {
                            case condicao.UsarTipoDeHabilidade:

                                EditorGUILayout.Space();
                                if (GUILayout.Button("Cancelar "))
                                {
                                    meuScript.tempCond.condicaoDeAtivacao = condicao.Nenhum;
                                    break;
                                }
                                EditorGUILayout.Space();

                                GUILayout.Label("A condição pede um Tipo de Habilide especifico, qual seria o tipo de habilidade ?");

                               

                                if (meuScript.tempCond.CondicaoAtivacaoTipoDeHabilidade != tiposNatureza.nenhum)
                                {
                                    if (GUILayout.Button("Proximo Parte "))
                                    { 
                                        meuScript.cond_ += 1; }
                                    if (GUILayout.Button("Cancelar"))
                                    { meuScript.cond_ -= 1; }
                                    if (GUILayout.Button("Adicionar mais uma condição"))
                                    {
                                        meuScript.CondicoesDeAtivacao.Add(meuScript.tempCond);
                                        meuScript.tempCond = new condicionais();
                                    }
                                }
                                else
                                {
                                    foreach (tiposNatureza a in Enum.GetValues(typeof(tiposNatureza)))
                                    {
                                        if (a == tiposNatureza.nenhum)
                                            continue;
                                        if (GUILayout.Button("" + a))
                                        {
                                            meuScript.tempCond.CondicaoAtivacaoTipoDeHabilidade = a;
                                        }
                                    }
                                }



                                break;
                            case condicao.UsarHabilidade:


                                EditorGUILayout.Space();
                                if (GUILayout.Button("Cancelar "))
                                {
                                    meuScript.tempCond.condicaoDeAtivacao = condicao.Nenhum;
                                    break;    
                                }

                                EditorGUILayout.Space();
                                
                                GUILayout.Label("A condição pede uma Habilide especifico, qual seria a habilidade ?");
                                EditorGUILayout.Space();
                                serializedObject.Update();
                               
                                EditorGUILayout.PropertyField(tempHabilidade, new GUIContent(""));
                                serializedObject.ApplyModifiedProperties();

                                if (meuScript.tempHabilidade != null)
                                {
                                    meuScript.tempCond.CondicaoUsarHabilidade_ReceberHabilidade = meuScript.tempHabilidade;

                                    if (GUILayout.Button("Proximo Parte "))
                                    { 
                                        meuScript.cond_ += 1; }
                                    if (GUILayout.Button("Cancelar"))
                                    { meuScript.cond_ -= 1; }
                                    if (GUILayout.Button("Adicionar mais uma condição"))
                                    {
                                        meuScript.CondicoesDeAtivacao.Add(meuScript.tempCond);
                                        meuScript.tempCond = new condicionais();
                                    }
                                    if (meuScript.cond_>1)
                                        meuScript.tempHabilidade = null;
                                }


                                break;
                            case  condicao.receberHabilidade:


                                EditorGUILayout.Space();
                                if (GUILayout.Button("Cancelar "))
                                {
                                    meuScript.tempCond.condicaoDeAtivacao = condicao.Nenhum;
                                    break;
                                }

                                EditorGUILayout.Space();

                                GUILayout.Label("A condição pede uma Habilide especifico, qual seria a habilidade ?");
                                EditorGUILayout.Space();
                                serializedObject.Update();

                                EditorGUILayout.PropertyField(tempHabilidade, new GUIContent(""));
                                serializedObject.ApplyModifiedProperties();

                                if (meuScript.tempHabilidade != null)
                                {
                                    meuScript.tempCond.CondicaoUsarHabilidade_ReceberHabilidade = meuScript.tempHabilidade;

                                    if (GUILayout.Button("Proximo Parte "))
                                    {
                                        meuScript.cond_ += 1; }
                                    if (GUILayout.Button("Cancelar"))
                                    { meuScript.cond_ -= 1; }
                                    if (GUILayout.Button("Adicionar mais uma condição"))
                                    {
                                        meuScript.CondicoesDeAtivacao.Add(meuScript.tempCond);
                                        meuScript.tempCond = new condicionais();
                                    }
                                    if (meuScript.cond_ > 1)
                                        meuScript.tempHabilidade = null;
                                }


                                break;
                            default:

                                if (GUILayout.Button("Proximo Parte "))
                                {  
                                    meuScript.cond_ += 1; }
                                if (GUILayout.Button("Cancelar"))
                                { meuScript.cond_ -= 1; }
                                if (GUILayout.Button("Adicionar mais uma condição"))
                                {
                                    meuScript.CondicoesDeAtivacao.Add(meuScript.tempCond);
                                    meuScript.tempCond = new condicionais();
                                }


                                break;
                        }

                       
                    }
                    else
                    {
                        EditorGUILayout.Space();
                        GUILayout.Label(" Qual a condição para que o efeito inicie ?");
                        foreach (condicao a in Enum.GetValues(typeof(condicao)))
                        {
                            if (a == condicao.Nenhum)
                                continue;
                            if (GUILayout.Button("" + a))
                            {
                                meuScript.tempCond.condicaoDeAtivacao = a;

                            }

                        }
                    }
                   
                 


                  
                    if(meuScript.cond_ >1)
                    {
                        meuScript.CondicoesDeAtivacao.Add(meuScript.tempCond);
                    }
                    break;
                case 2:
                    serializedObject.Update();
                  
                    EditorGUILayout.PropertyField(CondicoesDeAtivacao, new GUIContent("Condições de ativação "));
                    serializedObject.ApplyModifiedProperties();
                    if (GUILayout.Button(" !! Reiniciar !! "))
                    {
                        meuScript.cond_ = 0;
                        break;
                    }

                    EditorGUILayout.Space();
                    GUILayout.Label("Efeitos da habilidade");
                    bool flag = false;
                    if(meuScript.tempEfeit.TipoDeEfeito != efeito.Nenhum)
                    {
                        switch (meuScript.tempEfeit.TipoDeEfeito) {
                     
                            case efeito.ModificarEfeitoDaCondicao :


                                EditorGUILayout.Space();
                                if (GUILayout.Button("Cancelar "))
                                {
                                    meuScript.tempEfeit.TipoDeEfeito = efeito.Nenhum;
                                    break;
                                }

                                EditorGUILayout.Space();

                                GUILayout.Label("O efeito vai alterar os valores de uma Habilidade especifica ");
                                EditorGUILayout.Space();
                                serializedObject.Update();

                                EditorGUILayout.PropertyField(tempHabilidade, new GUIContent(""));
                                serializedObject.ApplyModifiedProperties();

                                if (meuScript.tempHabilidade != null)
                                {
                                    meuScript.tempEfeit.tipoDeHabilidadeEmModificarEfeitoDaCondicao = meuScript.tempHabilidade;
                                    flag = true;
                                }

                                break;
                            case efeito.BonusDeStatus :
                           

                                       EditorGUILayout.Space();
                                if (GUILayout.Button("Cancelar "))
                                {
                                    meuScript.tempEfeit.TipoDeEfeito = efeito.Nenhum;
                                    break;
                                }
                                EditorGUILayout.Space();

                                GUILayout.Label("O efeito ira altrar o status de um buffer ");



                                if (meuScript.tempEfeit.TipoDeStatusEmBonusDeStatus!= statusCriatura.Nenhum)
                                {
                                  
                                    flag = true;
                                    
                                }
                                else
                                {
                                    foreach (statusCriatura a in Enum.GetValues(typeof(statusCriatura)))
                                    {
                                        if (a == statusCriatura.Nenhum)
                                            continue;
                                        if (GUILayout.Button("" + a))
                                        {
                                            meuScript.tempEfeit.TipoDeStatusEmBonusDeStatus= a;
                                        }
                                    }
                                }
                                break;
                            case efeito.BonusDeTipoDehabilidade:


                                EditorGUILayout.Space();
                                if (GUILayout.Button("Cancelar "))
                                {
                                    meuScript.tempEfeit.TipoDeEfeito = efeito.Nenhum;
                                    break;
                                }
                                EditorGUILayout.Space();

                                GUILayout.Label("O efeito Depende de um Tipo de habilidade");



                                if (meuScript.tempEfeit.tipoNatureza != tiposNatureza.nenhum)
                                {

                                    flag = true;

                                }
                                else
                                {

                                    foreach (tiposNatureza a in Enum.GetValues(typeof(tiposNatureza)))
                                    {
                                        if (a == tiposNatureza.nenhum)
                                            continue;
                                        if (GUILayout.Button("" + a))
                                        {
                                            meuScript.tempEfeit.tipoNatureza = a;
                                        }
                                    }

                                }

                                break;
                            default:
                                flag = true;
                                break;
                        }

                    }
                    else
                    {
                        EditorGUILayout.Space();
                        GUILayout.Label("Qaul vai ser o efeito ?");
                        foreach (efeito a in Enum.GetValues(typeof(efeito)))
                        {
                            if (a == efeito.Nenhum)
                                continue;
                            if (GUILayout.Button("" + a))
                            {
                                meuScript.tempEfeit.TipoDeEfeito = a;

                            }

                        }
                    }

                    if (flag)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        GUILayout.Label("O valor do efeito sera baseado em :");

                        if(meuScript.tempEfeit.valor.formato != FormaDeValor.nenhum) {
                            serializedObject.Update();
                            EditorGUILayout.PropertyField(quantidade_, new GUIContent("Porcentagem da fonte :  "));
                            serializedObject.ApplyModifiedProperties();
                        } else { 
                        foreach (FormaDeValor a in Enum.GetValues(typeof(FormaDeValor)))
                        {
                            if (a == FormaDeValor.nenhum)
                                continue;
                                if (GUILayout.Button("" + a))
                                {
                                    meuScript.tempEfeit.valor.formato = a;
                                }
                            }

                        }
                        if(meuScript.quantidade_ != 0)
                        {

                            if (GUILayout.Button("Proximo Parte "))
                            {
                                meuScript.quantidade_ = 0;
                                meuScript.cond_ += 1;
                            }
                            if (GUILayout.Button("Cancelar"))
                            {
                              
                                meuScript.tempEfeit = new Efeitos_();
                                meuScript.quantidade_ = 0;
                            
                            }

                            if (GUILayout.Button("Adicionar efeito"))
                            {
                                meuScript.tempEfeit.valor.quantidade = meuScript.quantidade_;
                                meuScript.EfeitosAplicaveis.Add(meuScript.tempEfeit);
                                meuScript.tempEfeit = new Efeitos_();
                                meuScript.quantidade_ = 0;
                            }
                            //                            meuScript.tempEfeit.valor.quantidade
                        }
                        if (meuScript.cond_ > 2)
                        {

                            meuScript.tempEfeit.valor.quantidade = meuScript.quantidade_;
                            meuScript.EfeitosAplicaveis.Add(meuScript.tempEfeit);


                        }
                        if(meuScript.cond_ < 2)
                        {
                            meuScript.tempEfeit = new Efeitos_();
                            meuScript.quantidade_ = 0;
                        }
                      

                    }


                    break;
                case 3:

                    EditorGUILayout.Space();
                    serializedObject.Update();

                    EditorGUILayout.PropertyField(CondicoesDeAtivacao, new GUIContent("Condições de ativação "));
                    EditorGUILayout.PropertyField(EfeitosAplicaveis, new GUIContent("Efeitos "));
                    serializedObject.ApplyModifiedProperties();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    GUILayout.Label("A duracao da passiva");

                    bool flag2 = false;
                    if(meuScript.Tempduracao_.FormatoDaDuracao != tiposDeDuracao.Nenhum)
                    {
                        switch (meuScript.Tempduracao_.FormatoDaDuracao)
                        {
                            case tiposDeDuracao.turno:

                                serializedObject.Update();
                             
                                EditorGUILayout.PropertyField(quantidade_, new GUIContent("Quantos turnos ? :  "));
                                serializedObject.ApplyModifiedProperties();

                                if(meuScript.quantidade_ > 0)
                                {
                                    meuScript.Tempduracao_.quantidade = (int)meuScript.quantidade_;
                                    flag2 = true;
                                }

                                break;
                            default:
                                flag2 = true;
                                break;
                        }
                    }
                    else
                    {
                        foreach (tiposDeDuracao a in Enum.GetValues(typeof(tiposDeDuracao)))
                        {
                            if (a == tiposDeDuracao.Nenhum)
                                continue;
                            if (GUILayout.Button("" + a))
                            {
                                meuScript.Tempduracao_.FormatoDaDuracao = a;
                            }
                        }

                    }

                    if (flag2)
                    {
                        //terminar essa merda


                        if (GUILayout.Button("Proximo Parte "))
                        {
                            meuScript.DuracaoDoEfeito = meuScript.Tempduracao_;
                            meuScript.quantidade_ = 0;
                           
                            meuScript.cond_ += 1;
                        }
                        if (GUILayout.Button("Cancelar"))
                        {
                            meuScript.Tempduracao_ = new duracao();
                            meuScript.quantidade_ = 0;

                        }

                       

                    }


                    break;
                case 4:
                    EditorGUILayout.Space();
                    serializedObject.Update();

                    EditorGUILayout.PropertyField(CondicoesDeAtivacao, new GUIContent("Condições de ativação "));
                    EditorGUILayout.PropertyField(EfeitosAplicaveis, new GUIContent("Efeitos "));
                    EditorGUILayout.PropertyField(DuracaoDoEfeito, new GUIContent("A duração da passiva "));
                    
                    serializedObject.ApplyModifiedProperties();

                    if(meuScript.alvo != TiposDeAlvo.Nenhum)
                    {
                        if (GUILayout.Button("Terminar"))
                        {
                            meuScript.cond_ += 1;
                        }
                        if (GUILayout.Button("cancelar"))
                        {
                            meuScript.alvo = TiposDeAlvo.Nenhum;
                        }
                    }
                    else
                    {
                        EditorGUILayout.Space();
                        GUILayout.Label(" Qual Vai ser o alvo do efeito da passiva ?");
                        foreach (TiposDeAlvo a in Enum.GetValues(typeof(TiposDeAlvo)))
                        {
                            if (a == TiposDeAlvo.Nenhum)
                                continue;
                            if (GUILayout.Button("" + a))
                            {
                                meuScript.alvo = a;

                            }

                        }

                    }
                    break;
                case 5:

                    EditorGUILayout.Space();
                    serializedObject.Update();

                    EditorGUILayout.PropertyField(CondicoesDeAtivacao, new GUIContent("Condições de ativação "));
                    EditorGUILayout.PropertyField(EfeitosAplicaveis, new GUIContent("Efeitos "));
                    EditorGUILayout.PropertyField(DuracaoDoEfeito, new GUIContent("A duração da passiva "));
                    EditorGUILayout.PropertyField(alvo, new GUIContent("O alvo do efeito da passiva "));

                    serializedObject.ApplyModifiedProperties();
                    break;
            }



            foreach (var a in Enum.GetValues(typeof(condicao)))
            {

            }
            serializedObject.ApplyModifiedProperties();
        }



    }
}
