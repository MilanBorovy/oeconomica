using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Oeconomica.Game.CommoditiesNS;
using Oeconomica.Game.BranchesNS;

namespace Oeconomica.Game.BuildingsNS
{
    public static class BuildingsExtensions
    {
        /// <summary>
        /// Gets name associated with building
        /// </summary>
        /// <param name="building">Building type</param>
        /// <returns>String name of building</returns>
        public static string GetName(this Buildings building)
        {
            return building.GetAttribute<BuildingsAttribute>().name;
        }
        /// <summary>
        /// Gets kind of business associated with building
        /// </summary>
        /// <param name="building">Building type</param>
        /// <returns>Kind of business</returns>
        public static Branches GetBranch(this Buildings building)
        {
            return building.GetAttribute<BuildingsAttribute>().branch;
        }
        /// <summary>
        /// Gets level of building associated with building
        /// </summary>
        /// <param name="building">Building type</param>
        /// <returns>Level of building</returns>
        public static int GetGrade(this Buildings building)
        {
            return building.GetAttribute<BuildingsAttribute>().grade;
        }
        /// <summary>
        /// Gets production & consumption of building, including bonus action&charity
        /// </summary>
        /// <param name="building">Building type</param>
        /// <returns>Production & Consumption struct</returns>
        public static ProductionConsumptionRate GetPCRate(this Buildings building)
        {
            return building.GetAttribute<BuildingsAttribute>().pcrate;
        }
        /// <summary>
        /// Gets model representation associated with building
        /// </summary>
        /// <param name="building">Building type</param>
        /// <returns>Model of building</returns>
        public static UnityEngine.Object GetModel(this Buildings building)
        {
            return building.GetAttribute<BuildingsAttribute>().model;
        }
        /// <summary>
        /// Gets all buildings
        /// </summary>
        /// <returns>Every building</returns>
        public static GameObject[] All()
        {
            return GameObject.FindGameObjectsWithTag("Building");
        }
        /// <summary>
        /// Draws production & consumption rate of building
        /// </summary>
        /// <param name="productionCol">Production display slots</param>
        /// <param name="consumptionCol">Consumption display slots</param>
        /// <param name="building">Building</param>
        public static void DrawPC(string productionCol, string consumptionCol, Buildings building)
        {
            //Icons textures
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
            GameObject[] productionSlots = GameObject.FindGameObjectsWithTag(productionCol);
            GameObject[] consumptionSlots = GameObject.FindGameObjectsWithTag(consumptionCol);

            //Get production&consumption rate
            ProductionConsumptionRate pcrate = BuildingsExtensions.GetPCRate(building);
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
                        DrawToSlot(ref productionIdx, productionSlots, icons[commodity]);
                    }

                    //Consumption
                    for (int quantity = 0; quantity < pcrate_array[commodity, 1]; quantity++)
                    {
                        DrawToSlot(ref consumptionIdx, consumptionSlots, icons[commodity]);
                    }
                }
                //Draw money
                else
                {
                    //Production
                    if (pcrate_array[commodity, 0] > 0)
                    {
                        DrawToSlot(ref productionIdx, productionSlots, icons[commodity], pcrate.p_money);
                    }

                    //Consumption
                    if (pcrate_array[commodity, 1] > 0)
                    {
                        DrawToSlot(ref consumptionIdx, consumptionSlots, icons[commodity], pcrate.c_money);
                    }
                }
            }
            if (pcrate.action) //Draw bonus action
            {
                DrawToSlot(ref productionIdx, productionSlots, icons[4]);
            }
            if (pcrate.charity) //Draw bonus charity
            {
                DrawToSlot(ref productionIdx, productionSlots, icons[5]);
            }
        }

        private static void DrawToSlot(ref int slot, GameObject[] slots, Texture2D icon)
        {
            DrawToSlot(ref slot, slots, icon, 0);
        }

        /// <summary>
        /// Draws icon into specified slot with specified value
        /// </summary>
        /// <param name="slot">Slot in which to draw</param>
        /// <param name="slots">Collection of slots</param>
        /// <param name="icon">Icon to draw</param>
        /// <param name="value">Value to draw</param>
        private static void DrawToSlot(ref int slot, GameObject[] slots, Texture2D icon, int value)
        {
            foreach (GameObject s in slots)
            {
                if (s.transform.parent.name == "Slot" + slot)
                {
                    s.GetComponent<RawImage>().texture = icon;
                    s.GetComponentInChildren<Text>().text = value > 0 ? value.ToString() : "";
                    slot = slot > 1 ? 2 : slot + 1;
                    break;
                }
            }
        }
    }
}
