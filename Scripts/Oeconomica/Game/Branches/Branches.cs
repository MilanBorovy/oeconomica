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
            Buildings.E_COAL,
            Buildings.E_NUCLEAR,
            Buildings.E_SOLAR)]
        ENERGETICS,

        [Branches(
            "IT",
            Buildings.I_DEVELOP,
            Buildings.I_HOSTING,
            Buildings.I_ISP,
            Buildings.I_SERVIS)]
        IT,

        [Branches(
            "Školství",
            Buildings.S_ELEMENTARY,
            Buildings.S_HIGH_ECO,
            Buildings.S_HIGH_IND,
            Buildings.S_UNI_ECO,
            Buildings.S_UNI_IND)]
        EDUCATION,

        [Branches(
            "Zdravotnictví",
            Buildings.H_AIR_AMBULANCE,
            Buildings.H_AMBULANCE,
            Buildings.H_HOSPITAL,
            Buildings.H_MED_CENTRE,
            Buildings.H_SURGERY)]
        HEALTHCARE,

        [Branches(
            "Autoprůmysl",
            Buildings.A_BUS,
            Buildings.A_CAR,
            Buildings.A_LUXURY,
            Buildings.A_MOTORCYCLE,
            Buildings.A_TRUCK)]
        AUTOMOTIVE,

        [Branches(
            "Autodoprava",
            Buildings.T_LARGE_SHIPPING,
            Buildings.T_PUBLIC,
            Buildings.T_SMALL_SHIPPING,
            Buildings.T_TAXI,
            Buildings.T_TRANSPORT)]
        TRANSPORT
    }
}
