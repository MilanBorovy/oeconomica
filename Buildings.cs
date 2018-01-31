using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class BuildingsAttribute : Attribute
{
    internal int branch; //Kind of business
    internal int grade; //Level of building
    internal ProductionConsumptionRate pcrate; //Production and consumption of building
    internal string name; //Name of building
    internal UnityEngine.Object model; //Building model

    /// <summary>
    /// Custom Buildings attribute
    /// </summary>
    /// <param name="branch">Kind of business</param>
    /// <param name="name">Level of building</param>
    /// <param name="pcrate">Production & consumption of building</param>
    /// <param name="action">Bonus action from building</param>
    /// <param name="charity">Charity from building</param>
    /// <param name="grade">Level of building</param>
    /// <param name="model">Building model (name)</param>
    internal BuildingsAttribute(int branch, string name, int[] pcrate, bool action, bool charity, int grade, string model)
    {
        this.branch = branch;
        this.grade = grade;
        this.pcrate = new ProductionConsumptionRate(pcrate, action, charity);
        this.name = name;
        this.model = Resources.Load("Buildings/" + model);
    }
}

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
    public static int GetBranch(this Buildings building)
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

    public static GameObject[] All()
    {
        return GameObject.FindGameObjectsWithTag("Building");
    }
}

public struct ProductionConsumptionRate
{
    //Production
    public int p_electricity { get; private set; }
    public int p_labour { get; private set; }
    public int p_vehicles { get; private set; }
    public int p_money { get; private set; }
    
    //Consumption
    public int c_electricity { get; private set; }
    public int c_labour { get; private set; }
    public int c_vehicles { get; private set; }
    public int c_money { get; private set; }
    
    //Bonus
    public bool action { get; private set; }
    public bool charity { get; private set; }

    /// <summary>
    /// Constructor. Set of produced and consumed commodities associated with buildings.
    /// 4 basic commodities are in form of int array.
    /// Commodities are in order electricity, labour, vehicles, money. First is production,
    /// second consumption.
    /// </summary>
    /// <param name="pc">Numeric expression of production&consumption rate of 4 basic commodities (Electricity, Labour, Vehicles, Money)</param>
    /// <param name="action">Whether is building giving extra action once per round</param>
    /// <param name="charity">Whether is building giving extra charity once per round</param>
    public ProductionConsumptionRate(int[] pc, bool action, bool charity)
    {
        this.p_electricity = pc[0];
        this.p_labour = pc[2];
        this.p_vehicles = pc[4];
        this.p_money = pc[6];
        this.c_electricity = pc[1];
        this.c_labour = pc[3];
        this.c_vehicles = pc[5];
        this.c_money = pc[7];
        this.action = action;
        this.charity = charity;
    }
}

public enum Buildings
{
    [Buildings(
        6,
        "Prázdná parcela",
        new int[8]
        {
            0, 0,
            0, 0,
            0, 0,
            0, 0
        },
        false,
        false,
        0,
        "empty")]
    EMPTY,

    [Buildings(
        0,
        "Solární elektrárna",
        new int[8]
        {
            1, 0,
            0, 0,
            0, 0,
            0, 2
        },
        false,
        false,
        1,
        "e_solar")]
    E_SOLAR
}

public class Odvetvi
{
    public int id { get; private set; }
    public string name { get; private set; }
    public List<Buildings> setOfBuildings { get; private set; }

    public Odvetvi(int id, string name, List<Buildings> setOfBuildings)
    {
        this.id = id;
        this.name = name;
        this.setOfBuildings = setOfBuildings;
    }
}

public class Budovy {
    /*
    public static Stavba s010 = new Stavba(0, "Tepelná elektrárna", new int[4, 2] { { 2, 0 }, { 0, 0 }, { 0, 0 }, { 0, 4 } }, 2, false, false);
    public static Stavba s020 = new Stavba(0, "Jaderná elektrárna", new int[4, 2] { { 3, 0 }, { 0, 0 }, { 0, 0 }, { 0, 6 } }, 3, false, false);
    public static Stavba s100 = new Stavba(1, "IT poradenství", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 4, 0 } }, 1, false, false);
    public static Stavba s110 = new Stavba(1, "Hosting", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 1 }, { 10, 0 } }, 2, false, false);
    public static Stavba s120 = new Stavba(1, "ISP", new int[4, 2] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 10, 0 } }, 3, false, false);
    public static Stavba s121 = new Stavba(1, "Vývojářské studio", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 6, 0 } }, 3, false, false);
    public static Stavba s200 = new Stavba(2, "Základní škola", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 0, 0 } }, 1, false, false);
    public static Stavba s210 = new Stavba(2, "SŠE", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 1, 0 } }, 2, false, false);
    public static Stavba s211 = new Stavba(2, "SPŠ", new int[4, 2] { { 0, 2 }, { 2, 0 }, { 0, 0 }, { 0, 0 } }, 2, false, false);
    public static Stavba s220 = new Stavba(2, "VŠE", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 0, 0 } }, 3, true, false);
    public static Stavba s221 = new Stavba(2, "VŠT", new int[4, 2] { { 0, 0 }, { 2, 0 }, { 0, 1 }, { 0, 0 } }, 3, false, false);
    public static Stavba s300 = new Stavba(3, "Ordinace", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 5, 0 } }, 1, false, false);
    public static Stavba s310 = new Stavba(3, "ZZS", new int[4, 2] { { 0, 1 }, { 0, 1 }, { 0, 0 }, { 9, 0 } }, 2, false, false);
    public static Stavba s311 = new Stavba(3, "Zdravotní středisko", new int[4, 2] { { 0, 0 }, { 0, 2 }, { 0, 0 }, { 10, 0 } }, 2, false, false);
    public static Stavba s320 = new Stavba(3, "LZS", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 0, 3 } }, 3, false, true);
    public static Stavba s321 = new Stavba(3, "Nemocnice", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 7, 0 } }, 3, false, false);
    public static Stavba s400 = new Stavba(4, "Výroba motocyklů", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 0, 0 } }, 1, false, false);
    public static Stavba s410 = new Stavba(4, "Automobilka", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 1, 0 }, { 0, 0 } }, 2, false, false);
    public static Stavba s411 = new Stavba(4, "Výroba autobusů", new int[4, 2] { { 0, 0 }, { 0, 2 }, { 2, 0 }, { 0, 0 } }, 2, false, false);
    public static Stavba s420 = new Stavba(4, "Luxusní automobilka", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 0, 0 } }, 3, true, false);
    public static Stavba s421 = new Stavba(4, "Výroba tahačů", new int[4, 2] { { 0, 1 }, { 0, 1 }, { 2, 0 }, { 0, 0 } }, 3, false, false);
    public static Stavba s500 = new Stavba(5, "Přepravní společnost", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 6, 0 } }, 1, false, false);
    public static Stavba s510 = new Stavba(5, "Taxi služba", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 2 }, { 12, 0 } }, 2, false, false);
    public static Stavba s511 = new Stavba(5, "Malá spediční společnost", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 1 }, { 11, 0 } }, 2, false, false);
    public static Stavba s520 = new Stavba(5, "MHD", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 4, 0 } }, 3, false, true);
    public static Stavba s521 = new Stavba(5, "Velká spediční společnost", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 9, 0 } }, 3, false, false);
    */
    public List<Odvetvi> odvetvi = new List<Odvetvi>(new Odvetvi[] { energy, empty });
    public static Odvetvi energy = new Odvetvi(0, "Energetika", new List<Buildings>(new Buildings[] { Buildings.E_SOLAR }));
    //public static Odvetvi o1 = new Odvetvi(1, "IT", new List<Stavba>(new Stavba[] { s100, s110, s120, s121 }));
    //public static Odvetvi o2 = new Odvetvi(2, "Školství", new List<Stavba>(new Stavba[] { s200, s210, s211, s220, s221 }));
    //public static Odvetvi o3 = new Odvetvi(3, "Zdravotnictví", new List<Stavba>(new Stavba[] { s300, s310, s311, s320, s321 }));
    //public static Odvetvi o4 = new Odvetvi(4, "Automobilka", new List<Stavba>(new Stavba[] { s400, s410, s411, s420, s421 }));
    //public static Odvetvi o5 = new Odvetvi(5, "Přeprava", new List<Stavba>(new Stavba[] { s500, s510, s511, s520, s521 }));
    public static Odvetvi empty = new Odvetvi(6, "Nic", new List<Buildings>(new Buildings[] { Buildings.EMPTY }));
}
