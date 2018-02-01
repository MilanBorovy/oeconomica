using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oeconomica.Game.BuildingsNS;

namespace Oeconomica.Game.BranchesNS
{
    public static class BranchesExtensions
    {
        /// <summary>
        /// Gets DisplayName of branch
        /// </summary>
        public static string GetName(this Branches branch)
        {
            return branch.GetAttribute<BranchesAttribute>().name;
        }
        /// <summary>
        /// Gets available buildings for branch
        /// </summary>
        public static List<Buildings> GetBuildings(this Branches branch)
        {
            return branch.GetAttribute<BranchesAttribute>().buildings;
        }
        /// <summary>
        /// Returns all branches available excluding NONE
        /// </summary>
        public static List<Branches> AllBranches()
        {
            List<Branches> branches = ((Branches[])Enum.GetValues(typeof(Branches))).ToList<Branches>();
            branches.Remove(Branches.NONE);
            return branches;
        }
    }
}   