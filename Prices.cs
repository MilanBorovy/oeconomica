using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prices {

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

    //Development of price of electricity
    private static List<int> _electricity_development;
    private static int electricity_development
    {
        set { _electricity_development.Add(value); }
    }
    public static List<int> ElectricityDevelopment
    {
        get { return _electricity_development; }
    }

    //Development of price of labour
    private static List<int> _labour_development;
    private static int labour_development
    {
        set { _labour_development.Add(value); }
    }
    public static List<int> LabourDevelopment
    {
        get { return _labour_development; }
    }

    //Development of price of vehicles
    private static List<int> _vehicles_development;
    private static int vehicles_development
    {
        set { _vehicles_development.Add(value); }
    }
    public static List<int> VehiclesDevelopment
    {
        get { return _vehicles_development; }
    }

    public static void Start()
    {
        _electricity_development = new List<int>();
        _labour_development = new List<int>();
        _vehicles_development = new List<int>();
        _electricity = 2;
        _labour = 2;
        _vehicles = 2;
        electricity_development = Electricity;
        labour_development = Labour;
        vehicles_development = Vehicles;
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
        electricity_development = Electricity;
        labour_development = Labour;
        vehicles_development = Vehicles;
    }
}
