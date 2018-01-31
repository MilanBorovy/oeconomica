using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPrices : MonoBehaviour {

    public void Start()
    {
        Prices.Start();
    }
    	
	void Update ()
    {
        GameObject.Find("ElectricityPrice").GetComponent<Text>().text = string.Format("{0},000,000 Kč", Prices.Electricity);
        GameObject.Find("LabourPrice").GetComponent<Text>().text = string.Format("{0},000,000 Kč", Prices.Labour);
        GameObject.Find("VehiclesPrice").GetComponent<Text>().text = string.Format("{0},000,000 Kč", Prices.Vehicles);

        ProductionConsumptionRate total_pcrate = HUDProduction.TotalPCRate;

        DisplayExpectation(total_pcrate.p_electricity, total_pcrate.c_electricity, "ElectricityExpectation");
        DisplayExpectation(total_pcrate.p_labour, total_pcrate.c_labour, "LabourExpectation");
        DisplayExpectation(total_pcrate.p_vehicles, total_pcrate.c_vehicles, "VehiclesExpectation");
    }

    private void DisplayExpectation(int production, int consumption, string hud_element)
    {
        GameObject.Find(hud_element).GetComponent<Text>().text = 
            production > consumption ? 
            "▼" :
            production < consumption ?
            "▲" :
            "-";
        GameObject.Find(hud_element).GetComponent<Text>().color =
            production > consumption ?
            new Color(200f / 255f, 50f / 255f, 50f / 255f) :
            production < consumption ?
            new Color(50f / 255f, 200f / 255f, 50f / 255f) :
            new Color(50f / 255f, 50f / 255f, 50f / 255f);
    }
}
