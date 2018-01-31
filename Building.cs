using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Building : MonoBehaviour {

    public Buildings ActualBuilding { get; private set; } //Actual building type
    private Object BuildingInstance; //In-game representation of building

    void Start()
    {
        ActualBuilding = Buildings.EMPTY;
        BuildingInstance = Instantiate(BuildingsExtensions.GetModel(ActualBuilding), gameObject.transform);
    }

    void OnMouseDown()
    {
        if(!RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("BuildingOverview").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
            !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("BuildingUpgrade").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
            !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("Commodities").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
            !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("Costs").GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
           (GameObject.Find("BuildingOverview").GetComponent("BuildingControl") as BuildingControl).Show(gameObject);
    }

    /// <summary>
    /// Change type of building
    /// </summary>
    /// <param name="building">New building type</param>
    public void ChangeBuilding(Buildings building)
    {
        ActualBuilding = building;
        Destroy(BuildingInstance); //Remove in-game representation of old building
        BuildingInstance = Instantiate(BuildingsExtensions.GetModel(ActualBuilding), gameObject.transform); //Add new building to the scene
    }
}
