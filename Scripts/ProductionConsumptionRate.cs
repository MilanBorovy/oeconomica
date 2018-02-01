using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oeconomica.Game.CommoditiesNS
{
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
}