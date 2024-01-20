using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class JogoDaVida : MonoBehaviour
{

    public class celula {
        public bool vida;
        public GameObject obj;
        public Vector3 indc;
    }

 
    public List<List<celula>> matriz;

    public float scala;
    public int tamanhoX,tamanhoY;
    public GameObject objCelula;
    public void iniciar()
    {
        matriz = new List<List<celula>>();

        for (int x = 0; x < tamanhoX; x++)
        {
            matriz.Add(new List<celula>());
            matriz[x] = new List<celula>();
            for (int y = 0; y < tamanhoY; y++)
            {
                matriz[x].Add(new celula());
                matriz[x][y].indc = new Vector2(x, y);
                instanciarCelula(matriz[x][y]);

            }
        }
    }
    private void Start()
    {
        iniciar();
        verificarVivoMorto();
       adicionarAleatorio();
    }
    public float minimoDeVivosParaAdicionarAleatorios;
   
    public void adicionarAleatorio()
    {


        for (int x = 0; x < tamanhoX; x++)
        {
            for (int y = 0; y < tamanhoY; y++)
            {
                if (Random.Range(0f, 1f) > 0.75f)
                {
                    if (!matriz[x][y].vida)
                    {
                        // instanciarCelula(matriz[x][y]);
                        matriz[x][y].obj.SetActive(true);
                        matriz[x][y].vida = true;
                    }
                }
            }


        }

       
       
    }
    public float tempoDeVerificacao;
    float t_vr;
    public void instanciarCelula( celula b)
    {
     
        b.obj = Instantiate(objCelula, transform);
        b.obj.transform.position = transform.position + b.indc * scala;
      //  b.obj.transform.localScale = Vector3.one * 20 / scala;
        b.obj.GetComponent<RectTransform>().sizeDelta = Vector2.one * scala;
          


        
    }
   
    public void verificarVivoMorto()
    {
        if (t_vr > tempoDeVerificacao)
        {
            t_vr = 0;
        
            foreach (var a in matriz)
            {
                foreach (var b in a)
                {
                    int temp = testarRegras(b.indc);

                    if (b.vida)
                    {
                        if (temp < 2)
                        {
                            b.vida = false;
                        }
                        else if (temp == 3 || temp == 2)
                        {
                            b.vida = true;
                        }
                        else
                        {
                            b.vida = false;
                        }
                        if (b.vida)
                        {
                            b.obj.SetActive(true);
                            if (!b.obj)
                            {
                                //instanciarCelula(b);
                               
                            }
                        }
                        else
                        {
                            b.obj.SetActive(false);
                            if (b.obj)
                            {
                              //  Destroy(b.obj);

                            }
                        }
                    }
                    else
                    {
                        if(temp == 3)
                        {
                            b.vida = true;

                            b.obj.SetActive(true);
                            if (!b.obj)
                            {
                                // instanciarCelula(b);
                                b.obj.SetActive(true);
                            }
                        }
                    }
                       

                }
            }
        }
        else
        {
            t_vr += Time.deltaTime;
        }
    }
    public int  testarRegras(Vector2 indc)
    {
        int contador = 0;
        for(int x = -1; x < 2; x += 2)
        {
           

            for(int y = -1; y < 2; y += 2)
            {
                try
                {
                    if (matriz[(int)indc.x + x][(int)indc.y + y].vida)
                    {
                        contador++;
                    }
                }
                catch { }
            }
            try
            {
                if (matriz[(int)indc.x + x][(int)indc.y].vida)
                {
                    contador++;
                }
            }
            catch { }

            try
            {
                if (matriz[(int)indc.x][(int)indc.y + x].vida)
                {
                    contador++;
                }
            }
            catch { }
        }

        return contador;
    }

    public bool testarEscala, reset = true;
    public void reset_(Vector2 c)
    {
        tamanhoX = (int)(c.x / scala);
        tamanhoY= (int)(c.y / scala);

        reset = false;

        foreach (var a in matriz)
        {
            foreach (var b in a)
            {
                b.obj.SetActive(false);
                if (b.obj)
                {
                  //  Destroy(b.obj);
                  
                }
            }
        }
        iniciar();
        adicionarAleatorio();
    }
    public bool adicionarAleatorios;

    public void detectarMouse()
    {

    }

    public void inicarJOgo()
    {

    }
    private void Update()
    {
        if (!testarEscala)
        {
            reset = false;
            if (adicionarAleatorios)
            {
                adicionarAleatorios = false;
                adicionarAleatorio();
            }

            verificarVivoMorto();
            detectarMouse();

        }
       


    }

}
