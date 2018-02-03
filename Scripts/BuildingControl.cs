using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oeconomica.Game.BuildingsNS;
using Oeconomica.Game.CommoditiesNS;
using Oeconomica.Game.Effects;
using UnityEngine.EventSystems;

namespace Oeconomica.Game.HUD
{
    public class BuildingControl : MonoBehaviour
    {
        //Wait for released MB for buttons & window
        private bool wait = false;

        //Building script for building
        private Building buildingLogic;

        //Is this window visible?
        private bool _visible;
        public bool Visible
        {
            get
            {
                return _visible;
            }
            private set
            {
                gameObject.transform.localScale = new Vector3(value ? 1 : 0, 1, 1);
                _visible = value;
            }
        }


        private void Start()
        {
            Visible = false; //Hide window
        }

        /// <summary>
        /// Show building control panel over specified building
        /// </summary>
        /// <param name="building">Building</param>
        public void Show(Building building)
        {
            if (Visible) //Close already opened menu before opening "new" one
                Hide();
            this.buildingLogic = building; //Get logic

            //Building highlight
            OutlineEffect.SetColor(buildingLogic.Owner.Color);
            buildingLogic.HighlightBuilding(true);

            Visible = true; //Show window

            gameObject.transform.Find("Name").GetComponent<Text>().text =
                string.Format("Budova: {0}", BuildingsExtensions.GetName(buildingLogic.ActualBuilding)); //Set name of building

            gameObject.transform.Find("Profit").GetComponent<Text>().text =
                string.Format("V tomto kole: {0},000,000 Kč", ProfitCalculation(buildingLogic)); //Set profit

            wait = true;

            //Buttons text
            string[,] buttonLabels = new string[,]
            {
            { "POSTAVIT", "NIC" },
            { "ZMĚNIT ↑/↔", "ZBOURAT" },
            { "ZMĚNIT ↑/↔", "ZMĚNIT ↓" },
            { "ZMĚNIT ↔", "ZMĚNIT ↓" }
            };

            //Buttons
            Button upgrade = GameObject.Find("Upgrade").GetComponent<Button>();
            Button downgrade = GameObject.Find("Downgrade").GetComponent<Button>();

            //Own building filter
            int display = building.transform.parent.name == "Player" + GameLogic.HasTurn.ID ? 1 : 0;

            //Set buttons visibility
            upgrade.transform.localScale = new Vector3(display, display, display);
            downgrade.transform.localScale = new Vector3(display, display, display);

            //Set buttons interactibility
            upgrade.interactable = display != 0 && GameLogic.actions > 0;
            downgrade.interactable = display != 0 && BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) != 0 && GameLogic.actions > 0;

            //Set buttons text
            GameObject.Find("UpgradeTag").GetComponent<Text>().text = buttonLabels[BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding), 0];
            GameObject.Find("DowngradeTag").GetComponent<Text>().text = buttonLabels[BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding), 1];

            //Display production & consumption rates
            DisplayPC(buildingLogic.ActualBuilding);
        }

        /// <summary>
        /// Clears production & consumption overview of current building
        /// </summary>
        private void ClearPC()
        {
            //Get icons to clear
            GameObject[] productionSlots = GameObject.FindGameObjectsWithTag("Production");
            GameObject[] consumptionSlots = GameObject.FindGameObjectsWithTag("Consumption");

            //Clear icons
            foreach (GameObject p in productionSlots)
            {
                p.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Icons/none");
                p.transform.Find("Value").GetComponent<Text>().text = "";
            }

            foreach (GameObject c in consumptionSlots)
            {
                c.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Icons/none");
                c.transform.Find("Value").GetComponent<Text>().text = "";
            }
        }

        /// <summary>
        /// Shows production & consumption overview of specified buildig
        /// </summary>
        /// <param name="building">Building</param>
        private void DisplayPC(Buildings building)
        {
            ClearPC(); //Clear PC before redrawing
            BuildingsExtensions.DrawPC("Production", "Consumption", building);
        }

        private void Update()
        {
            if (Visible)
            {
                if (Input.GetMouseButtonDown(0) &&
                    !wait)
                {
                    //Click outside window
                    if (!EventSystem.current.IsPointerOverGameObject() && !wait)
                        Hide();
                }
                else if (!Input.GetMouseButtonDown(0) &&
                    wait)
                {
                    wait = false;
                }
            }
        }

        /// <summary>
        /// Hides building control window
        /// </summary>
        private void Hide()
        {
            buildingLogic.HighlightBuilding(false);
            Visible = false;
        }

        /// <summary>
        /// Calculates profit based on prices and production&consumption of building
        /// </summary>
        /// <param name="building">Building</param>
        /// <returns></returns>
        private int ProfitCalculation(Building building)
        {
            ProductionConsumptionRate pcrate = BuildingsExtensions.GetPCRate(building.ActualBuilding); //Production&consumption rates

            //Incomes
            int incomes = pcrate.p_electricity * Prices.Electricity;
            incomes += pcrate.p_labour * Prices.Labour;
            incomes += pcrate.p_vehicles * Prices.Vehicles;
            incomes += pcrate.p_money;

            //Expenses
            int expenses = pcrate.c_electricity * Prices.Electricity;
            expenses += pcrate.c_labour * Prices.Labour;
            expenses += pcrate.c_vehicles * Prices.Vehicles;
            expenses += pcrate.c_money;

            //Profit
            return incomes - expenses;
        }

        /// <summary>
        /// Opens upgrade menu
        /// </summary>
        public void Upgrade()
        {
            if (!wait)
            {
                Hide();
                (GameObject.Find("BuildingUpgrade").GetComponent("BuildingUpgrade") as BuildingUpgrade).Show(buildingLogic, true);
            }
        }

        /// <summary>
        /// Opens downgrade menu or destroys building
        /// </summary>
        public void Downgrade()
        {
            if (!wait)
            {
                Hide();
                if (BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) > 1) //Downgrade building
                    (GameObject.Find("BuildingUpgrade").GetComponent("BuildingUpgrade") as BuildingUpgrade).Show(buildingLogic, false);
                else if (BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) == 1) //Destroy building
                {
                    GameLogic.Action();
                    buildingLogic.ChangeBuilding(Buildings.EMPTY);
                }
            }
        }
    }
}