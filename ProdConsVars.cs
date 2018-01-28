using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProdConsVars : MonoBehaviour {
    
    GameObject[] budovy;

    void Start()
    {
        budovy = GameObject.FindGameObjectsWithTag("Budova");
    }

    void Update()
    {
        int
            eProd = 0,
            eCons = 0,
            lProd = 0,
            lCons = 0,
            tProd = 0,
            tCons = 0;
        foreach (GameObject budova in budovy)
        {
            Budova script = budova.GetComponent("Budova") as Budova;
            eProd += script.typ.prodCons[0, 0];
            eCons += script.typ.prodCons[0, 1];
            lProd += script.typ.prodCons[1, 0];
            lCons += script.typ.prodCons[1, 1];
            tProd += script.typ.prodCons[2, 0];
            tCons += script.typ.prodCons[2, 1];
        }
        float eP = (float)eProd / 10.0f;
        float eC = (float)eCons / 10.0f;
        float lP = (float)lProd / 10.0f;
        float lC = (float)lCons / 10.0f;
        float tP = (float)tProd / 10.0f;
        float tC = (float)tCons / 10.0f;

        float EP = GameObject.Find("eProdFront").GetComponent<Image>().fillAmount;
        float EC = GameObject.Find("eConsFront").GetComponent<Image>().fillAmount;
        float LP = GameObject.Find("lProdFront").GetComponent<Image>().fillAmount;
        float LC = GameObject.Find("lConsFront").GetComponent<Image>().fillAmount;
        float TP = GameObject.Find("tProdFront").GetComponent<Image>().fillAmount;
        float TC = GameObject.Find("tConsFront").GetComponent<Image>().fillAmount;

        if (EP > eP)
            EP = Mathf.Clamp(EP - Time.deltaTime, eP, 1f);
        else if (EP < eP)
            EP = Mathf.Clamp(EP + Time.deltaTime, 0f, eP);
        if (EC > eC)
            EC = Mathf.Clamp(EC - Time.deltaTime, eC, 1f);
        else if (EC < eC)
            EC = Mathf.Clamp(EC + Time.deltaTime, 0f, eC);
        if (LP > lP)
            LP = Mathf.Clamp(LP - Time.deltaTime, lP, 1f);
        else if (LP < lP)
            LP = Mathf.Clamp(LP + Time.deltaTime, 0f, lP);
        if (LC > lC)
            LC = Mathf.Clamp(LC - Time.deltaTime, lC, 1f);
        else if (LC < lC)
            LC = Mathf.Clamp(LC + Time.deltaTime, 0f, lC);
        if (TP > tP)
            TP = Mathf.Clamp(TP - Time.deltaTime, tP, 1f);
        else if (TP < tP)
            TP = Mathf.Clamp(TP + Time.deltaTime, 0f, tP);
        if (TC > tC)
            TC = Mathf.Clamp(TC - Time.deltaTime, tC, 1f);
        else if (TC < tC)
            TC = Mathf.Clamp(TC + Time.deltaTime, 0f, tC);

        GameObject.Find("eProdFront").GetComponent<Image>().fillAmount = EP;
        GameObject.Find("eConsFront").GetComponent<Image>().fillAmount = EC;
        GameObject.Find("lProdFront").GetComponent<Image>().fillAmount = LP;
        GameObject.Find("lConsFront").GetComponent<Image>().fillAmount = LC;
        GameObject.Find("tProdFront").GetComponent<Image>().fillAmount = TP;
        GameObject.Find("tConsFront").GetComponent<Image>().fillAmount = TC;
    }
}
