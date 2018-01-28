using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OvladaniBudovyS : MonoBehaviour
{
    Budova bud;

    public Texture2D texE;
    public Texture2D texL;
    public Texture2D texT;
    public Texture2D texC;
    public Texture2D texA;
    public Texture2D texCh;
    private List<GameObject> temp = new List<GameObject>();
    private int cooldown = 0;
    private bool wait = false;

    private void Start()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    public void Zobrazit(GameObject budova)
    {
        (GameObject.Find("UpgradePanel").GetComponent("UpgradeDowngrade") as UpgradeDowngrade).Skryt();
        bud = budova.GetComponent("Budova") as Budova;
        Budova info = (budova.GetComponent("Budova") as Budova);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("OvladaniBudov").transform.position = new Vector3(budova.transform.position.x, 50,budova.transform.position.z - 50);
        GameObject.Find("tBuilding").GetComponent<Text>().text = string.Format("Budova: {0}", info.typ.nazev);
        GameObject.Find("tProj").GetComponent<Text>().text = string.Format("V tomto kole: {0},000,000 Kč", VypocetZisku(info));
        string tl1 = "";
        string tl2 = "";
        int vid = 0;
        Hra h = GameObject.Find("Hra").GetComponent("Hra") as Hra;
        if (budova.transform.IsChildOf(GameObject.Find("Budovy").transform.GetChild(h.hraci[h.hracNaRade].id)))
        {
            GameObject.Find("tlDestroy").GetComponent<Button>().transform.localScale = new Vector3(1, 1, 1);
            GameObject.Find("tlBuild").GetComponent<Button>().transform.localScale = new Vector3(1, 1, 1);
            GameObject.Find("tlBuild").GetComponent<Button>().interactable = true;
            switch (info.typ.uroven)
            {
                case 0:
                    tl1 = "POSTAVIT";
                    tl2 = "NIC";
                    vid = 0;
                    break;
                case 1:
                    tl1 = "ZMĚNIT ↑/↔";
                    if (h.hraci[h.hracNaRade].finance >= 1 && h.dostupneAkce > 0)
                    {
                        tl2 = "ZBOURAT";
                        GameObject.Find("tlDestroy").GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        tl2 = "ZBOURAT (nelze)";
                        GameObject.Find("tlDestroy").GetComponent<Button>().interactable = false;
                    }
                    vid = 1;
                    break;
                case 2:
                    tl1 = "ZMĚNIT ↑/↔";
                    tl2 = "ZMĚNIT ↓";
                    vid = 1;
                    break;
                case 3:
                    tl1 = "ZMĚNIT ↔";
                    tl2 = "ZMĚNIT ↓";
                    vid = 1;
                    break;
            }
        }
        else
        {
            GameObject.Find("tlDestroy").GetComponent<Button>().transform.localScale = new Vector3(0, 0, 0);
            GameObject.Find("tlBuild").GetComponent<Button>().transform.localScale = new Vector3(0, 0, 0);
            GameObject.Find("tlBuild").GetComponent<Button>().interactable = false;
            GameObject.Find("tlDestroy").GetComponent<Button>().interactable = false;
        }
        GameObject.Find("tBuild").GetComponent<Text>().text = tl1;
        GameObject.Find("tDestroy").GetComponent<Text>().text = tl2;
        GameObject.Find("tlDestroy").transform.localScale = new Vector3(vid, vid, vid);
        SmazaniProdCons();
        ZakresleniProdCons(info);
        cooldown = 10;
        wait = true;
    }

    private void ZakresleniProdCons(Budova info)
    {
        Texture2D[] textury = new Texture2D[] { texE, texL, texT, texC, texA, texCh };

        //produkce
        List<Transform> produkce = new List<Transform>();
        produkce.Add(GameObject.Find("prod0").transform.FindChild("image"));
        produkce.Add(GameObject.Find("prod1").transform.FindChild("image"));
        produkce.Add(GameObject.Find("prod2").transform.FindChild("image"));
        int x = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i != 3)
            {
                for (int j = 0; j < info.typ.prodCons[i, 0]; j++)
                {
                    produkce[x].GetComponent<RawImage>().texture = textury[i];
                    produkce[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    x++;
                    if (x > 2)
                        x = 2;
                }
            }
            else
            {
                if (info.typ.prodCons[i, 0] > 0)
                {
                    produkce[x].GetComponent<RawImage>().texture = textury[i];
                    produkce[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    produkce[x].transform.FindChild("hodnota").GetComponent<Text>().text = info.typ.prodCons[i, 0].ToString();
                    x++;
                    if (x > 2)
                        x = 2;
                }
            }
        }
        if (info.typ.akceNavic)
        {
            produkce[x].GetComponent<RawImage>().texture = textury[4];
            produkce[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            x++;
            if (x > 2)
                x = 2;
        }
        if (info.typ.charita)
        {
            produkce[x].GetComponent<RawImage>().texture = textury[5];
            produkce[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            x++;
            if (x > 2)
                x = 2;
        }

        //spotřeba
        List<Transform> spotreba = new List<Transform>();
        spotreba.Add(GameObject.Find("cons0").transform.FindChild("image"));
        spotreba.Add(GameObject.Find("cons1").transform.FindChild("image"));
        spotreba.Add(GameObject.Find("cons2").transform.FindChild("image"));
        int y = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i != 3)
            {
                for (int j = 0; j < info.typ.prodCons[i, 1]; j++)
                {
                    spotreba[y].GetComponent<RawImage>().texture = textury[i];
                    spotreba[y].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    y++;
                    if (y > 2)
                        y = 2;
                }
            }
            else
            {
                if (info.typ.prodCons[i, 1] > 0)
                {
                    spotreba[y].GetComponent<RawImage>().texture = textury[i];
                    spotreba[y].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    spotreba[y].transform.FindChild("hodnota").GetComponent<Text>().text = info.typ.prodCons[i, 1].ToString();
                    y++;
                    if (y > 2)
                        y = 2;
                }
            }
        }
    }

    private void SmazaniProdCons()
    {
        GameObject[] prods = GameObject.FindGameObjectsWithTag("prod");
        GameObject[] conss = GameObject.FindGameObjectsWithTag("cons");
        foreach (GameObject p in prods)
        {
            p.GetComponent<RawImage>().texture = null;
            p.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
            p.transform.FindChild("hodnota").GetComponent<Text>().text = null;
        }
        foreach (GameObject p in conss)
        {
            p.GetComponent<RawImage>().texture = null;
            p.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
            p.transform.FindChild("hodnota").GetComponent<Text>().text = null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(gameObject.GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !wait)
        {
            Skryt();
        }
        if (wait)
        {
            cooldown--;
            if (cooldown == 0)
                wait = false;
        }
    }

    private void Skryt()
    {
        gameObject.GetComponent<CanvasRenderer>().transform.localScale = new Vector3(0, 0, 0);
        SmazaniProdCons();
    }

    private int VypocetZisku(Budova info)
    {
        int prEl = info.typ.prodCons[0, 0] * Ceny.elektrina;
        int prLa = info.typ.prodCons[1, 0] * Ceny.prsila;
        int prTr = info.typ.prodCons[2, 0] * Ceny.auta;
        int prCa = info.typ.prodCons[3, 0];
        int prijem = prEl + prLa + prTr + prCa;

        int vyEl = info.typ.prodCons[0, 1] * Ceny.elektrina;
        int vyLa = info.typ.prodCons[1, 1] * Ceny.prsila;
        int vyTr = info.typ.prodCons[2, 1] * Ceny.auta;
        int vyCa = info.typ.prodCons[3, 1];
        int vydaje = vyEl + vyLa + vyTr + vyCa;

        return prijem - vydaje;
    }

    public void ClickUp()
    {
        if (!wait)
        {
            Skryt();
            (GameObject.Find("UpgradePanel").GetComponent("UpgradeDowngrade") as UpgradeDowngrade).Zobrazit(bud, true);
        }
    }

    public void ClickDown()
    {
        if (!wait)
        {
            Skryt();
            if (bud.typ.uroven > 1)
                (GameObject.Find("UpgradePanel").GetComponent("UpgradeDowngrade") as UpgradeDowngrade).Zobrazit(bud, false);
            else if (bud.typ.uroven == 1)
            {
                Hra h = GameObject.Find("Hra").GetComponent("Hra") as Hra;
                h.Akce();
                bud.ZmenaStavby(Budovy.sX);
                
            }
        }
    }
}
