using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Oeconomica.Game.BuildingsNS;

namespace Oeconomica.Game.BranchesNS
{
    public class BranchesAttribute : Attribute
    {
        internal string name; //Name of branch
        internal List<Buildings> buildings; //Available buildings from that branch

        /// <summary>
        /// Custom Branch attributes
        /// </summary>
        /// <param name="name">DisplayName of branch</param>
        /// <param name="buildings">List of available buildings</param>
        internal BranchesAttribute(string name, params Buildings[] buildings)
        {
            this.name = name;
            this.buildings = new List<Buildings>();
            foreach (Buildings b in buildings)
                this.buildings.Add(b);
        }
    }
}