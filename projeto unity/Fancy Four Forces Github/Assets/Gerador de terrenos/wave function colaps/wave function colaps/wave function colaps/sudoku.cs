using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * sudoku deve ser criado como um objeto
 * ao criar o objeto é necessario indicar um sudoki_cluster que é a lista de todos os modulos do sudoku, e tambem o tamanho deste sudoku
 * !! este sudoku é baseado em que todas as possibilidades são 0 ate que tenha um novo mod que as modifique
 * então é obrigatorio ter algum mod instanciado antes de iniciar as instancias automaticas
 * 
 * 
 * 
 * 
 * [passo a passo ]
 * - 1° crie esta classe como um objeto ou inicie a função " iniciar "
 * 
 *  -> novaestrutura()  -- para adicionar manualmente
 *  -> MelhorResposta() -- para instanciar automaticamente
 *  
 *  -> objetos ja instanciado devem usar botarNalista para reservar o local
 *  
 *  caso queira que a geração seja baseada em algum referencial:
 *  -> observador -- ponto de referencia
 *  -> limitador -- tamanho do circulo
 */
public class sudoku : MonoBehaviour
{
    public List<sudoku_modulo> modulos = new List<sudoku_modulo>();
    public List<List<ConstrutorSudoku>> matriz;
    private List<ConstrutorSudoku> matriz_;


    
    private int tamanho,limitador;
    private int senoide =3;
    public float escala;
    private GameObject observador;


    //                     y / x

    
    public void iniciar(sudoki_Cluester aux, int tamanho_, float escala_)
    {

        matriz = new List<List<ConstrutorSudoku>>();
        matriz_ = new List<ConstrutorSudoku>();
        tamanho = tamanho_;
        escala = escala_;
      
        modulos = aux.Lista;
        Debug.Log(modulos + "   a    " + aux);
        Debug.Log(aux.Lista + " <--------");

        for (int y = 0; y < tamanho; y++)
        {
            matriz.Add(new List<ConstrutorSudoku>());
            for (int x = 0; x < tamanho; x++)
            {
                matriz[y].Add(new ConstrutorSudoku());

                matriz[y][x].indice = new Vector2(x, y);





                matriz_.Add(matriz[y][x]);
            }

        }
    }
    public sudoku(sudoki_Cluester aux,int tamanho_,float escala_)
    {

         matriz = new List<List<ConstrutorSudoku>>();
        matriz_ = new List<ConstrutorSudoku>();
      tamanho = tamanho_;
      modulos = aux.Lista;
        escala = escala_;


        for (int y = 0; y < tamanho; y++)
        {          
            matriz.Add(new List<ConstrutorSudoku>());         
            for (int x = 0; x < tamanho; x++)
            {   
                matriz[y].Add(new ConstrutorSudoku());

                matriz[y][x].indice = new Vector2(x,y);
               
               
                
              
                
                matriz_.Add(matriz[y][x]);
            }
           
        }

    }
  public sudoku ( sudoki_Cluester aux2,GameObject aux,int raio){
observador = aux;
limitador = raio;
modulos = aux2.Lista;

  }

   
    public void botarNalista(sudoku_modulo aux,Vector3 auxV_,GameObject gm)
    {

        Vector3 auxV = calcularIndice( auxV_ ) ;
        Debug.Log(auxV);
        calcularProbabilidades(0);
        if (matriz[(int)auxV.y][(int)auxV.x].respostaProvavel().Contains(aux)
             || matriz[(int)auxV.y][(int)auxV.x].respostaProvavel().Count == 0)
        {


            criarEstrutura(matriz[(int)auxV.y][(int)auxV.x], modulos.IndexOf(aux), auxV,gm);
            

        }
        else
        {

           
        }


    }
    private void criarEstrutura(ConstrutorSudoku constucto, int indice, Vector3 local,GameObject gm)
    {



        if (constucto.alvo != null)
        {
            Destroy(constucto.alvo);
        }
        constucto.verificado = false;
        constucto.alvo = gm;

        constucto.definirCada(modulos[indice]);




        for (int g = -1; g < 2; g += 2)
        {
            if (constucto.indice.y != 0 && g == -1)
            {
                if (matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].alvo == null)
                {

                    matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].adicionar(g, -1, g == -1 ? constucto.somente_embaixo[0].somente_embaixo : constucto.somente_emcima[0].somente_emcima);
                    matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].removertodos(g, -1, g == -1 ? constucto.somente_embaixo[0].somente_embaixo : constucto.somente_emcima[0].somente_emcima);
                    matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].verificado = true;
                    matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].respostaProvavel();

                }

            }
            if (constucto.indice.x != 0 && g == -1)
            {

                if (matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].alvo == null)
                {


                    matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].adicionar(g, 1, g == -1 ? constucto.somente_esquerda[0].somente_esquerda : constucto.somente_direita[0].somente_direita);
                    matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].removertodos(g, 1, g == -1 ? constucto.somente_esquerda[0].somente_esquerda : constucto.somente_direita[0].somente_direita);
                    matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].verificado = true;
                    matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].respostaProvavel();
                }
            }

        }
    }

    public bool novaestrutura(sudoku_modulo aux,Vector3 auxV_)
    {
        Vector3 auxV = auxV_ + new Vector3(0,0,auxV_.y);
        Debug.Log(auxV);
        calcularProbabilidades(0);
       if(matriz[(int)auxV.y][(int)auxV.x].respostaProvavel().Contains(aux) 
            ||matriz[(int)auxV.y][(int)auxV.x].respostaProvavel().Count ==0 ){


            criarEstrutura(matriz[(int)auxV.y][(int)auxV.x], modulos.IndexOf(aux),auxV);
            return true;

       }else{

        return false;
       }
            

       
    }
    public ConstrutorSudoku proximaContrucao(){
        
        ConstrutorSudoku auxf = new ConstrutorSudoku();

            calcularProbabilidades(0);
            List<ConstrutorSudoku> filtro = new List<ConstrutorSudoku>();
            filtro.AddRange(matriz_.FindAll(x => x.verificado == true && x.contador > 0));
            //filtro.AddRange(matriz.Find(x => x.Exists(c=> c.alvo == null) ));
            if (filtro.Count > 0)
            {



                int auxi = filtro[0].contador;
                float menor_distancia = filtro[0].distancia;
                ConstrutorSudoku aux2 = null;

                if (observador == null)
                {
                    foreach (ConstrutorSudoku aux in filtro)
                    {

                        if (aux.respostaProvavel().Count < auxi)
                        {
                            auxi = aux.contador;

                        }

                        if (aux.contador == 1)
                        {
                            break;
                        }

                    }
                    filtro.RemoveAll(x => x.contador != auxi);
                    aux2 = filtro[filtro.Count == 1 ? 0 : Random.Range(0, filtro.Count)];
                    // Debug.Log("possiveis :" + filtro.Count + "  com : " + auxi);
                }
                else
                {
                    filtro.RemoveAll(x => x.respostaProvavel().Count <= 0);
                    foreach (ConstrutorSudoku aux in filtro)
                    {
                        aux.distancia = Vector3.Distance(aux.visual.transform.position, observador.transform.position);
                        if (aux.distancia < menor_distancia || aux == filtro[0])
                        {
                            menor_distancia = aux.distancia;
                        } 
                    }
                    aux2 = filtro.Find(x => limitador == 0 ? x.distancia == menor_distancia : (x.distancia == menor_distancia) && (x.distancia < limitador));

                    Debug.Log("possiveis :" + filtro.Count + "  com : " + menor_distancia + "m" + (aux2 == null ? "erro " : " ok"));
                    
                }
        if(aux2 != null){
               try
                {
                    int aux3 = modulos.IndexOf(aux2.respostaProvavel()[(aux2.provavel.Count == 1 ? 0 : Random.Range(0, aux2.provavel.Count))]);

                    auxf = matriz[(int)aux2.indice.y][(int)aux2.indice.x];
                    auxf.indice_modulo = aux3;
                   
                }
                catch
                {
                    
                } }
            

            }
            else
            {
               
                tirar_c();
               
                
                
            }
      
        return auxf;
    }

    public void novaestrutura(sudoku_modulo aux)
    {
      MelhorResposta(aux);
    }
    private void criarEstrutura (ConstrutorSudoku constucto ,int indice,Vector3 local)
    {
       
      
      
        if(constucto.alvo != null)
        {
            Destroy(constucto.alvo);
        }
        constucto.verificado = false;
        constucto.alvo = Instantiate(modulos[indice].modulo, calcularLocal(local), Quaternion.identity,transform);

       
        constucto.definirCada(modulos[indice]);

           


           for (int g = -1; g < 2; g += 2)
            {
                if (constucto.indice.y != 0 && g == -1 )
                {
                    if (matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].alvo == null)
                    {

                        matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].adicionar(g, -1, g == -1 ? constucto.somente_embaixo[0].somente_embaixo : constucto.somente_emcima[0].somente_emcima);
                        matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].removertodos(g, -1, g == -1 ? constucto.somente_embaixo[0].somente_embaixo : constucto.somente_emcima[0].somente_emcima);
                        matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].verificado = true;
                        matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].respostaProvavel();

                    }

                }
                if (constucto.indice.x != 0 && g == -1 )
                {

                    if (matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].alvo == null )
                    {


                        matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].adicionar(g, 1, g == -1 ? constucto.somente_esquerda[0].somente_esquerda : constucto.somente_direita[0].somente_direita);
                        matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].removertodos(g, 1, g == -1 ? constucto.somente_esquerda[0].somente_esquerda : constucto.somente_direita[0].somente_direita);
                        matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].verificado = true;
                        matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].respostaProvavel();
                    }
                }

            }


      

        
       
    }
  
   private void criarEstrutura (ConstrutorSudoku constucto ,int indice)
    {
       
      //  constucto.probabilidades.Clear();
       
       // constucto.probabilidades.Add(modulos[indice]);
      
        if(constucto.alvo != null)
        {
            Destroy(constucto.alvo);
        }
        constucto.verificado = false;
        constucto.alvo = Instantiate(modulos[indice].modulo,calcularLocal( new Vector3(constucto.indice.x,0,constucto.indice.y)), Quaternion.identity,transform);

   
        constucto.definirCada(modulos[indice]);

           


           for (int g = -1; g < 2; g += 2)
            {
                if (constucto.indice.y != 0 && g == -1 )
                {
                    if (matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].alvo == null)
                    {

                        matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].adicionar(g, -1, g == -1 ? constucto.somente_embaixo[0].somente_embaixo : constucto.somente_emcima[0].somente_emcima);
                        matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].removertodos(g, -1, g == -1 ? constucto.somente_embaixo[0].somente_embaixo : constucto.somente_emcima[0].somente_emcima);
                        matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].verificado = true;
                        matriz[(int)constucto.indice.y + g][(int)constucto.indice.x].respostaProvavel();

                    }

                }
                if (constucto.indice.x != 0 && g == -1 )
                {

                    if (matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].alvo == null )
                    {


                        matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].adicionar(g, 1, g == -1 ? constucto.somente_esquerda[0].somente_esquerda : constucto.somente_direita[0].somente_direita);
                        matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].removertodos(g, 1, g == -1 ? constucto.somente_esquerda[0].somente_esquerda : constucto.somente_direita[0].somente_direita);
                        matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].verificado = true;
                        matriz[(int)constucto.indice.y][(int)constucto.indice.x + g].respostaProvavel();
                    }
                }

            }


      

        
       
    }
  
    

    public void calcularProbabilidades(int aaa) // <==== dar uma olhada aqui 
    {

        List<ConstrutorSudoku> lista = new List<ConstrutorSudoku>();
        lista.AddRange(matriz_.FindAll(x => x.alvo != null && x.gerado_verificado == false));
      

        // os n�o nulos passam para os nulos as probabilidades
        foreach (ConstrutorSudoku aux in lista)
        {
            for (int g = -1; g < 2; g += 2)
            {
                if (aux.indice.y != 0 && g == -1 || aux.indice.y != tamanho - 1 && g == 1)
                {
                    if (matriz[(int)aux.indice.y + g][(int)aux.indice.x].alvo == null)
                    {

                        matriz[(int)aux.indice.y + g][(int)aux.indice.x].adicionar(g, -1, g == -1 ? aux.somente_embaixo[0].somente_embaixo : aux.somente_emcima[0].somente_emcima);
                        matriz[(int)aux.indice.y + g][(int)aux.indice.x].removertodos(g, -1, g == -1 ? aux.somente_embaixo[0].somente_embaixo : aux.somente_emcima[0].somente_emcima);
                        matriz[(int)aux.indice.y + g][(int)aux.indice.x].verificado = true;
                        matriz[(int)aux.indice.y + g][(int)aux.indice.x].respostaProvavel();

                    }

                }
                if (aux.indice.x != 0 && g == -1 || aux.indice.x != tamanho - 1 && g == 1)
                {

                    if (matriz[(int)aux.indice.y][(int)aux.indice.x + g].alvo == null )
                    {


                        matriz[(int)aux.indice.y][(int)aux.indice.x + g].adicionar(g, 1, g == -1 ? aux.somente_esquerda[0].somente_esquerda : aux.somente_direita[0].somente_direita);
                        matriz[(int)aux.indice.y][(int)aux.indice.x + g].removertodos(g, 1, g == -1 ? aux.somente_esquerda[0].somente_esquerda : aux.somente_direita[0].somente_direita);
                        matriz[(int)aux.indice.y][(int)aux.indice.x + g].verificado = true;
                        matriz[(int)aux.indice.y][(int)aux.indice.x + g].respostaProvavel();
                    }
                }

            }
            aux.gerado_verificado = true;

        }

        foreach (ConstrutorSudoku aux in lista)
        {
            //direita
            if (aux.indice.x < tamanho - 2)
            {
                for (int x = 1; x < tamanho - aux.indice.x; x++)
                {
                    List<sudoku_modulo> aux2 = new List<sudoku_modulo>();

                   
                    if (matriz[(int)aux.indice.y][(int)aux.indice.x + x].alvo != null)
                    {
                        break;}
                    else
                    {
                        foreach (sudoku_modulo aux3 in matriz[(int)aux.indice.y][(int)aux.indice.x + x - 1].respostaProvavel())
                        {
                            aux2.AddRange(aux3.somente_direita.FindAll(c => aux2.Contains(c) == false));
                        }

                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].adicionar(1, 1, aux2);
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].respostaProvavel();


                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].verificado = true;
                        if(aux2.Count == modulos.Count)
                        {
                           break;
                        }
                    }
                }
            }
            //esquerda
            if (aux.indice.x > 1)
            {
                for (int x = -1;aux.indice.x + x > -1 ; x--)
                {
                    List<sudoku_modulo> aux2 = new List<sudoku_modulo>();

                    foreach (sudoku_modulo aux3 in matriz[(int)aux.indice.y][(int)aux.indice.x + x + 1].respostaProvavel()) {
                        aux2.AddRange(aux3.somente_esquerda.FindAll(c=> aux2.Contains(c)==false));
                    }

                   

                    if (matriz[(int)aux.indice.y][(int)aux.indice.x + x].alvo != null)
                    {
                       
                        break;
                    }
                    else
                    {
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].adicionar(-1, 1, aux2);
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].respostaProvavel();
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].verificado = true;
                       
                    }
                    if (aux2.Count == modulos.Count)
                    {
                        break;
                    }
                }
            }

            //emcima
            if (aux.indice.y < tamanho - 2)
            {
                for (int x = 1; x < (tamanho )- aux.indice.y; x++)
                {
                    List<sudoku_modulo> aux2 = new List<sudoku_modulo>();

                    foreach (sudoku_modulo aux3 in matriz[(int)aux.indice.y + x - 1][(int)aux.indice.x].respostaProvavel())
                    {
                        aux2.AddRange(aux3.somente_emcima.FindAll(c => aux2.Contains(c) == false));
                    }

                   
                    if (matriz[(int)aux.indice.y + x][(int)aux.indice.x].alvo != null)
                    {
                        
                        break;
                    }
                    else
                    {
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].adicionar(1, -1, aux2);
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].respostaProvavel();

                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].verificado = true;
                    }
                    if (aux2.Count == modulos.Count)
                    {
                        break;
                    }
                }
            }
            //embaixo
            if (aux.indice.y > 1)
            {
                for (int x = -1;  aux.indice.y + x > -1; x--)
                {

                    List<sudoku_modulo> aux2 = new List<sudoku_modulo>();

                    foreach (sudoku_modulo aux3 in matriz[(int)aux.indice.y + x + 1][(int)aux.indice.x].respostaProvavel())
                    {
                        aux2.AddRange(aux3.somente_embaixo.FindAll(c => aux2.Contains(c) == false));
                    }

                    

                    if (matriz[(int)aux.indice.y + x][(int)aux.indice.x].alvo != null)
                    {
                        
                        break;
                    }
                    else
                    {

                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].adicionar(-1, -1, aux2);
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].respostaProvavel();
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].verificado = true;
                        
                    }
                    if (aux2.Count == modulos.Count)
                    {
                        break;
                    }//
                }
            }
        }

        
        lista.Clear();
        lista.AddRange(matriz_.FindAll(x => x.verificado == true && x.alvo == null));

        foreach (ConstrutorSudoku aux in lista)
        {
            //direita
            if (aux.indice.x < tamanho - 2)
            {
                for (int x = 1; x < tamanho - aux.indice.x && Mathf.Abs(x) <  senoide; x++)
                {
                    List<sudoku_modulo> aux2 = new List<sudoku_modulo>();


                    if (matriz[(int)aux.indice.y][(int)aux.indice.x + x].alvo != null ||
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].verificado == false)
                    {
                        break;
                    }
                    else
                    {
                        foreach (sudoku_modulo aux3 in matriz[(int)aux.indice.y][(int)aux.indice.x + x - 1].respostaProvavel())
                        {
                            aux2.AddRange(aux3.somente_direita.FindAll(c => aux2.Contains(c) == false));
                        }

                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].adicionar(1, 1, aux2);
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].respostaProvavel();


                      //  matriz[(int)aux.indice.y][(int)aux.indice.x + x].verificado = true;
                        if (aux2.Count == modulos.Count)
                        {
                            break;
                        }
                    }
                }
            }
            //esquerda
            if (aux.indice.x > 1)
            {
                for (int x = -1; aux.indice.x + x > -1 && Mathf.Abs(x) < senoide; x--)
                {
                    List<sudoku_modulo> aux2 = new List<sudoku_modulo>();

                    foreach (sudoku_modulo aux3 in matriz[(int)aux.indice.y][(int)aux.indice.x + x + 1].respostaProvavel())
                    {
                        aux2.AddRange(aux3.somente_esquerda.FindAll(c => aux2.Contains(c) == false));
                    }



                    if (matriz[(int)aux.indice.y][(int)aux.indice.x + x].alvo != null ||
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].verificado == false)
                    {

                        break;
                    }
                    else
                    {
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].adicionar(-1, 1, aux2);
                        matriz[(int)aux.indice.y][(int)aux.indice.x + x].respostaProvavel();
                      //  matriz[(int)aux.indice.y][(int)aux.indice.x + x].verificado = true;

                    }
                    if (aux2.Count == modulos.Count)
                    {
                        break;
                    }
                }
            }

            //emcima
            if (aux.indice.y < tamanho - 2)
            {
                for (int x = 1; x < (tamanho) - aux.indice.y && Mathf.Abs(x) < senoide; x++)
                {
                    List<sudoku_modulo> aux2 = new List<sudoku_modulo>();

                    foreach (sudoku_modulo aux3 in matriz[(int)aux.indice.y + x - 1][(int)aux.indice.x].respostaProvavel())
                    {
                        aux2.AddRange(aux3.somente_emcima.FindAll(c => aux2.Contains(c) == false));
                    }


                    if (matriz[(int)aux.indice.y + x][(int)aux.indice.x].alvo != null ||
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].verificado == false)
                    {

                        break;
                    }
                    else
                    {
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].adicionar(1, -1, aux2);
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].respostaProvavel();

                      //  matriz[(int)aux.indice.y + x][(int)aux.indice.x].verificado = true;
                    }
                    if (aux2.Count == modulos.Count)
                    {
                        break;
                    }
                }
            }
            //embaixo
            if (aux.indice.y > 1)
            {
                for (int x = -1; aux.indice.y + x > -1 && Mathf.Abs(x) < senoide; x--)
                {

                    List<sudoku_modulo> aux2 = new List<sudoku_modulo>();

                    foreach (sudoku_modulo aux3 in matriz[(int)aux.indice.y + x + 1][(int)aux.indice.x].respostaProvavel())
                    {
                        aux2.AddRange(aux3.somente_embaixo.FindAll(c => aux2.Contains(c) == false));
                    }



                    if (matriz[(int)aux.indice.y + x][(int)aux.indice.x].alvo != null ||
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].verificado == false)
                    {

                        break;
                    }
                    else
                    {

                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].adicionar(-1, -1, aux2);
                        matriz[(int)aux.indice.y + x][(int)aux.indice.x].respostaProvavel();
                    //    matriz[(int)aux.indice.y + x][(int)aux.indice.x].verificado = true;

                    }
                    if (aux2.Count == modulos.Count)
                    {
                        break;
                    }//
                }
            }
        }

    }

  
    public void MelhorResposta()
    {


            calcularProbabilidades(0);
            List<ConstrutorSudoku> filtro = new List<ConstrutorSudoku>();
            filtro.AddRange(matriz_.FindAll(x => x.verificado == true && x.contador > 0));
            //filtro.AddRange(matriz.Find(x => x.Exists(c=> c.alvo == null) ));
            if (filtro.Count > 0)
            {



                int auxi = filtro[0].contador;
                float menor_distancia = filtro[0].distancia;
                ConstrutorSudoku aux2 = null;

                if (observador == null)
                {
                    foreach (ConstrutorSudoku aux in filtro)
                    {

                        if (aux.respostaProvavel().Count < auxi)
                        {
                            auxi = aux.contador;

                        }

                        if (aux.contador == 1)
                        {
                            break;
                        }

                    }
                    filtro.RemoveAll(x => x.contador != auxi);
                    aux2 = filtro[filtro.Count == 1 ? 0 : Random.Range(0, filtro.Count)];
                    // Debug.Log("possiveis :" + filtro.Count + "  com : " + auxi);
                }
                else
                {
                    filtro.RemoveAll(x => x.respostaProvavel().Count <= 0);
                    foreach (ConstrutorSudoku aux in filtro)
                    {
                        aux.distancia = Vector3.Distance(aux.visual.transform.position, observador.transform.position);
                        if (aux.distancia < menor_distancia || aux == filtro[0])
                        {
                            menor_distancia = aux.distancia;
                        } 
                    }
                    aux2 = filtro.Find(x => limitador == 0 ? x.distancia == menor_distancia : (x.distancia == menor_distancia) && (x.distancia < limitador));

                    Debug.Log("possiveis :" + filtro.Count + "  com : " + menor_distancia + "m" + (aux2 == null ? "erro " : " ok"));
                    
                }
                 if(aux2 != null){
               try
                {

                    int aux3 = modulos.IndexOf(aux2.respostaProvavel()[(aux2.provavel.Count == 1 ? 0 : //Random.Range(0,aux2.provavel.Count) 
                                                                                                        calcularProvaveis(aux2)
                        )]);
                    criarEstrutura(matriz[(int)aux2.indice.y][(int)aux2.indice.x], aux3);
                }
                catch
                {
                    
                } }
            

            }
            else
            {
               
                tirar_c();
               
                
                
            }
        
        
    
    }
    private int calcularPL(Vector2 indc)
    {

        float nm = ((tamanho / 2) * escala);
        Vector2 aux = new Vector2(transform.position.x - nm, transform.position.z - nm);
        Vector2 aux2 = new Vector2(indc.x *escala, indc.y * escala);

        if (Mathf.PerlinNoise(((aux.y + aux2.y) / 1.5f),((aux.x + aux2.x) / 1.5f))* 17 > 1)
        {
            return 10;
        }
        else
        {
            return -1; 
        }
    }
    public int calcularProvaveis(ConstrutorSudoku local_)
    {
        List<sudoku_modulo> lista = new List<sudoku_modulo>();
            lista.AddRange( local_.provavel);

        int indc = -1;
        int somatoria = 0;

        foreach(sudoku_modulo aux in lista)
        {
            if (aux.vantagemPL)
                somatoria += calcularPL(local_.indice);
            somatoria += aux.probabilidade;
        }
        int vl = Random.Range(0, somatoria);
        somatoria = 0;

        for(int x =0; x < lista.Count; x++)
        {
            
            somatoria += lista[x].probabilidade + (lista[x].vantagemPL ? calcularPL(local_.indice):0);
            if(vl < somatoria)
            {
                indc = x;
                break;
            }
        }

        return indc;
    }
    public bool MelhorResposta(sudoku_modulo aux4)
    {

        if (aux4 != null)
        {

                calcularProbabilidades(0);
                List<ConstrutorSudoku> filtro = new List<ConstrutorSudoku>();
                filtro.AddRange(matriz_.FindAll(x => x.verificado == true && x.contador > 0 && x.respostaProvavel().Contains(aux4)));
                //filtro.AddRange(matriz.Find(x => x.Exists(c=> c.alvo == null) ));
                if (filtro.Count > 0)
                {



                    int auxi = filtro[0].contador;  
                    float menor_distancia = filtro[0].distancia;
                    ConstrutorSudoku aux2 = null;

                    if (observador == null)
                    {
                        foreach (ConstrutorSudoku aux in filtro)
                        {

                            if (aux.respostaProvavel().Count < auxi)
                            {
                                auxi = aux.contador;

                            }

                            if (aux.contador == 1)
                            {
                                break;
                            }

                        }
                        filtro.RemoveAll(x => x.contador != auxi);
                        aux2 = filtro[filtro.Count == 1 ? 0 : Random.Range(0, filtro.Count)];
                        Debug.Log("possiveis :" + filtro.Count + "  com : " + auxi + " em : " + aux2.indice);
                    }
                    else
                    {
                        filtro.RemoveAll(x => x.respostaProvavel().Count <= 0);
                        foreach (ConstrutorSudoku aux in filtro)
                        {
                            aux.distancia = Vector3.Distance(aux.visual.transform.position, observador.transform.position);
                            if (aux.distancia < menor_distancia || aux == filtro[0])
                            {
                                menor_distancia = aux.distancia;
                            }
                        }
                        aux2 = filtro.Find(x => limitador == 0 ? x.distancia == menor_distancia : (x.distancia == menor_distancia) && (x.distancia < limitador));

                        Debug.Log("possiveis :" + filtro.Count + "  com : " + menor_distancia + "m" + (aux2 == null ? "erro " : " ok"));
                        
                    }



                if (aux2 != null)
                        {
                       

                    try
                    {
                        int aux3 = modulos.IndexOf(aux4);
                        criarEstrutura(matriz[(int)aux2.indice.y][(int)aux2.indice.x], aux3);
                    }
                    catch
                    {        }
                    
                    return true;
                    }else{
                         return false;

                    }
                }
                else
                {
                    tirar_c();          
                    return false;

                }
            
        }else{
            return false;
        }


    }

    public void tirar_c()
    {
        List<ConstrutorSudoku> filtro = new List<ConstrutorSudoku>();
        
        for(int i=0; i<tamanho; i++)
        {
            for(int k = 0; k < tamanho; k++)
            {
                if (matriz[i][k].alvo == null)
                {
                    filtro.Add(matriz[i][k]);
                }
            }
            
        }
       
        calcularProbabilidades(1);
      

        foreach (ConstrutorSudoku sudoku in filtro)
        {

            List<sudoku_modulo> somatoriaL = new List<sudoku_modulo>();
            List<sudoku_modulo> bkp = new List<sudoku_modulo>();
         
            try
            {
                somatoriaL.AddRange(sudoku.somente_emcima);
          
                somatoriaL.AddRange(sudoku.somente_esquerda);
           
                somatoriaL.AddRange(sudoku.somente_direita);
              somatoriaL.AddRange(sudoku.somente_embaixo);
            }
            catch { }
            bkp.AddRange(somatoriaL);
            try
            {
                somatoriaL.RemoveAll(c=>sudoku.somente_emcima.Contains(c) ==false);

                somatoriaL.RemoveAll(c => sudoku.somente_esquerda.Contains(c)==false);
                    
                somatoriaL.RemoveAll(c => sudoku.somente_direita.Contains(c)==false);
                somatoriaL.RemoveAll(c => sudoku.somente_embaixo.Contains(c)==false);
            }catch { }
          
            if (somatoriaL.Count == 0) {
                Debug.LogError(bkp.Count);
                int aux = modulos.IndexOf(bkp[bkp.Count == 1 ? 0 : Random.Range(0, bkp.Count)]);
                aux = aux < 0 ? Random.Range(0, modulos.Count) : aux;

              //  criarEstrutura(sudoku, sudoku.indice, aux);
            }
            else
            {

                //criarEstrutura(sudoku, sudoku.indice, modulos.IndexOf(somatoriaL[somatoriaL.Count == 1 ?0:Random.Range(0, somatoriaL.Count)]));
            }
        }

    }

    
  

  
    public int intensidade_suavisar;
  
    private Vector3 calcularLocal(Vector3 indic)
    {
        Vector3 auxBase = transform.position + (indic * escala);
        float normal = (tamanho / 2) * escala;

        return new Vector3(auxBase.x - normal, 0, auxBase.z - normal);

    } 
    private Vector3 calcularIndice(Vector3 posicao)
    {
            float normal = ((tamanho / 2) * escala);

        Vector3 zero = new Vector3(transform.position.x - normal, transform.position.z - normal,0);
        Debug.Log(zero.y + "  o" + posicao.z);
        return new Vector3(((posicao.x - zero.x)/escala ) ,((posicao.z - zero.y) / escala ),0);
    }

}



public class ConstrutorSudoku {
    
 
    public List<sudoku_modulo> somente_esquerda, somente_direita, somente_emcima, somente_embaixo;
    public List<sudoku_modulo> provavel;
    public GameObject alvo;
  
    public Vector2 indice;
    public bool verificado,gerado_verificado;
    public int contador;
    public GameObject visual;
    public int indice_modulo;
    public ConstrutorSudoku()
    {
       
        somente_esquerda = new List<sudoku_modulo>();
        somente_direita = new List<sudoku_modulo>();
        somente_emcima = new List<sudoku_modulo>();
        somente_embaixo = new List<sudoku_modulo>();
    }
    public float distancia;
 
    public void limpar()
    {
        
        somente_esquerda = new List<sudoku_modulo>();
        somente_direita = new List<sudoku_modulo>();
        somente_emcima = new List<sudoku_modulo>();
        somente_embaixo = new List<sudoku_modulo>();
        provavel = new List<sudoku_modulo>();
        contador=0;
        gerado_verificado = false;

        verificado = false;
        GameObject.Destroy(alvo);
        alvo = null;
    }


    public List<sudoku_modulo >respostaProvavel()
    {
        List<sudoku_modulo> aux2 = new List<sudoku_modulo>();
       
       
        foreach (sudoku_modulo aux in somente_esquerda)
        {
            if (aux2.Contains(aux) == false)
            {
                aux2.Add(aux);
            }
        }
        somente_esquerda.Clear();
        somente_esquerda.AddRange(aux2);
        
        aux2.Clear();
        
       foreach (sudoku_modulo aux in somente_direita)
       {
           if (aux2.Contains(aux) == false)
           {
               aux2.Add(aux);
           }
       }
        somente_direita.Clear();
        somente_direita.AddRange(aux2);
        aux2.Clear();

       foreach (sudoku_modulo aux in somente_emcima)
       {
           if (aux2.Contains(aux) == false)
           {
               aux2.Add(aux);
           }
       }
        somente_emcima.Clear();
        somente_emcima.AddRange(aux2);
        aux2.Clear();

       foreach (sudoku_modulo aux in somente_embaixo)
       {
           if (aux2.Contains(aux) == false)
           {
               aux2.Add(aux);
           }
       }
        somente_embaixo.Clear();
        somente_embaixo.AddRange(aux2);
        aux2.Clear();
       

        aux2.AddRange(somente_emcima);
       // aux2.AddRange(somente_embaixo);
      //  aux2.AddRange(somente_direita);
       // aux2.AddRange(somente_esquerda);

       

        aux2.AddRange(somente_embaixo.FindAll(x=> aux2.Contains(x)==false ));
        aux2.AddRange(somente_direita.FindAll(x => aux2.Contains(x) == false));
        aux2.AddRange(somente_esquerda.FindAll(x => aux2.Contains(x) == false));

       
        if (somente_emcima.Count > 0)
        {
            aux2.RemoveAll(x => somente_emcima.Contains(x) == false);
        }if (somente_embaixo.Count > 0)
        {
            aux2.RemoveAll(x => somente_embaixo.Contains(x) == false);
        }
        if (somente_direita.Count > 0)
        {
           aux2.RemoveAll(x => somente_direita.Contains(x) == false);
        }if (somente_esquerda.Count > 0)
        {
            aux2.RemoveAll(x => somente_esquerda.Contains(x) == false);
        }


        contador = aux2.Count;
        provavel = aux2;
        return aux2;




    }
    public void removertodos(int x, int b, List<sudoku_modulo> aux)
    {



        if (b < 0)
        {
            if (x == -1)
            {
                somente_emcima.Clear();
                somente_emcima.AddRange(aux);
                
            }
            if (x == 1)
            {
                somente_embaixo.Clear();
                somente_embaixo.AddRange(aux);
               // somente_embaixo.RemoveAll(x => aux.Contains(x) == false);
            }
        }
        else
        {

            if (x == 1)
            {

                somente_esquerda.Clear();
                somente_esquerda.AddRange(aux);
               // somente_esquerda.RemoveAll(x => aux.Contains(x) == false);


            }
            if (x == -1)
            {
                somente_direita.Clear();
                somente_direita.AddRange(aux);
              //  somente_direita.RemoveAll(x => aux.Contains(x) == false);
            }
        }

        
      
       
       ;

    }
    public  void adicionar(int x,int b ,List<sudoku_modulo> aux)
    {
        if (b < 0) {
            if (x == -1)
            {
                somente_emcima.Clear();
                somente_emcima.AddRange(aux.FindAll(c => somente_emcima.Contains(c) == false));
            }
            if (x == 1)
            {
                somente_embaixo.Clear();
                somente_embaixo.AddRange(aux.FindAll(c => somente_embaixo.Contains(c) == false));
            }
        }
        else {

            if (x == 1)
            {

                somente_esquerda.Clear();
                somente_esquerda.AddRange(aux.FindAll(c => somente_esquerda.Contains(c) == false));


            }
            if (x == -1)
            {
                somente_direita.Clear();
                somente_direita.AddRange(aux.FindAll(c => somente_direita.Contains(c) == false));
            } 
        }

    }
  
    public void definirCada(sudoku_modulo aux)
    {

        somente_emcima.Clear();
        somente_emcima.Add(aux);


        somente_esquerda.Clear();
        somente_esquerda.Add(aux);


        somente_direita.Clear();
        somente_direita.Add(aux);

        somente_embaixo.Clear();
        somente_embaixo.Add(aux);

        /*
        List<sudoku_modulo> nova = new List<sudoku_modulo>();
        nova.AddRange(aux.somente_emcima.FindAll(x=>somente_emcima.Contains(x)==false));
        somente_emcima.AddRange(nova);
        nova.Clear();

        nova.AddRange(aux.somente_esquerda.FindAll(x => somente_esquerda.Contains(x) == false));
        somente_esquerda.AddRange(nova);
        nova.Clear();


        nova.AddRange(aux.somente_direita.FindAll(x => somente_direita.Contains(x) == false));
        somente_direita.AddRange(nova);
        nova.Clear();

       
        nova.AddRange(aux.somente_embaixo.FindAll(x => somente_embaixo.Contains(x) == false));
        somente_embaixo.AddRange(nova);
        */

    }


}

