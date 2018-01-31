using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public static List<Player> players { get; private set; } //List of players
    public static int round { get; private set; } //Actual round
    public static Player HasTurn { get; private set; } //Player who has turn right now
    public static int actions { get; private set; } //Available actions
    private float timeLimit, time; //Time limit & elapsed time of turn

    private void Start ()
    {
        //Init players
        players = new List<Player>();
        players.Add(new Player("Hráč 1", "Firma s.r.o.", players.Count));
        players.Add(new Player("Hráč 2", "Firma a.s.", players.Count));
        players.Add(new Player("Hráč 3", "Firma v.o.s.", players.Count));
        players.Add(new Player("Hráč 4", "Firma k.s.", players.Count));

        //Init player signs
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject.Find("Signs").transform
                    .Find("Player" + i)
                    .Find(j == 0 ? "Front" : "Back")
                    .Find("Background")
                    .Find("Name")
                    .GetComponent<Text>().text = players[i].Name;
                GameObject.Find("Signs").transform
                    .Find("Player" + i)
                    .Find(j == 0 ? "Front" : "Back")
                    .Find("Background")
                    .Find("Company")
                    .GetComponent<Text>().text = players[i].Company;
                GameObject.Find("Signs").transform
                    .Find("Player" + i)
                    .Find(j == 0 ? "Front" : "Back")
                    .Find("Background")
                    .GetComponent<Image>().color = players[i].Color;
            }
        }

        //Set turn timer
        timeLimit = 120f;
        time = 0f;

        //Set round & turn
        round = 0;
        HasTurn = players[0];
        actions = 1;

        ShowPlayerInfo();
    }

    private void Update()
    {
        time += Time.deltaTime;
        GameObject.Find("TimerForeground").GetComponent<Image>().fillAmount = (timeLimit - time) / timeLimit;
        if (time >= timeLimit)
            NextTurn();
    }

    /// <summary>
    /// Shows information about player who has turn
    /// </summary>
    public static void ShowPlayerInfo()
    {
        for (int i = 1; i < 9; i++)
        {
            GameObject.Find("CharityStep" + i).GetComponent<Toggle>().isOn = HasTurn.Charity >= i;
        }

        GameObject.Find("CharityAdd").GetComponent<Button>().interactable = HasTurn.Money >= 6 && HasTurn.Charity < 8;

        GameObject.Find("PlayerName").GetComponent<Text>().text = HasTurn.Name;
        GameObject.Find("PlayerCompany").GetComponent<Text>().text = HasTurn.Company;
        GameObject.Find("PlayerCapital").GetComponent<Text>().text = string.Format("{0},000,000 Kč", HasTurn.Money);
        GameObject.Find("PlayerCharity").GetComponent<Text>().text = Charity.level[HasTurn.Charity];
    }


    /// <summary>
    /// Adds charity to player who has turn actually
    /// </summary>
    public void AddCharity()
    {
        HasTurn.Money = -6;
        HasTurn.Charity = 1;
        actions--;
        ShowPlayerInfo();
    }

    /// <summary>
    /// End turn and start new one
    /// </summary>
    public void NextTurn()
    {
        actions = 1;
        if (HasTurn.ID + 1 >= players.Count )
            NextRound();
        else
            HasTurn = players[HasTurn.ID + 1];
        ShowPlayerInfo();
        time = 0;
    }

    /// <summary>
    /// End round and prepare for new one
    /// </summary>
    public void NextRound()
    {
        HasTurn = players[0];
        round++;
        PAYDAY();
        NewPrices();
    }

    /// <summary>
    /// Pays money to player according to the prices and their buildings
    /// </summary>
    private void PAYDAY()
    {
        foreach (Player p in players)
        {
            int profit = 0;
            foreach(GameObject b in GameObject.FindGameObjectsWithTag("Building"))
            {
                if(b.transform.IsChildOf(GameObject.Find("Buildings").transform.Find("Player" + p.ID)))
                {
                    ProductionConsumptionRate pcrate = BuildingsExtensions.GetPCRate((b.GetComponent<Building>() as Building).ActualBuilding);

                    profit += (pcrate.p_electricity - pcrate.c_electricity) * Prices.Electricity;
                    profit += (pcrate.p_labour - pcrate.c_labour) * Prices.Labour;
                    profit += (pcrate.p_vehicles - pcrate.c_vehicles) * Prices.Vehicles;
                    profit += (pcrate.p_money - pcrate.c_money);
                }
            }
            p.Money = profit;
        }
    }

    /// <summary>
    /// Sets new prices according to production and consumption rates
    /// </summary>
    private void NewPrices()
    {
        ProductionConsumptionRate total_pcrate = HUDProduction.TotalPCRate;
        Prices.Electricity = (int)Mathf.Clamp(total_pcrate.c_electricity - total_pcrate.p_electricity, -1, 1);
        Prices.Labour = (int)Mathf.Clamp(total_pcrate.c_labour - total_pcrate.p_labour, -1, 1);
        Prices.Vehicles = (int)Mathf.Clamp(total_pcrate.c_vehicles - total_pcrate.p_vehicles, -1, 1);
        Prices.TrackDevelopment();
    }

    /// <summary>
    /// Removes 1 action
    /// </summary>
    public static void Action() { actions--; }
}
