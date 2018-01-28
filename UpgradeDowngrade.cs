using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDowngrade : MonoBehaviour {

    Budovy budovy = new Budovy();
    bool wait = false;
    private int selOdv = 0;
    bool up = false;
    private Budova bud;
    private int selIndex;

    public Texture2D texE;
    public Texture2D texL;
    public Texture2D texT;
    public Texture2D texC;
    public Texture2D texA;
    public Texture2D texCh;

    private List<Stavba> zobrazeneStavby;

    public void Zobrazit(Budova budova, bool upgrade)
    {
        bud = budova;
        up = upgrade;
        selOdv = budova.typ.odvetvi;
        selIndex = -1;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("tActual").GetComponent<Text>().text = string.Format("Aktuálně: {0}", budova.typ.nazev);
        ZobrazitOdvetvi();
        vykreslitZvolenyIndex();
        GameObject.Find("tlOdvDals").GetComponent<Button>().interactable = (upgrade && budova.typ.uroven <= 1);
        GameObject.Find("tlOdvPred").GetComponent<Button>().interactable = (upgrade && budova.typ.uroven <= 1);
        wait = true;
    }

    private void ZobrazitOdvetvi()
    {
        List<Stavba> platneStavby = new List<Stavba>();
        VycistiOdvetvi();
        foreach (Stavba stavba in budovy.odvetvi[selOdv].platneStavby)
        {
            if (up && (stavba.uroven == bud.typ.uroven || (stavba.uroven == bud.typ.uroven + 1 && (stavba.odvetvi == bud.typ.odvetvi || bud.typ.odvetvi == 6))) && stavba != bud.typ)
            {
                platneStavby.Add(stavba);
            }
            else if (!up && stavba.uroven == bud.typ.uroven - 1 && stavba != bud.typ)   
            {
                platneStavby.Add(stavba);
            }
        }
        GameObject.Find("tBranch").GetComponent<Text>().text = string.Format("Odvětví: {0}", budovy.odvetvi[selOdv].nazev);
        if (platneStavby.Count > 0)
        {
            GameObject.Find("StavbaA").transform.FindChild("tStavba").GetComponent<Text>().text = platneStavby[0].nazev;
            VykreslitProdCons("StavbaA", platneStavby[0]);
            if (platneStavby.Count > 1)
            {
                GameObject.Find("StavbaB").transform.FindChild("tStavba").GetComponent<Text>().text = platneStavby[1].nazev;
                VykreslitProdCons("StavbaB", platneStavby[1]);
                if (platneStavby.Count > 2)
                {
                    GameObject.Find("StavbaC").transform.FindChild("tStavba").GetComponent<Text>().text = platneStavby[2].nazev;
                    VykreslitProdCons("StavbaC", platneStavby[2]);
                }
            }
        }
        zobrazeneStavby = platneStavby;
    }

    private void VykreslitProdCons(string stavba, Stavba stavby)
    {
        Texture2D[] textury = new Texture2D[] { texE, texL, texT, texC, texA, texCh };
        List<Transform> produkce = new List<Transform>();
        List<Transform> spotreba = new List<Transform>();
        produkce.Add(GameObject.Find(stavba).transform.FindChild("bPro").transform.FindChild("pro0").transform.FindChild("image"));
        produkce.Add(GameObject.Find(stavba).transform.FindChild("bPro").transform.FindChild("pro1").transform.FindChild("image"));
        produkce.Add(GameObject.Find(stavba).transform.FindChild("bPro").transform.FindChild("pro2").transform.FindChild("image"));
        spotreba.Add(GameObject.Find(stavba).transform.FindChild("bCon").transform.FindChild("con0").transform.FindChild("image"));
        spotreba.Add(GameObject.Find(stavba).transform.FindChild("bCon").transform.FindChild("con1").transform.FindChild("image"));
        spotreba.Add(GameObject.Find(stavba).transform.FindChild("bCon").transform.FindChild("con2").transform.FindChild("image"));

        int x = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i != 3)
            {
                for (int j = 0; j < stavby.prodCons[i, 0]; j++)
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
                if (stavby.prodCons[i, 0] > 0)
                {
                    produkce[x].GetComponent<RawImage>().texture = textury[i];
                    produkce[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    produkce[x].transform.FindChild("hodnota").GetComponent<Text>().text = stavby.prodCons[i, 0].ToString();
                    x++;
                    if (x > 2)
                        x = 2;
                }
            }
        }
        if (stavby.akceNavic)
        {
            produkce[x].GetComponent<RawImage>().texture = textury[4];
            produkce[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            x++;
            if (x > 2)
                x = 2;
        }
        if (stavby.charita)
        {
            produkce[x].GetComponent<RawImage>().texture = textury[5];
            produkce[x].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
            x++;
            if (x > 2)
                x = 2;
        }

        int y = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i != 3)
            {
                for (int j = 0; j < stavby.prodCons[i, 1]; j++)
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
                if (stavby.prodCons[i, 1] > 0)
                {
                    spotreba[y].GetComponent<RawImage>().texture = textury[i];
                    spotreba[y].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    spotreba[y].transform.FindChild("hodnota").GetComponent<Text>().text = stavby.prodCons[i, 1].ToString();
                    y++;
                    if (y > 2)
                        y = 2;
                }
            }
        }
    }

    private void VycistiOdvetvi()
    {
        GameObject.Find("StavbaA").transform.FindChild("tStavba").GetComponent<Text>().text = "";
        GameObject.Find("StavbaB").transform.FindChild("tStavba").GetComponent<Text>().text = "";
        GameObject.Find("StavbaC").transform.FindChild("tStavba").GetComponent<Text>().text = "";
        GameObject[] proCons = GameObject.FindGameObjectsWithTag("proConUpgrade");
        foreach (GameObject p in proCons)
        {
            p.GetComponent<RawImage>().texture = null;
            p.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
            p.transform.FindChild("hodnota").GetComponent<Text>().text = null;
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(gameObject.GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !wait)
                Skryt();
            else if (RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("StavbaA").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !wait && GameObject.Find("StavbaA").transform.FindChild("tStavba").GetComponent<Text>().text != "")
                selIndex = 0;
            else if (RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("StavbaB").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !wait && GameObject.Find("StavbaB").transform.FindChild("tStavba").GetComponent<Text>().text != "")
                selIndex = 1;
            else if (RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("StavbaC").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !wait && GameObject.Find("StavbaC").transform.FindChild("tStavba").GetComponent<Text>().text != "")
                selIndex = 2;
            vykreslitZvolenyIndex();
        }
        if (wait)
        {
            wait = false;
        }
    }

    public void Skryt()
    {
        gameObject.GetComponent<CanvasRenderer>().transform.localScale = new Vector3(0, 0, 0);
    }

    public void DalsiOdvetvi()
    {
        selOdv++;
        if (selOdv > 5)
            selOdv = 0;
        ZobrazitOdvetvi();
        selIndex = -1;
        vykreslitZvolenyIndex();
    }

    public void PredchoziOdvetvi()
    {
        selOdv--;
        if (selOdv < 0)
            selOdv = 5;
        ZobrazitOdvetvi();
        selIndex = -1;
        vykreslitZvolenyIndex();
    }

    private void vykreslitZvolenyIndex()
    {
        Vector3 x = new Vector3(0, 0, 0);
        switch(selIndex)
        {
            case 0:
                x.x = 0.5f;
                break;
            case 1:
                x.y = 0.5f;
                break;
            case 2:
                x.z = 0.5f;
                break;
            default:
                break;
        }
        GameObject.Find("StavbaA").GetComponent<Image>().color = new Color(0, 0, x.x, 0.5f);
        GameObject.Find("StavbaB").GetComponent<Image>().color = new Color(0, 0, x.y, 0.5f);
        GameObject.Find("StavbaC").GetComponent<Image>().color = new Color(0, 0, x.z, 0.5f);
        Hra h = GameObject.Find("Hra").GetComponent("Hra") as Hra;
        if (selIndex != -1 && ((h.hraci[h.hracNaRade].finance >= 4 && bud.typ.uroven == 0) || h.hraci[h.hracNaRade].finance >= 1 && bud.typ.uroven != 0) && h.dostupneAkce > 0)
        {
            GameObject.Find("tlConf").GetComponent<Button>().interactable = true;
            if (bud.typ.uroven == 0)
                GameObject.Find("tConf").GetComponent<Text>().text = string.Format("POTVRDIT ({0},000,000 Kč)", 4);
            else
                GameObject.Find("tConf").GetComponent<Text>().text = string.Format("POTVRDIT ({0},000,000 Kč)", 1);
        }
        else
        {
            GameObject.Find("tlConf").GetComponent<Button>().interactable = false;
            GameObject.Find("tConf").GetComponent<Text>().text = string.Format("POTVRDIT (nelze)");
        }
    }

    public void PotvrzeniStavby()
    {
        Skryt();
        Hra h = GameObject.Find("Hra").GetComponent("Hra") as Hra;
        h.Akce();
        if (bud.typ.uroven == 0)
            h.hraci[h.hracNaRade].Finance(4, false);
        else
            h.hraci[h.hracNaRade].Finance(1, false);
        h.VypsaniInfoHracNaRade();
        bud.ZmenaStavby(zobrazeneStavby[selIndex]);
    }
}
