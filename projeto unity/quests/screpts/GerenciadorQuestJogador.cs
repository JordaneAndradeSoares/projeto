using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorQuestJogador : MonoBehaviour
{
    public ClickarEmCoisas clk;

    public List<DerrotarMonstro> monstr;
    public List<TodosOsMonstros> monstros = new List<TodosOsMonstros>();
   
    public List<gQuest> ListaQuests = new List<gQuest>();
    public List<gQuest> QuestsCompletas= new List<gQuest>();
    public Inventa inventario_;
    [System.Serializable]
    public struct gQuest
    {
        public Quest quest_;
        public int quantidadeInicial;

    }
    public struct TodosOsMonstros {
        public DerrotarMonstro monstro;
        public int quantidade;
    }
    private void Start()
    {
        foreach (DerrotarMonstro a in monstr)
        {
            TodosOsMonstros aa = new TodosOsMonstros();
            aa.monstro = a;
            monstros.Add(aa);
        }
    }

    public void clikou(Quest a)
    {
        if (ListaQuests.Exists(x=>x.quest_ ==a) || QuestsCompletas.Exists(x => x.quest_ == a))
        {
            ListaQuests.Remove(ListaQuests.Find(x => x.quest_ == a));
        }
        else
        {
            gQuest aux = new gQuest();
            aux.quest_ = a;
            ListaQuests.Add(aux);
        }
    }
    public bool completouQuestItens(GeralIten a,int quantidade)
    {
        return inventario_.temecItem(a, quantidade);
      
    }
    public bool completouQuestMonstros(DerrotarMonstro a,int b,gQuest c)
    {
        if (c.quantidadeInicial == 0)
        {
           c.quantidadeInicial = monstros.Find(x => x.monstro == a).quantidade;
        }

        if(monstros.Find(x=>x.monstro == a).quantidade >= c.quantidadeInicial + b)
        {
            return true;
        } 
        return false;
    }
    public bool completouQuest(gQuest a)
    {
        bool Qitem = false;
        foreach (var aux in a.quest_.problema.itens)
        {
            Qitem =completouQuestItens(aux.item,aux.quantidade);
            if (Qitem == false)
                break;
        }
        Qitem = a.quest_.problema.itens.Count == 0 ? true : Qitem;
        bool Qmonstro = false;
        foreach (var aux in a.quest_.problema.monstros)
        {
            Qmonstro = completouQuestMonstros(aux.mosntro, aux.quantidade,a);
            if (Qmonstro == false)
                break;
        }
        Qmonstro = a.quest_.problema.monstros.Count == 0 ? true : Qmonstro;
        return Qitem ? Qmonstro ? true : false : false;
    }
    float aux;
    private void Update()
    {
        if(aux > 1)
        {
            aux = 0;
            List<gQuest> aux__ = new List<gQuest>();
            foreach(gQuest a in ListaQuests)
            {
                if (completouQuest(a))
                {
                    Debug.Log("completou A quest");
                    aux__.Add(a);
                }
            }
            QuestsCompletas.AddRange(aux__);
            ListaQuests.RemoveAll(x => aux__.Contains(x));
        }
        aux += Time.deltaTime;
    }
}
