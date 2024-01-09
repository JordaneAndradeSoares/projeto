using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

namespace Ageral
{
  

public class triangulador : MonoBehaviour 
    {
        // Classe para representar um ponto
     
        public class Ponto
        {
            public Vector2 posicao;
            public List<Ponto> conexoes = new List<Ponto>();
        }

        // Classe para representar um triângulo
        [System.Serializable]
        public class Triangulo
        {
            public void calcular()
            {
                foreach(Ponto a in pontos)
                {
                    posicao.Add(a.posicao);
                }
            }
            public List<Vector2> posicao = new List<Vector2>();
            public List<Ponto> pontos = new List<Ponto>();
        }
        public List<Vector3> pontos_inicial = new List<Vector3>();
        public List<Vector3> triangulos_ = new List<Vector3>();

        public bool guizmos;
       public  List<Vector3> triangular(List<Vector3> vertices)
        {

            List<Vector2> mapeamento = new List<Vector2>();
            foreach (Vector3 bb in vertices)
            {
                mapeamento.Add(new Vector2(MathF.Round(bb.x - transform.position.x), MathF.Round(bb.z - transform.position.z)));
            }
         
            return Triangulacao(mapeamento);
        }
     
        // Função para verificar se duas linhas se cruzam
        private  bool LinhasCruzam(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
        {
         
            float o1 = Orientacao(p1, p2, q1);
            float o2 = Orientacao(p1, p2, q2);
            float o3 = Orientacao(q1, q2, p1);
            float o4 = Orientacao(q1, q2, p2);

            if (o1 != o2 && o3 != o4)
                return true;

            return false;
        }

        // Função para calcular a orientação de três pontos
        private  float Orientacao(Vector2 p, Vector2 q, Vector2 r)
        {
            float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
            if (val == 0) return 0;  // Colinear
            return (val > 0) ? 1 : 2; // Horário ou anti-horário
        }

        // Função para realizar a triangulação
        private  List<Vector3> Triangulacao(List<Vector2> vertices)
        {
            // Mapeamento dos pontos
            List<Ponto> pontos = new List<Ponto>();
            foreach (Vector2 v in vertices)
            {
                Ponto ponto = new Ponto { posicao = v };
                pontos.Add(ponto);
            }

            // Definir que todos os pontos estão conectados entre si
            foreach (Ponto p in pontos)
            {
                p.conexoes.AddRange(pontos);
                p.conexoes.Remove(p);
            }


            // Remover pontos que criam linhas que cruzam com outros pontos
           
           foreach (Ponto p1 in pontos)
           {
               foreach (Ponto p2 in p1.conexoes.ToList())
               {

                   foreach (Ponto q1 in pontos)
                   {
                       if(q1 == p1 || q1 == p2) 
                           continue;
                       foreach (Ponto q2 in q1.conexoes.ToList())
                       {
                           if (p1 == q1 || p1 == q2 || p2 == q1 || p2 == q2)
                               continue;
                           if (LinhasCruzam(p1.posicao, p2.posicao, q1.posicao, q2.posicao))
                           {
                               p1.conexoes.Remove(p2);
                               q1.conexoes.Remove(q2);

                           }                   
                       }
                   }
               }
           }

            foreach (Ponto a in pontos)
            {
                foreach (Ponto b in a.conexoes)
                {
                    if (!b.conexoes.Contains(a))
                    {
                        b.conexoes.Add(a);
                    }
                }
            }


            
            foreach (Ponto a in pontos)
            {
                List<Ponto> auxA = new List<Ponto>();
                foreach (Ponto b in a.conexoes)
                {
                    // de A para B
                   
                    foreach (Ponto C in pontos)
                    {
                        if (C == a || C == b)
                            continue;
                      
                            if (!LinhasCruzam(a.posicao,b.posicao, a.posicao, C.posicao))
                            {
                            auxA.Add(C);
                                C.conexoes.Add(a);

                            }

                       
                    }
                   

                }
                a.conexoes.AddRange(auxA);
            }

          
            // Criar triângulos
            List<Triangulo> triangulos = new List<Triangulo>();
            foreach (Ponto p1 in pontos)
            {
           
                foreach (Ponto p2 in p1.conexoes)
                {
                    foreach (Ponto p3 in p1.conexoes)
                    {
                        if (p2 != p3 && p2.conexoes.Contains(p3))
                        {
                            Triangulo triangulo = new Triangulo();
                            triangulo.pontos.Add(p1);
                            triangulo.pontos.Add(p2);
                            triangulo.pontos.Add(p3);
                            triangulos.Add(triangulo);
                        }
                    }
                }
            }
         //remove repetidos
            List<Triangulo> ff = new List<Triangulo>();
            foreach (Triangulo t in triangulos)
            {
              
              
                    if (!temigual(t, ff))
                    {
                    t.calcular();
                        ff.Add(t);   
                    }
              
            }
            List<Triangulo> tr = new List<Triangulo>();
            //remover internos
            foreach(Triangulo t in ff)
            {
                bool ex = true;
               
                foreach(Ponto p in t.pontos)
                {
                    if (!ex)
                        break;
                    foreach(Triangulo tt in ff) {
                        if (tt == t || tt.pontos.Contains(p))
                            continue;
                        if (PontoDentroTriangulo(p.posicao, tt.pontos[0].posicao, tt.pontos[1].posicao, tt.pontos[2].posicao))
                        {
                            tr.Add(t);
                            ex = false;
                            break;
                        }
                    }
                }
            }
            ff.RemoveAll(x => tr.Contains(x));

            //converte os triangulos para lista de vec3
            List<Vector3> lf = new List<Vector3>();
            foreach (Triangulo qq in ff)
            {

                lf.Add(new Vector3(qq.pontos[0].posicao.x,0, qq.pontos[0].posicao.y));
                lf.Add(new Vector3 (qq.pontos[1].posicao.x,0, qq.pontos[1].posicao.y));
                lf.Add(new Vector3(qq.pontos[2].posicao.x,0, qq.pontos[2].posicao.y));
            }
                    return lf;
        }
       bool temigual(Triangulo a , List<Triangulo> b)
        {

            Ponto A0 = a.pontos[0];
            Ponto A1 = a.pontos[1];
            Ponto A2 = a.pontos[2];

          foreach(Triangulo t in b)
            {
                if (t.pontos.Contains(A0) &&
                    t.pontos.Contains(A1) &&
                    t.pontos.Contains(A2))
                {
                    return true;
                }
            }

            return false;
        }
      public  bool PontoDentroTriangulo(Vector2 A, Vector2 B1, Vector2 B2, Vector2 B3)
        {
            float u, v, w;
            CalculaCoordenadasBaricentricas(A, B1, B2, B3, out u, out v, out w);
             return u >= 0 && v >= 0 && w >= 0 && (u + v + w) <= (1.1f );
        }
        public  bool PontoDentroTriangulo(Vector3 A_, Vector3 B1_, Vector3 B2_, Vector3 B3_)
        {
            //   Vector2 A = new Vector2(A_.x - B1_.x, A_.z - B1_.z);

            //  Vector2 B1 = new Vector2(B1_.x - transform.position.x, B1_.z - transform.position.z);
            //  Vector2 B2 = new Vector2(B2_.x - transform.position.x, B2_.z - transform.position.z);
            //  Vector2 B3 = new Vector2(B3_.x - transform.position.x, B3_.z - transform.position.z);

              Vector2 A = new Vector2(A_.x, A_.z).normalized;

              Vector2 B1 = new Vector2(B1_.x , B1_.z ).normalized;
              Vector2 B2 = new Vector2(B2_.x , B2_.z ).normalized;
              Vector2 B3 = new Vector2(B3_.x , B3_.z ).normalized;





            float u, v, w;
            CalculaCoordenadasBaricentricas(A, B1, B2, B3, out u, out v, out w);
            Debug.Log(u + " " + v + " " + w);
            return u >= 0 && v >= 0 && w >= 0 && (u + v + w) <= (2f);
        }

        // Calcula as coordenadas baricêntricas do ponto P em relação ao triângulo A, B, C
        static void CalculaCoordenadasBaricentricas(Vector2 P, Vector2 A, Vector2 B, Vector2 C, out float u, out float v, out float w)
        {
         
            Vector2 v0 = B - A,
                v1 = C - A,
                v2 = P - A;
         
            float d00 = Vector2.Dot(v0, v0); // 1
            float d01 = Vector2.Dot(v0, v1); // 1   
            float d11 = Vector2.Dot(v1, v1); // 5
            float d20 = Vector2.Dot(v2, v0); //0.5
            float d21 = Vector2.Dot(v2, v1); //1.5
            float denom = d00 * d11 - d01 * d01; //  4  =  1 * 5 - 1 *1
          //   Debug.Log("d00 " + d00 + ";  d01 " + d01 + "; d11 " + d11 + "; d20 " + d20 + "; d21 " + d21 + " ; denom  " + denom);
            v = (d11 * d20 - d01 * d21) / denom; //v   0.25  1/4   = 5 * 0.5 - 1 * 1.5
            w = (d00 * d21 - d01 * d20) / denom; //w  0.25    = 1.5 - 0.5
            u = 1.0f - v - w;                    //u   0.5     =  1 - 0,5
          //  Debug.Log("-----------------------");
        }




    }

}
