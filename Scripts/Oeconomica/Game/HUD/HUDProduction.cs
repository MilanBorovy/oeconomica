using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oeconomica.Game.CommoditiesNS;
using Oeconomica.Game.BuildingsNS;

namespace Oeconomica.Game.HUD
{
    public class HUDProduction : MonoBehaviour
    {

        //Summarizes all production and consumption of commodities (not money or bonus actions & charity)
        public static ProductionConsumptionRate TotalPCRate
        {
            get
            {
                int
                    electricity_production = 0, electricity_consumption = 0,
                    labour_production = 0, labour_consumption = 0,
                    vehicles_production = 0, vehicles_consumption = 0;

                foreach (GameObject building in BuildingsExtensions.All())
                {
                    Building b = building.GetComponent("Building") as Building;
                    ProductionConsumptionRate pcrate = b.ActualBuilding.GetPCRate();
                    electricity_production += pcrate.p_electricity;
                    electricity_consumption += pcrate.c_electricity;
                    labour_production += pcrate.p_labour;
                    labour_consumption += pcrate.c_labour;
                    vehicles_production += pcrate.p_vehicles;
                    vehicles_consumption += pcrate.c_vehicles;
                }

                return new ProductionConsumptionRate
                    (
                        new int[8]
                        {
                        electricity_production, electricity_consumption,
                        labour_production, labour_consumption,
                        vehicles_production, vehicles_consumption,
                        0, 0
                        },
                        false, false
                    );
            }
        }

        void Start()
        {
        }

        void Update()
        {

            //Summary
            ProductionConsumptionRate total_pcrate = TotalPCRate;

            //Display new rates
            SetDisplayValue(total_pcrate.p_electricity, "ElectricityProduction");
            SetDisplayValue(total_pcrate.c_electricity, "ElectricityConsumption");
            SetDisplayValue(total_pcrate.p_labour, "LabourProduction");
            SetDisplayValue(total_pcrate.c_labour, "LabourConsumption");
            SetDisplayValue(total_pcrate.p_vehicles, "VehiclesProduction");
            SetDisplayValue(total_pcrate.c_vehicles, "VehiclesConsumption");
        }

        /// <summary>
        /// Displays new rates of production and consumption
        /// </summary>
        /// <param name="wanted">Target value</param>
        /// <param name="hud">Name of hud element, indicating rate of production or consumption</param>
        private void SetDisplayValue(int wanted, string hud)
        {
            GameObject hud_element = GameObject.Find(hud); //Get element
            float old_value = hud_element.GetComponent<Image>().fillAmount; //Get actual value
            float new_value = (float)wanted / 20f; //Convert to <0;1>
            hud_element.GetComponent<Image>().fillAmount = //Display
                old_value > new_value ?
                Mathf.Clamp(old_value - Time.deltaTime, new_value, 1f) :
                old_value < new_value ?
                Mathf.Clamp(old_value + Time.deltaTime, 0f, new_value) :
                old_value;
        }
    }
}
