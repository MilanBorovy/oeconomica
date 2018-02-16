using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oeconomica.Game.HUD;
using Oeconomica.Game;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace Oeconomica.Game.BuildingsNS
{
    public class Building : NetworkBehaviour
    {

        [SyncVar(hook = "OnBuildingChanged")]private Buildings _actualBuilding;
        public Buildings ActualBuilding {
            get
            {
                return _actualBuilding;
            }
            private set
            {
                _actualBuilding = value;
            }
        }//Actual building type

        public Player Owner { get; private set; }
        private GameObject BuildingInstance; //In-game representation of building

        void Start()
        {
            if (isServer)
                CmdChangeBuilding(Buildings.EMPTY);
            else
                OnBuildingChanged(ActualBuilding);
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
        [Command]
        public void CmdChangeBuilding(Buildings building)
        {
            ActualBuilding = building;

            Debug.Log("Building changed to " + ActualBuilding.GetName());
        }
        
        private void OnBuildingChanged(Buildings value)
        {
            Destroy(BuildingInstance); //Remove in-game representation of old building
            BuildingInstance = (GameObject)Instantiate(value.GetModel(), gameObject.transform); //Add new building to the scene
            ActualBuilding = value;
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
