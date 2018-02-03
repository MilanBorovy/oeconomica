using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oeconomica
{
    public class Player
    {
        private int _id;
        public int ID { get { return _id; } }

        private string _name;
        public string Name { get { return _name; } }

        private string _company;
        public string Company { get { return _company; } }

        private int _money;
        public int Money
        {
            get { return _money; }
            set { _money += value; }
        }

        private int _charity;
        public int Charity
        {
            get { return _charity; }
            set { _charity += value; }
        }

        private int _loan;
        public int Loan
        {
            get { return _loan; }
            set { _loan += value; }
        }

        private Color _color;
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
    }
}
