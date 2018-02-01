using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oeconomica.Game.BuildingsNS;
using Oeconomica.Game.CommoditiesNS;

namespace Oeconomica.Game.HUD
{
    public class BuildingControl : MonoBehaviour
    {
        //Cooldown for buttons
        private int cooldown = 0;
        private bool wait = false;

        //Building script for building
        private Building buildingLogic;

        private void Start()
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0); //Hide window
        }

        /// <summary>
        /// Show building control panel over specified building
        /// </summary>
        /// <param name="building">Building</param>
        public void Show(GameObject building)
        {
            (GameObject.Find("BuildingUpgrade").GetComponent("BuildingUpgrade") as BuildingUpgrade).Hide();
            buildingLogic = building.GetComponent("Building") as Building; //Get logic

            gameObject.transform.localScale = new Vector3(1, 1, 1); //Show window
            gameObject.transform.parent.position = new Vector3(building.transform.position.x, 50, building.transform.position.z - 50); //Move panel to building
            gameObject.transform.Find("Name").GetComponent<Text>().text =
                string.Format("Budova: {0}", BuildingsExtensions.GetName(buildingLogic.ActualBuilding)); //Set name of building
            gameObject.transform.Find("Profit").GetComponent<Text>().text =
                string.Format("V tomto kole: {0},000,000 Kč", ProfitCalculation(buildingLogic)); //Set profit

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

            ClearPC();
            DisplayPC(buildingLogic.ActualBuilding);
            cooldown = 10;
            wait = true;
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
            BuildingsExtensions.DrawPC("Production", "Consumption", building);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) &&
                !RectTransformUtility.RectangleContainsScreenPoint(gameObject.GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
                !wait)
            {
                Hide();
            }
            if (wait)
            {
                cooldown--;
                wait = cooldown != 0;
            }
        }

        /// <summary>
        /// Hides building control window
        /// </summary>
        private void Hide()
        {
            gameObject.GetComponent<CanvasRenderer>().transform.localScale = new Vector3(0, 0, 0);
            ClearPC();
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
                (GameObject.Find("BuildingUpgrade").GetComponent("BuildingUpgrade") as BuildingUpgrade).Zobrazit(buildingLogic, true);
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
                    (GameObject.Find("BuildingUpgrade").GetComponent("BuildingUpgrade") as BuildingUpgrade).Zobrazit(buildingLogic, false);
                else if (BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) == 1) //Destroy building
                {
                    GameLogic.Action();
                    buildingLogic.ChangeBuilding(Buildings.EMPTY);
                }
            }
        }
    }
}