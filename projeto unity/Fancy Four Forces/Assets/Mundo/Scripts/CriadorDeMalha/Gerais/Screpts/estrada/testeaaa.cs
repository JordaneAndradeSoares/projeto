using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ageral
{
    public class testeaaa : MonoBehaviour
    {
        public Collider tempCollider;
     
        public bool testar()
        {
            Vector3 pontoMaisProximo = tempCollider.ClosestPointOnBounds(transform.position);
            pontoMaisProximo.y = 0;
            Vector3 auxJ = transform.position;
            auxJ.y = 0;
            Debug.Log(Vector3.Distance(auxJ, pontoMaisProximo) + "   " + pontoMaisProximo);
            return Vector3.Distance(auxJ, pontoMaisProximo) < 1;
        }

        // Update is called once per frame
    
    }
    [CustomEditor(typeof(testeaaa))]
    public class aadsasdasdasd : Editor
    {
      
        private void OnSceneGUI()
        {
            testeaaa meuScript = (testeaaa)target;
             if (meuScript.testar() )
            {

                Handles.color = Color.green;
            }
            else
            {

                Handles.color = Color.red;
            }
            Handles.DrawSolidDisc(meuScript.transform.position, Vector3.up, 1);
        }

        }
    }
