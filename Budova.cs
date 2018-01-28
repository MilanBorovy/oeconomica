using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Budova : MonoBehaviour {

    public Stavba typ { get; private set; }
    
    void Start()
    {
        typ = Budovy.sX;
    }

    void OnMouseDown()
    {
        if(!RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("OvladaniBudovy").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("UpgradePanel").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("ProdConsBars").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("CostsBar").GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
           (GameObject.Find("OvladaniBudovy").GetComponent("OvladaniBudovyS") as OvladaniBudovyS).Zobrazit(gameObject);
    }

    public void ZmenaStavby(Stavba stavba)
    {
        typ = stavba;
    }
}
