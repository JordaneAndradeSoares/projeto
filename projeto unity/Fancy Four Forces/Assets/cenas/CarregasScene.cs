using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarregasScene : MonoBehaviour
{
    // Start is called before the first frame update
    public string nomeScene;

    public bool carregar;
    public void carregarScene()
    {
        StartCoroutine(LoadYourAsyncScene());
        //  carregar = true;
        //  AsyncOperation operation = SceneManager.LoadSceneAsync(nomeScene, LoadSceneMode.Single);
        //  AsyncOperation operation = SceneManager.LoadSceneAsync("MyScene", loadLightingSettings: true);
        // SceneManager.LoadScene(nomeScene);
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nomeScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    bool fundocarregado = false;
    public RawImage RI;
    public float fadeScale;

    
    public void carregarFundo()
    {
        fundocarregado = RI.color.a >= 0.85f;

        Color r = RI.color;
        r.a += Time.deltaTime * fadeScale;
        RI.color = r;

        if (fundocarregado)
        {
            Color r_ = RI.color;
            r_.a = 0;
            RI.color = r_;
        }
    }

    private void OnEnable()
    {
        float screenSize = Screen.width;
        float screenHeight = Screen.height;
        tamanhoTela =  new Vector2(screenSize, screenHeight);
    }
    public void mostrarBotoes()
    {
        botoes.SetActive(true);
        iniciar.SetActive(true);
        //Destroy(this);
        // gameObject.SetActive(false);
    }
    public GameObject botoes, iniciar;
 public RawImage fundoPreto;
    public Vector2 tamanhoTela;
    public JogoDaVida jv;
    private void Update()
    {
        if (fundocarregado)
        {
            mostrarBotoes();
        }
        else
        {
            carregarFundo();
        }
        if (carregar)
        {
       //     SceneManager.LoadScene(nomeScene);
          StartCoroutine(LoadYourAsyncScene());
        }

        float screenSize = Screen.width;
        float screenHeight = Screen.height ;
        // Define o tamanho do `RawImage`
        fundoPreto.rectTransform.sizeDelta = new Vector2(screenSize, screenHeight);

        if (jv.gameObject.active)
        {
            if (screenSize != tamanhoTela.x || screenHeight != tamanhoTela.y)
            {
                try
                {
                    tamanhoTela.x = screenSize;
                    tamanhoTela.y = screenHeight;
                    jv.reset_(tamanhoTela);
                }
                catch { }
            }
        }
    }
}
