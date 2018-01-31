using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        upgrade.interactable = display != 0;
        downgrade.interactable = display != 0 && BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) != 0;

        //Set buttons text
        GameObject.Find("UpgradeTag").GetComponent<Text>().text = buttonLabels[BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding), 0];
        GameObject.Find("DowngradeTag").GetComponent<Text>().text = buttonLabels[BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding), 1];

        ClearPC();
        DisplayPC(buildingLogic);
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
            p.GetComponent<RawImage>().texture = null;
            p.transform.Find("Value").GetComponent<Text>().text = null;
        }

        foreach (GameObject c in consumptionSlots)
        {
            c.GetComponent<RawImage>().texture = null;
            c.transform.Find("Value").GetComponent<Text>().text = null;
        }
    }

    /// <summary>
    /// Shows production & consumption overview of specified buildig
    /// </summary>
    /// <param name="building">Building</param>
    private void DisplayPC(Building building)
    {
        Texture2D[] icons = new Texture2D[] 
        {
            (Texture2D)Resources.Load("Icons/electricity"),
            (Texture2D)Resources.Load("Icons/labour"),
            (Texture2D)Resources.Load("Icons/vehicles"),
            (Texture2D)Resources.Load("Icons/money"),
            (Texture2D)Resources.Load("Icons/action"),
            (Texture2D)Resources.Load("Icons/charity")
        };

        //Get slots
        GameObject[] productionSlots = GameObject.FindGameObjectsWithTag("Production");
        GameObject[] consumptionSlots = GameObject.FindGameObjectsWithTag("Consumption");

        //Get production&consumption rate
        ProductionConsumptionRate pcrate = BuildingsExtensions.GetPCRate(building.ActualBuilding);
        int[,] pcrate_array = new int[4, 2] 
        { 
            { pcrate.p_electricity, pcrate.c_electricity },
            { pcrate.p_labour, pcrate.c_labour },
            { pcrate.p_vehicles, pcrate.c_vehicles },
            { pcrate.p_money, pcrate.c_money }
        };

        //Index of last empty slot for production&consumption
        int productionIdx = 0;
        int consumptionIdx = 0;

        for (int commodity = 0; commodity < 4; commodity++) //Cycle going through each commodity
        {
            if (commodity != 3) //Special rule for money
            {
                //Draw commodities separately
                //Production
                for (int quantity = 0; quantity < pcrate_array[commodity, 0]; quantity++)
                {
                    productionSlots[productionIdx].GetComponent<RawImage>().texture = icons[commodity];
                    productionIdx = productionIdx > 1 ? 2 : productionIdx + 1;
                }

                //Consumption
                for (int quantity = 0; quantity < pcrate_array[commodity, 1]; quantity++)
                {
                    consumptionSlots[consumptionIdx].GetComponent<RawImage>().texture = icons[commodity];
                    consumptionIdx = consumptionIdx > 1 ? 2 : consumptionIdx + 1;
                }
            }
            //Draw money
            else
            {
                //Production
                if (pcrate_array[commodity, 0] > 0)
                {
                    productionSlots[productionIdx].GetComponent<RawImage>().texture = icons[commodity];
                    productionSlots[productionIdx].transform.FindChild("Value").GetComponent<Text>().text = pcrate_array[commodity, 0].ToString();
                    productionIdx = productionIdx > 1 ? 2 : productionIdx + 1;
                }

                //Consumption
                if (pcrate_array[commodity, 1] > 0)
                {
                    consumptionSlots[consumptionIdx].GetComponent<RawImage>().texture = icons[commodity];
                    consumptionSlots[consumptionIdx].transform.FindChild("Value").GetComponent<Text>().text = pcrate_array[commodity, 1].ToString();
                    consumptionIdx = consumptionIdx > 1 ? 2 : consumptionIdx + 1;
                }
            }
        }
        if (pcrate.action) //Draw bonus action
        {
            productionSlots[productionIdx].GetComponent<RawImage>().texture = icons[4];
            productionIdx = productionIdx > 1 ? 2 : productionIdx + 1;
        }
        if (pcrate.charity) //Draw bonus charity
        {
            productionSlots[productionIdx].GetComponent<RawImage>().texture = icons[5];
            productionIdx = productionIdx > 1 ? 2 : productionIdx + 1;
        }
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
