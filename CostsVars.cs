using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostsVars : MonoBehaviour {

    public void Start()
    {
        Ceny.Start();
    }
    	
	void Update () {
        GameObject.Find("eCost").GetComponent<Text>().text = string.Format("{0},000,000 Kč", Ceny.elektrina);
        GameObject.Find("lCost").GetComponent<Text>().text = string.Format("{0},000,000 Kč", Ceny.prsila);
        GameObject.Find("tCost").GetComponent<Text>().text = string.Format("{0},000,000 Kč", Ceny.auta);

        int EP = Mathf.CeilToInt(GameObject.Find("eProdFront").GetComponent<Image>().fillAmount * 20);
        int EC = Mathf.CeilToInt(GameObject.Find("eConsFront").GetComponent<Image>().fillAmount * 20);
        int LP = Mathf.CeilToInt(GameObject.Find("lProdFront").GetComponent<Image>().fillAmount * 20);
        int LC = Mathf.CeilToInt(GameObject.Find("lConsFront").GetComponent<Image>().fillAmount * 20);
        int TP = Mathf.CeilToInt(GameObject.Find("tProdFront").GetComponent<Image>().fillAmount * 20);
        int TC = Mathf.CeilToInt(GameObject.Find("tConsFront").GetComponent<Image>().fillAmount * 20);

        if (EP > EC)
        {
            GameObject.Find("eVyvoj").GetComponent<Text>().text = "▼";
            GameObject.Find("eVyvoj").GetComponent<Text>().color = new Color(200f / 255f, 50f / 255f, 50f / 255f);
        }
        else if (EP < EC)
        {
            GameObject.Find("eVyvoj").GetComponent<Text>().text = "▲";
            GameObject.Find("eVyvoj").GetComponent<Text>().color = new Color(50f / 255f, 200f / 255f, 50f / 255f);
        }
        else
        {
            GameObject.Find("eVyvoj").GetComponent<Text>().text = "-";
            GameObject.Find("eVyvoj").GetComponent<Text>().color = new Color(50f / 255f, 50f / 255f, 50f/255f);
        }

        if (LP > LC)
        {
            GameObject.Find("lVyvoj").GetComponent<Text>().text = "▼";
            GameObject.Find("lVyvoj").GetComponent<Text>().color = new Color(200f / 255f, 50f / 255f, 50f / 255f);
        }
        else if (LP < LC)
        {
            GameObject.Find("lVyvoj").GetComponent<Text>().text = "▲";
            GameObject.Find("lVyvoj").GetComponent<Text>().color = new Color(50f / 255f, 200f / 255f, 50f / 255f);
        }
        else
        {
            GameObject.Find("lVyvoj").GetComponent<Text>().text = "-";
            GameObject.Find("lVyvoj").GetComponent<Text>().color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
        }

        if (TP > TC)
        {
            GameObject.Find("tVyvoj").GetComponent<Text>().text = "▼";
            GameObject.Find("tVyvoj").GetComponent<Text>().color = new Color(200f / 255f, 50f / 255f, 50f / 255f);
        }
        else if (TP < TC)
        {
            GameObject.Find("tVyvoj").GetComponent<Text>().text = "▲";
            GameObject.Find("tVyvoj").GetComponent<Text>().color = new Color(50f / 255f, 200f / 255f, 50f / 255f);
        }
        else
        {
            GameObject.Find("tVyvoj").GetComponent<Text>().text = "-";
            GameObject.Find("tVyvoj").GetComponent<Text>().color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
        }
    }
}
