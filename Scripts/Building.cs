using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Oeconomica.Game.HUD;
using Oeconomica.Game;
using UnityEngine.EventSystems;

namespace Oeconomica.Game.BuildingsNS
{
    public class Building : MonoBehaviour
    {

        public Buildings ActualBuilding { get; private set; } //Actual building type
        public Player Owner { get; private set; }
        private GameObject BuildingInstance; //In-game representation of building

        void Start()
        {
            ActualBuilding = Buildings.EMPTY;
            BuildingInstance = (GameObject)Instantiate(BuildingsExtensions.GetModel(ActualBuilding), gameObject.transform);
        }

        void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                (GameObject.Find("BuildingOverview").GetComponent("BuildingControl") as BuildingControl).Show(this);
        }

        public void HighlightBuilding(bool highlight)
        {
            BuildingInstance.transform.GetChild(0).gameObject.layer = highlight ? LayerMask.NameToLayer("Outline") : LayerMask.NameToLayer("Default");
        }

        /// <summary>
        /// Change type of building
        /// </summary>
        /// <param name="building">New building type</param>
        public void ChangeBuilding(Buildings building)
        {
            ActualBuilding = building;
            Destroy(BuildingInstance); //Remove in-game representation of old building
            BuildingInstance = (GameObject)Instantiate(BuildingsExtensions.GetModel(ActualBuilding), gameObject.transform); //Add new building to the scene
        }

        /// <summary>
        /// Sets owner of building
        /// </summary>
        /// <param name="player">New owner of building</param>
        public void SetOwner(Player player)
        {
            this.Owner = player;
        }
    }
}
