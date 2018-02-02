using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Oeconomica.Game.HUD;
using Oeconomica.Game;

namespace Oeconomica.Game.BuildingsNS
{
    public class Building : MonoBehaviour
    {

        public Buildings ActualBuilding { get; private set; } //Actual building type
        public Player Owner { get; private set; }
        private Object BuildingInstance; //In-game representation of building

        void Start()
        {
            ActualBuilding = Buildings.EMPTY;
            BuildingInstance = Instantiate(BuildingsExtensions.GetModel(ActualBuilding), gameObject.transform);
            string parentName = gameObject.transform.parent.name;
            int x = int.Parse(parentName[parentName.Length - 1].ToString());
            Owner = GameLogic.players[x];
        }

        void OnMouseDown()
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("BuildingOverview").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
                !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("BuildingUpgrade").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
                !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("Commodities").GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
                !RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("Costs").GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
                (GameObject.Find("BuildingOverview").GetComponent("BuildingControl") as BuildingControl).Show(gameObject);
        }

        public void HighlightBuilding(bool highlight)
        {
            (BuildingInstance as GameObject).layer = highlight ? LayerMask.NameToLayer("Outline") : LayerMask.NameToLayer("Default");
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
}
