using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Oeconomica.Game.CommoditiesNS
{
    public static class Prices
    {
        //Development of production, consumption & price
        public struct Development
        {
            public int production { get; private set; }
            public int consumption { get; private set; }
            public int price { get; private set; }

            public Development(int production, int consumption, int price)
            {
                this.production = production;
                this.consumption = consumption;
                this.price = price;
            }
        }

        //Price of electricity
        private static int _electricity;
        public static int Electricity
        {
            get { return _electricity + 1; }
            set { _electricity = Clamp(_electricity + value); }
        }

        //Price of labour
        private static int _labour;
        public static int Labour
        {
            get { return _labour + 2; }
            set { _labour = Clamp(_labour + value); }
        }

        //Price of vehicles
        private static int _vehicles;
        public static int Vehicles
        {
            get { return _vehicles + 3; }
            set { _vehicles = Clamp(_vehicles + value); }
        }

        //Development of electricity
        private static List<Development> _electricity_development;
        private static Development electricity_development
        {
            set { _electricity_development.Add(value); }
        }
        public static List<Development> ElectricityDevelopment
        {
            get { return _electricity_development; }
        }

        //Development of labour
        private static List<Development> _labour_development;
        private static Development labour_development
        {
            set { _labour_development.Add(value); }
        }
        public static List<Development> LabourDevelopment
        {
            get { return _labour_development; }
        }

        //Development of vehicles
        private static List<Development> _vehicles_development;
        private static Development vehicles_development
        {
            set { _vehicles_development.Add(value); }
        }
        public static List<Development> VehiclesDevelopment
        {
            get { return _vehicles_development; }
        }

        public static void Start()
        {
            _electricity_development = new List<Development>();
            _labour_development = new List<Development>();
            _vehicles_development = new List<Development>();
            _electricity = 2;
            _labour = 2;
            _vehicles = 2;
            electricity_development = new Development(0, 0, Electricity);
            labour_development = new Development(0, 0, Labour);
            vehicles_development = new Development(0, 0, Vehicles);
        }

        /// <summary>
        /// Clamps integer between 0 and 4 (according to price range)
        /// </summary>
        /// <param name="value">Value to be clamped</param>
        /// <returns></returns>
        private static int Clamp(int value)
        {
            return (int)Mathf.Clamp(value, 0, 4);
        }

        public static void TrackDevelopment()
        {
            ProductionConsumptionRate pcrate = HUD.HUDProduction.TotalPCRate;
            electricity_development = new Development(pcrate.p_electricity, pcrate.c_electricity, Electricity);
            labour_development = new Development(pcrate.p_labour, pcrate.c_labour, Labour);
            vehicles_development = new Development(pcrate.p_vehicles, pcrate.c_vehicles, Vehicles);
            OnDevelopmentUpdate();
        }

        public delegate void DevelopmentUpdate();
        public static event DevelopmentUpdate OnDevelopmentUpdate;
    }
}
