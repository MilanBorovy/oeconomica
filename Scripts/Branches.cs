using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oeconomica.Game.BuildingsNS;

namespace Oeconomica.Game.BranchesNS
{
    public enum Branches
    {
        [Branches(
            "Nic",
            Buildings.EMPTY)]
        NONE,
        [Branches(
            "Energetika",
            Buildings.E_SOLAR)]
        ENERGETICS
    }
}
