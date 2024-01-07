using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gerenciadorWFC : MonoBehaviour
{
    public sudoki_Cluester sdk_cluster;
    private sudoku sdk;
    public int tamanho;
    public float esca;
    //  public int Qiniciais;


    public bool gerarPre, gerarAutomatico;
    private List<SDk_estruturaGerada> presetados = new List<SDk_estruturaGerada>();
    private void Start()
    {
        sdk = gameObject.AddComponent<sudoku>();
        sdk.iniciar(sdk_cluster, tamanho,esca);
    }

    public void botaremSDK(SDk_estruturaGerada aux)
    {
        presetados.Add(aux);
    }
   

    void Update()
    {

       
        
        if(gerarPre)
        {
            gerarPre = false;
            foreach(SDk_estruturaGerada aux in presetados)
            {
                sdk.botarNalista(aux.sdkM, aux.transform.position,aux.gameObject);
            }
            gerarAutomatico = true;

        }
        if (gerarAutomatico)
        {
            if (presetados.Count > 0)
            {

                sdk.MelhorResposta();
            }
            else
            {
                //                                x/y
                    Vector3 localnv = new Vector3(0, 0, 0);
                    ConstrutorSudoku aux_ = new ConstrutorSudoku();
                    aux_.provavel = new List<sudoku_modulo>();
                    aux_.provavel.AddRange(sdk_cluster.Lista);
                    aux_.indice = new Vector2(localnv.x, localnv.y);



              
                sdk.novaestrutura(sdk_cluster.Lista[ sdk.calcularProvaveis(aux_)], localnv);

           

     
                presetados.Add(null);
              
            }
        }

    }
}
