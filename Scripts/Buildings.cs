using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Oeconomica.Game.BranchesNS;

namespace Oeconomica.Game.BuildingsNS
{
    public enum Buildings
    {
        [Buildings(
            Branches.NONE,
            "Prázdná parcela",
            new int[8]
            {
            0, 0,
            0, 0,
            0, 0,
            0, 0
            },
            false,
            false,
            0,
            "empty")]
        EMPTY,

        [Buildings(
            Branches.ENERGETICS,
            "Solární elektrárna",
            new int[8]
            {
            1, 0,
            0, 0,
            0, 0,
            0, 2
            },
            false,
            false,
            1,
            "e_solar")]
        E_SOLAR
    }
}

    /*
    (0, "Uhelná elektrárna", new int[4, 2] { { 2, 0 }, { 0, 0 }, { 0, 0 }, { 0, 4 } }, 2, false, false);
    (0, "Jaderná elektrárna", new int[4, 2] { { 3, 0 }, { 0, 0 }, { 0, 0 }, { 0, 6 } }, 3, false, false);
    (1, "IT poradenství", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 4, 0 } }, 1, false, false);
    (1, "Hosting", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 1 }, { 10, 0 } }, 2, false, false);
    (1, "ISP", new int[4, 2] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 10, 0 } }, 3, false, false);
    (1, "Vývojářské studio", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 6, 0 } }, 3, false, false);
    (2, "Základní škola", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 0, 0 } }, 1, false, false);
    (2, "SŠE", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 1, 0 } }, 2, false, false);
    (2, "SPŠ", new int[4, 2] { { 0, 2 }, { 2, 0 }, { 0, 0 }, { 0, 0 } }, 2, false, false);
    (2, "VŠE", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 0, 0 } }, 3, true, false);
    (2, "VŠT", new int[4, 2] { { 0, 0 }, { 2, 0 }, { 0, 1 }, { 0, 0 } }, 3, false, false);
    (3, "Ordinace", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 5, 0 } }, 1, false, false);
    (3, "ZZS", new int[4, 2] { { 0, 1 }, { 0, 1 }, { 0, 0 }, { 9, 0 } }, 2, false, false);
    (3, "Zdravotní středisko", new int[4, 2] { { 0, 0 }, { 0, 2 }, { 0, 0 }, { 10, 0 } }, 2, false, false);
    (3, "LZS", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 0, 3 } }, 3, false, true);
    (3, "Nemocnice", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 7, 0 } }, 3, false, false);
    (4, "Výroba motocyklů", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 0, 0 } }, 1, false, false);
    (4, "Automobilka", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 1, 0 }, { 0, 0 } }, 2, false, false);
    (4, "Výroba autobusů", new int[4, 2] { { 0, 0 }, { 0, 2 }, { 2, 0 }, { 0, 0 } }, 2, false, false);
    (4, "Luxusní automobilka", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 0, 0 } }, 3, true, false);
    (4, "Výroba tahačů", new int[4, 2] { { 0, 1 }, { 0, 1 }, { 2, 0 }, { 0, 0 } }, 3, false, false);
    (5, "Přepravní společnost", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 6, 0 } }, 1, false, false);
    (5, "Taxi služba", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 2 }, { 12, 0 } }, 2, false, false);
    (5, "Malá spediční společnost", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 1 }, { 11, 0 } }, 2, false, false);
    (5, "MHD", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 4, 0 } }, 3, false, true);
    (5, "Velká spediční společnost", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 9, 0 } }, 3, false, false);
    */
