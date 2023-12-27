using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static modoBatalha.Configuracao;
using static UnityEngine.GraphicsBuffer;

namespace modoBatalha
{
    public class Configuracao : MonoBehaviour
    {
        public Transform aliados, inimigos;

        public float espasamentoentreSi;

       

        public List<buffer_s> aliadosL = new List<buffer_s>();
        public List<buffer_s> inimigosL = new List<buffer_s>();

        [System.Serializable]
        public class buffer_s {
            public GameObject obj;
            public Vector3 local;
            public buffer_s()
            {
                local = Vector3.zero;
                obj = null;
            }
        }

    }
    [CustomEditor(typeof(Configuracao))]
    public class EditorMovimento : Editor
    {
        private void OnEnable()
        {
            Configuracao meuScript = (Configuracao)target;

            if(meuScript.aliadosL.Count == 0)
            {
                for(int x = 0; x < 4; x++)
                {

                    meuScript.aliadosL.Add(new buffer_s());
                }
            }

            if (meuScript.inimigosL.Count == 0)
            {
                for (int x = 0; x < 4; x++)
                {

                    meuScript.inimigosL.Add(new buffer_s());
                }
            }
        }
        private void OnSceneGUI()
        {
            Configuracao meuScript = (Configuracao)target;
            if (meuScript.aliados != null)
            {

                for (int x = 0; x < meuScript.aliadosL.Count; x++)
                {
                    meuScript.aliadosL[x].local = meuScript.aliados.position + (Vector3.right * x * meuScript.espasamentoentreSi);


                    Handles.color = Color.green;
                    Handles.DrawLine(meuScript.aliadosL[x].local, meuScript.aliadosL[x].local + Vector3.up);
                }

               
            }
            if (meuScript.inimigos != null)
            {
                for (int x = 0; x < meuScript.inimigosL.Count; x++)
                {
                    meuScript.inimigosL[x].local = meuScript.inimigos.position + (Vector3.right * x * meuScript.espasamentoentreSi);

                    Handles.color = Color.red;
                    Handles.DrawLine(meuScript.inimigosL[x].local, meuScript.inimigosL[x].local + Vector3.up);
                }
            }

            foreach(var a in meuScript.aliadosL)
            {
                if (!a.obj)
                    continue;
                a.obj.transform.position = a.local;
            }
            foreach(var a in meuScript.inimigosL)
            {
                if (!a.obj)
                    continue;
                a.obj.transform.position = a.local;
            }

        }
    }
}
