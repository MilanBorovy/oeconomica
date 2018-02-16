using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oeconomica.Game.CommoditiesNS;
using Oeconomica.Game.BuildingsNS;
using Oeconomica.Game.HUD;
using UnityEngine.Networking;
using System;
using System.Xml.Serialization;
using System.IO;

namespace Oeconomica.Game
{
    public class GameLogic : NetworkBehaviour
    {
        public static List<Player> players { get; private set; } //List of players
        public static GameLogic executive_logic;
        private static GameObject[] player_objects;
        public GameObject PlayerPrefab;

        //Actual round
        [SyncVar(hook = "OnNewRound")] public int round;

        //Player who has turn right now
        [SyncVar(hook = "OnNewTurn")] private int _has_turn_id;
        private static Player _has_turn;
        public static Player HasTurn
        {
            get
            {
                return _has_turn;
            }
            private set
            {
                executive_logic._has_turn_id = value.ID;
                _has_turn = value;
            }
        }

        public static int actions { get; private set; } //Available actions

        private const float timeLimit = 120f; //Time limit & elapsed time of turn

        [SyncVar(hook = "OnTimeChange")] private float time;

        private GameObject[] buildings;

        private void Start()
        {
            Debug.Log("Game started");

            executive_logic = this;

            //Load Players
            players = PlayerSetup.players;
            GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>().CommitSuicide();

            player_objects = GameObject.FindGameObjectsWithTag("Player");
            SortAlphabetically(player_objects, 0, player_objects.Length);

            //Add missing players
            //for (int i = players.Count; i < 4; i++)
            //    players.Add(new Player("Hráč " + (i + 1), "Společnost hráče " + (i + 1), players.Count, new Color(1.0f, 0.6f, 0.6f)));

            for(int i = 0; i < players.Count; i++)
            {
                player_objects[i].GetComponent<Player>().Apply(players[0]);
                players.RemoveAt(0);
                players.Add(player_objects[i].GetComponent<Player>());
            }

            if (isServer)
                foreach (NetworkConnection conn in NetworkServer.connections) 
                {
                    GameObject player_inst = Instantiate(PlayerPrefab);
                    NetworkServer.AddPlayerForConnection(conn, player_inst, (short)conn.connectionId);
                }
            
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
            
            //Give buildings to players
            buildings = GameObject.FindGameObjectsWithTag("Building");
            foreach (GameObject building in buildings)
            {
                building.GetComponent<Building>().SetOwner(
                        players[
                            int.Parse(
                                building.transform.parent.name[
                                    building.transform.parent.name.Length - 1]
                                    .ToString())]);
            }

            //Set round & turn
            round = 1;
            OnNewRound(round);
            if (isServer)
                HasTurn = players[0];
            else
                HasTurn = players[_has_turn_id];
            actions = 1;

            ShowPlayerInfo();
        }

        private void Update()
        {
            if (!isServer)
                return;
            float temp = time;
            time = temp + Time.deltaTime;
            if (time >= timeLimit)
                NextTurn();
        }

        private void OnTimeChange(float val)
        {
            GameObject.Find("TimerForeground").GetComponent<Image>().fillAmount = (timeLimit - val) / timeLimit;
        }

        /// <summary>
        /// Shows information about player who has turn
        /// </summary>
        public static void ShowPlayerInfo()
        {
            NetworkIdentity identity = HasTurn.gameObject.GetComponent<NetworkIdentity>();

            bool local = identity.hasAuthority || (identity.isServer && identity.clientAuthorityOwner == null);

            GameObject.Find("NextTurnButton").GetComponent<Button>().interactable = local;

            for (int i = 1; i < 9; i++)
            {
                GameObject.Find("CharityStep" + i).GetComponent<Toggle>().isOn = (int)HasTurn.Charity >= i;
            }

            GameObject.Find("CharityAdd").GetComponent<Button>().interactable = 
                (HasTurn.Money >= 6 && (int)HasTurn.Charity < 8 && actions > 0) && local;

            GameObject.Find("PlayerName").GetComponent<Text>().text = HasTurn.Name;
            GameObject.Find("PlayerCompany").GetComponent<Text>().text = HasTurn.Company;
            GameObject.Find("PlayerCapital").GetComponent<Text>().text = string.Format("{0},000,000 Kč", HasTurn.Money);
            GameObject.Find("PlayerCharity").GetComponent<Text>().text = HasTurn.Charity.GetString();
        }


        /// <summary>
        /// Adds charity to player who has turn actually
        /// </summary>
        public void AddCharity()
        {
            HasTurn.Money = -6;
            HasTurn.Charity = (Charity)1;
            Action();
            ShowPlayerInfo();
        }
        
        /// <summary>
        /// End turn and start new one
        /// </summary>
        public void NextTurn()
        {
            //Server-only method
            if (!isServer)
                return;

            if (HasTurn.ID + 1 >= players.Count)
                NextRound();
            else
                HasTurn = players[HasTurn.ID + 1];

            actions = 1;
            foreach (GameObject building in buildings)
            {
                Building buildingLogic = building.GetComponent<Building>();
                if (buildingLogic.Owner == HasTurn &&
                    buildingLogic.ActualBuilding.GetPCRate().action)
                    actions++;
            }

            time = 0;

            Debug.Log("New turn. " + HasTurn.Name + " has turn.");
        }
        
        /// <summary>
        /// Renew displayed information for new Turn
        /// </summary>
        private void OnNewTurn(int player_id)
        {
            HasTurn = players[player_id];

            actions = 1;
            foreach (GameObject building in buildings)
            {
                Building buildingLogic = building.GetComponent<Building>();
                if (buildingLogic.Owner == HasTurn &&
                    buildingLogic.ActualBuilding.GetPCRate().action)
                    actions++;
            }

            ShowPlayerInfo();
        }

        private void OnNewRound(int value)
        {
            this.round = value;
            GameObject.Find("RoundText").GetComponent<Text>().text = String.Format("Kolo: {0}", value);
        }

        /// <summary>
        /// End round and prepare for new one
        /// </summary>
        public void NextRound()
        {
            if (!isServer)
                return;
            round++;
            PAYDAY();
            NewPrices();

            HasTurn = players[0];

            Debug.Log("Start of round " + round);
        }

        /// <summary>
        /// Pays money to player according to the prices and their buildings
        /// </summary>
        private void PAYDAY()
        {
            if (!isServer)
                return;
            foreach (Player p in players)
            {
                int profit = 0;
                foreach (GameObject b in buildings)
                {
                    if (b.GetComponent<Building>().Owner == p)
                    {
                        ProductionConsumptionRate pcrate = (b.GetComponent<Building>() as Building).ActualBuilding.GetPCRate();

                        profit += (pcrate.p_electricity - pcrate.c_electricity) * Prices.Electricity;
                        profit += (pcrate.p_labour - pcrate.c_labour) * Prices.Labour;
                        profit += (pcrate.p_vehicles - pcrate.c_vehicles) * Prices.Vehicles;
                        profit += (pcrate.p_money - pcrate.c_money);
                        if (pcrate.charity)
                            p.Charity = (Charity)1;
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
            if (!isServer)
                return;
            ProductionConsumptionRate total_pcrate = HUDProduction.TotalPCRate;
            int electricity = (int)Mathf.Clamp(total_pcrate.c_electricity - total_pcrate.p_electricity, -1, 1);
            int labour = (int)Mathf.Clamp(total_pcrate.c_labour - total_pcrate.p_labour, -1, 1);
            int vehicles = (int)Mathf.Clamp(total_pcrate.c_vehicles - total_pcrate.p_vehicles, -1, 1);
            RpcNewPrices(electricity, labour, vehicles, true);
            
        }
        
        /// <summary>
        /// Prices synchronization
        /// </summary>
        /// <param name="track">Track development?</param>
        [ClientRpc]
        private void RpcNewPrices(int electricity, int labour, int vehicles, bool track)
        {
            Prices.Electricity = electricity;
            Prices.Labour = labour;
            Prices.Vehicles = vehicles;
            if (track)
                Prices.TrackDevelopment();
        }

        /// <summary>
        /// Price synchronization for newcoming players
        /// </summary>
        public void PricesSync()
        {
            if (!isServer)
                return;
            RpcNewPrices(Prices.Electricity, Prices.Labour, Prices.Vehicles, false);
        }

        /// <summary>
        /// Removes 1 action
        /// </summary>
        public static void Action() { actions--; }

        /// <summary>
        /// Quicksort for sorting gameobjects alphabetically
        /// </summary>
        private static void SortAlphabetically(GameObject[] arr, int left, int right)
        {
            if(left < right)
            {
                int boundary = left;

                for(int i = left + 1; i < right; i++)
                {
                    string refname = arr[left].name.ToLower();
                    string name = arr[i].name.ToLower();

                    int j;
                    for (j = 0;
                        refname[j] == name[j] &&
                        j < refname.Length - 1 &&
                        j < name.Length - 1;
                        j++) ;
                    if((int)refname[j] > (int)name[j] || (refname[j] == name[j] && refname.Length > name.Length))
                    {
                        SwapObjects(arr, ++boundary, i);
                    }
                }

                SwapObjects(arr, left, boundary);
                SortAlphabetically(arr, left, boundary);
                SortAlphabetically(arr, boundary + 1, right);
            }
        }

        /// <summary>
        /// Swap 2 gameobjects in array
        /// </summary>
        private static void SwapObjects(GameObject[] arr, int a, int b)
        {
            GameObject temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }
    }
}
