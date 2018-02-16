using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Oeconomica.Game;

namespace Oeconomica
{
    public class Player : NetworkBehaviour
    {
        [SyncVar]private int _id;
        public int ID { get { return _id; } }

        [SyncVar]private string _name;
        public string Name { get { return _name; } }

        [SyncVar]private string _company;
        public string Company { get { return _company; } }

        [SyncVar(hook = "OnMoneyChanged")]private int _money;
        public int Money
        {
            get { return _money; }
            set { _money += value; Debug.Log("Money of player " + Name + " has been changed by " + value + " to " + Money); }
        }

        [SyncVar(hook = "OnCharityChanged")]private int _charity;
        public Charity Charity
        {
            get { return (Charity)_charity; }
            set { _charity = _charity + (int)value; Debug.Log("Charity level of player " + Name + " has been changed to " + Charity.GetString()); }
        }

        [SyncVar]private int _loan;
        public int Loan
        {
            get { return _loan; }
            set { _loan += value; Debug.Log("Money of player " + Name + " has been changed by " + value + " to " + Loan); }
        }

        [SyncVar]private Color _color;
        public Color Color
        {
            get { return _color; }
        }

        public Player(string name, string company, int id, Color color)
        {
            _id = id;
            _name = name;
            _company = company;
            _money = 6;
            _charity = 0;
            _loan = 0;
            _color = color;
        }

        public void Apply(Player player)
        {
            this._id = player._id;
            this._name = player._name;
            this._company = player._company;
            this._money = player._money;
            this._charity = player._charity;
            this._loan = player._loan;
            this._color = player._color;
        }

        private void OnMoneyChanged(int value)
        {
            this._money = value;
            if(GameLogic.HasTurn != null)
                GameLogic.ShowPlayerInfo();
        }

        private void OnCharityChanged(int value)
        {
            this._charity = value;
            if (GameLogic.HasTurn != null)
                GameLogic.ShowPlayerInfo();
        }
    }
}
