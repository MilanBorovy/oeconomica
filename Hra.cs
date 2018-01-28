using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hra : MonoBehaviour {

    public List<Hrac> hraci { get; private set; }
    public int kolo { get; private set; }
    public int hracNaRade { get; private set; }
    public int dostupneAkce { get; private set; }
    private float timeLimit, time;

    private void Start () {
        hraci = new List<Hrac>();
        hraci.Add(new Hrac("Hráč 1", "Firma s.r.o.", 0));
        hraci.Add(new Hrac("Hráč 2", "Firma a.s.", 1));
        hraci.Add(new Hrac("Hráč 3", "Firma v.o.s.", 2));
        hraci.Add(new Hrac("Hráč 4", "Firma k.s.", 3));
        for (int i = 0; i < 4; i++)
        {
            GameObject.Find("Cedule").transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().color = hraci[i].color;
            GameObject.Find("Cedule").transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().color = hraci[i].color;

            GameObject.Find("Cedule").transform.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = hraci[i].firma;
            GameObject.Find("Cedule").transform.GetChild(i).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = hraci[i].firma;

            GameObject.Find("Cedule").transform.GetChild(i).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = hraci[i].name;
            GameObject.Find("Cedule").transform.GetChild(i).GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text = hraci[i].name;
        }
        timeLimit = 120f;
        kolo = 0;
        hracNaRade = 0;
        dostupneAkce = 1;
        time = 0f;
        VypsaniInfoHracNaRade();
    }

    private void Update()
    {
        time += Time.deltaTime;
        GameObject.Find("butDalsiTah").transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = (timeLimit - time) / timeLimit;
        if (time >= timeLimit)
            DalsiTah();
    }

    public void VypsaniInfoHracNaRade()
    {
        for (int i = 1; i < 9; i++)
        {
            if (hraci[hracNaRade].charita >= i)
                GameObject.Find("infoPlayer").transform.FindChild("playerCharitaG").FindChild(string.Format("char{0}Toggle", i)).GetComponent<Toggle>().isOn = true;
            else
                GameObject.Find("infoPlayer").transform.FindChild("playerCharitaG").FindChild(string.Format("char{0}Toggle", i)).GetComponent<Toggle>().isOn = false;
        }
        if (hraci[hracNaRade].finance >= 6 && hraci[hracNaRade].charita < 8)
        {
            GameObject.Find("infoPlayer").transform.FindChild("playerCharitaG").FindChild("addCharity").GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("infoPlayer").transform.FindChild("playerCharitaG").FindChild("addCharity").GetComponent<Button>().interactable = false;
        }
        GameObject.Find("infoPlayer").transform.FindChild("playerName").FindChild("playerNamet").GetComponent<Text>().text = hraci[hracNaRade].name;
        GameObject.Find("infoPlayer").transform.FindChild("playerOrgName").FindChild("playerOrgNamet").GetComponent<Text>().text = hraci[hracNaRade].firma;
        GameObject.Find("infoPlayer").transform.FindChild("playerKapital").FindChild("playerKapitalt").GetComponent<Text>().text = string.Format("{0},000,000 Kč", hraci[hracNaRade].finance);
        GameObject.Find("infoPlayer").transform.FindChild("playerCharita").FindChild("playerCharitat").GetComponent<Text>().text = Charita.ch[hraci[hracNaRade].charita];
    }

    public void addCharityClick()
    {
        hraci[hracNaRade].Finance(6, false);
        hraci[hracNaRade].Charita();
        VypsaniInfoHracNaRade();
        GameObject.Find("addCharity").GetComponent<Button>().interactable = false;
    }

    public void DalsiTah()
    {
        dostupneAkce = 1;
        if (hracNaRade + 1 >= hraci.Count)
            DalsiKolo();
        else
            hracNaRade++;
        VypsaniInfoHracNaRade();
        time = 0;
    }

    public void DalsiKolo()
    {
        hracNaRade = 0;
        kolo++;
        rozdeleniFinanci();
        zmenyCen();
    }

    private void rozdeleniFinanci()
    {
        foreach (Hrac h in hraci)
        {
            int zisky = 0;
            for (int i = 0; i < 4; i++)
            {
                int vynosy = 0;
                int naklady = 0;
                int[,] prodCons = (GameObject.Find("Budovy").transform.GetChild(h.id).GetChild(i).GetComponent("Budova") as Budova).typ.prodCons;
                vynosy += prodCons[0, 0] * Ceny.elektrina;
                vynosy += prodCons[1, 0] * Ceny.prsila;
                vynosy += prodCons[2, 0] * Ceny.auta;
                vynosy += prodCons[3, 0];
                naklady += prodCons[0, 1] * Ceny.elektrina;
                naklady += prodCons[1, 1] * Ceny.prsila;
                naklady += prodCons[2, 1] * Ceny.auta;
                naklady += prodCons[3, 1];
                zisky += vynosy - naklady;
            }
            h.Finance(zisky, true);
        }
    }

    private void zmenyCen()
    {
        int EP = Mathf.CeilToInt(GameObject.Find("eProdFront").GetComponent<Image>().fillAmount * 10);
        int EC = Mathf.CeilToInt(GameObject.Find("eConsFront").GetComponent<Image>().fillAmount * 10);
        int LP = Mathf.CeilToInt(GameObject.Find("lProdFront").GetComponent<Image>().fillAmount * 10);
        int LC = Mathf.CeilToInt(GameObject.Find("lConsFront").GetComponent<Image>().fillAmount * 10);
        int TP = Mathf.CeilToInt(GameObject.Find("tProdFront").GetComponent<Image>().fillAmount * 10);
        int TC = Mathf.CeilToInt(GameObject.Find("tConsFront").GetComponent<Image>().fillAmount * 10);

        if (EP > EC)
        {
            Ceny.snizitCenu('e', 1);
        }
        else if (EP < EC)
        {
            Ceny.zvysitCenu('e', 1);
        }
        if (LP > LC)
        {
            Ceny.snizitCenu('l', 1);
        }
        else if (LP < LC)
        {
            Ceny.zvysitCenu('l', 1);
        }
        if (TP > TC)
        {
            Ceny.snizitCenu('t', 1);
        }
        else if (TP < TC)
        {
            Ceny.zvysitCenu('t', 1);
        }
    }

    public void Akce()
    {
        dostupneAkce--;
    }

}
