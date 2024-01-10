using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
namespace ParaTodoTempo
{
    public class GerenciadoDiaNoite : MonoBehaviour
    {
        [SerializeField]
        private Transform transformLuz;
        public int DuracaoDoDia;
        public TextMeshProUGUI contador;
        public float segundos, multiplicador;


        private void Start()
        {
            multiplicador = 86400 / DuracaoDoDia;

            // Obtém a rotação X da luz
            float rotacaoXInicial = transformLuz.transform.rotation.eulerAngles.x;

            // Mapeia a rotação X no intervalo de -90 a 270 para segundos entre 0 e 86400
            segundos = Mathf.InverseLerp(-90, 270, rotacaoXInicial) * 86400;
        }
        private void Update()
        {
            segundos += Time.deltaTime * multiplicador;

            if(segundos >= 86400)
            {
                segundos = 0;
            }
            processarCeu();
            calcularHorario();
        }

        private void processarCeu()
        {
            float rotacaoX = Mathf.Lerp(-90, 270, segundos/86400);
            transformLuz.transform.rotation = Quaternion.Euler(rotacaoX, 0, 0);
        }
        private void calcularHorario()
        {
            if (contador != null)
            {
                contador.text = TimeSpan.FromSeconds(segundos).ToString(@"hh\:mm");
            }
        }
    }
}
