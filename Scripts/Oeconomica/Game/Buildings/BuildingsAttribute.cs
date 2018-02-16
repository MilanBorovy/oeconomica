using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Oeconomica.Game.CommoditiesNS;
using Oeconomica.Game.BranchesNS;

namespace Oeconomica.Game.BuildingsNS
{
    public class BuildingsAttribute : Attribute
    {
        internal Branches branch; //Kind of business
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
        internal BuildingsAttribute(Branches branch, string name, int[] pcrate, bool action, bool charity, int grade, string model)
        {
            this.branch = branch;
            this.grade = grade;
            this.pcrate = new ProductionConsumptionRate(pcrate, action, charity);
            this.name = name;
            this.model = Resources.Load("Buildings/" + model);
        }
    }
}
