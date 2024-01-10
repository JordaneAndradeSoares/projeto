using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ageral
{
    public class ValoresUniversais : MonoBehaviour
    {
        public static float gravidade;

        public static float VelocidadeDeCaminhadaPortePequeno;


        public float gravidade_;
        public float VelocidadeDeCaminhadaPortePequeno_;

        private void Update()
        {
            gravidade = gravidade_;
            VelocidadeDeCaminhadaPortePequeno = VelocidadeDeCaminhadaPortePequeno_;
        }
       public static float calcularPerlingNoise(float x, float y, float amplitude, float frequencia)
        {
            x *= amplitude;
            y*= amplitude;


            return Mathf.PerlinNoise(x/frequencia,y/frequencia);
        }
        public static List<Vector3> OptimizePath(List<Vector3> originalPath , float tolerancia)
        {
            if (originalPath.Count < 3)
            {
                return originalPath;
            }

            List<Vector3> optimizedPath = new List<Vector3>();
            optimizedPath.Add(originalPath[0]);

            for (int i = 1; i < originalPath.Count - 1; i++)
            {
                Vector3 previousPoint = optimizedPath[optimizedPath.Count - 1];
                Vector3 currentPoint = originalPath[i];
                Vector3 nextPoint = originalPath[i + 1];

                if (!ArePointsCollinear(previousPoint, currentPoint, nextPoint,tolerancia))
                {
                    optimizedPath.Add(currentPoint);
                }
            }

            optimizedPath.Add(originalPath[originalPath.Count - 1]);

            return optimizedPath;
        }
        public static bool ArePointsCollinear(Vector3 a, Vector3 b, Vector3 c , float tolerancia)
        {
            Vector2 ab = new Vector2(b.x - a.x, b.z - a.z);
            Vector2 ac = new Vector2(c.x - a.x, c.z - a.z);

            float crossProduct = ab.x * ac.y - ab.y * ac.x;

            // Se o produto cruzado for quase zero, os pontos est�o em linha reta.
            return MathF.Abs( crossProduct ) < tolerancia;
        }
        public static bool LinhasCruzadas(Vector3 a , Vector3 b , Vector3 c , Vector3 d)
        {
            
            float ABC = Orientacao3(a,b,c);
            float ABD = Orientacao3(a, b, d);
            float CDA = Orientacao3(c, d, a);
            float CDB = Orientacao3(c, d, b);

            bool A_ = ABC != ABD;
            bool B_ = CDA != CDB;


            return A_ && B_;
        }
        public static float Orientacao(Vector2 p, Vector2 q, Vector2 r)
        {
            float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
            if (val == 0) return 0;  // esta reto
            return (val > 0) ? 1 : 2; // direita / esquerda
        }
        public static float Orientacao3(Vector3 p, Vector3 q, Vector3 r)
        {
            float val = (q.z - p.z) * (r.x - q.x) - (q.x - p.x) * (r.z - q.z);
            if (val == 0) return 0;  // Colinear
            return (val > 0) ? 1 : 2; // Horário ou anti-horário
        }
      public static bool VerificarCurvaAguda(List<Vector3> pontos, float anguloLimite)
        {
            if (pontos.Count < 3)
            {
          
                return false;
            }
            Vector3 maisLonge = Vector3.zero;
            float distM = Vector3.Distance(pontos[0], pontos[1]);
            for (int i = 1; i < pontos.Count - 1; i++)
            {
                float tempDist = Vector3.Distance(pontos[i], pontos[i + 1]);
                if (tempDist > distM)
                {
                    maisLonge = pontos[i];
                    distM = tempDist;
                }
            }

           
                Vector3 vetorAnterior = pontos[0] - maisLonge;
                Vector3 vetorAtual = pontos[pontos.Count-1] - maisLonge;

               
                vetorAnterior.Normalize();
                vetorAtual.Normalize();

                float angulo = Mathf.Acos(Vector3.Dot(vetorAnterior, vetorAtual)) * Mathf.Rad2Deg;

                if (angulo < anguloLimite)
                {
                  
                    return true;
                }
            

          
            return false;
        }
    }
}
