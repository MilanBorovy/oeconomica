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

public class Budovy {
    /*
    public static Stavba s010 = new Stavba(0, "Tepelná elektrárna", new int[4, 2] { { 2, 0 }, { 0, 0 }, { 0, 0 }, { 0, 4 } }, 2, false, false);
    public static Stavba s020 = new Stavba(0, "Jaderná elektrárna", new int[4, 2] { { 3, 0 }, { 0, 0 }, { 0, 0 }, { 0, 6 } }, 3, false, false);
    public static Stavba s100 = new Stavba(1, "IT poradenství", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 4, 0 } }, 1, false, false);
    public static Stavba s110 = new Stavba(1, "Hosting", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 1 }, { 10, 0 } }, 2, false, false);
    public static Stavba s120 = new Stavba(1, "ISP", new int[4, 2] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 10, 0 } }, 3, false, false);
    public static Stavba s121 = new Stavba(1, "Vývojářské studio", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 0, 0 }, { 6, 0 } }, 3, false, false);
    public static Stavba s200 = new Stavba(2, "Základní škola", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 0, 0 } }, 1, false, false);
    public static Stavba s210 = new Stavba(2, "SŠE", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 1, 0 } }, 2, false, false);
    public static Stavba s211 = new Stavba(2, "SPŠ", new int[4, 2] { { 0, 2 }, { 2, 0 }, { 0, 0 }, { 0, 0 } }, 2, false, false);
    public static Stavba s220 = new Stavba(2, "VŠE", new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, 0 }, { 0, 0 } }, 3, true, false);
    public static Stavba s221 = new Stavba(2, "VŠT", new int[4, 2] { { 0, 0 }, { 2, 0 }, { 0, 1 }, { 0, 0 } }, 3, false, false);
    public static Stavba s300 = new Stavba(3, "Ordinace", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 5, 0 } }, 1, false, false);
    public static Stavba s310 = new Stavba(3, "ZZS", new int[4, 2] { { 0, 1 }, { 0, 1 }, { 0, 0 }, { 9, 0 } }, 2, false, false);
    public static Stavba s311 = new Stavba(3, "Zdravotní středisko", new int[4, 2] { { 0, 0 }, { 0, 2 }, { 0, 0 }, { 10, 0 } }, 2, false, false);
    public static Stavba s320 = new Stavba(3, "LZS", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 0, 3 } }, 3, false, true);
    public static Stavba s321 = new Stavba(3, "Nemocnice", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 0 }, { 7, 0 } }, 3, false, false);
    public static Stavba s400 = new Stavba(4, "Výroba motocyklů", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 0, 0 } }, 1, false, false);
    public static Stavba s410 = new Stavba(4, "Automobilka", new int[4, 2] { { 0, 1 }, { 0, 0 }, { 1, 0 }, { 0, 0 } }, 2, false, false);
    public static Stavba s411 = new Stavba(4, "Výroba autobusů", new int[4, 2] { { 0, 0 }, { 0, 2 }, { 2, 0 }, { 0, 0 } }, 2, false, false);
    public static Stavba s420 = new Stavba(4, "Luxusní automobilka", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 0, 0 } }, 3, true, false);
    public static Stavba s421 = new Stavba(4, "Výroba tahačů", new int[4, 2] { { 0, 1 }, { 0, 1 }, { 2, 0 }, { 0, 0 } }, 3, false, false);
    public static Stavba s500 = new Stavba(5, "Přepravní společnost", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 6, 0 } }, 1, false, false);
    public static Stavba s510 = new Stavba(5, "Taxi služba", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 2 }, { 12, 0 } }, 2, false, false);
    public static Stavba s511 = new Stavba(5, "Malá spediční společnost", new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 1 }, { 11, 0 } }, 2, false, false);
    public static Stavba s520 = new Stavba(5, "MHD", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 4, 0 } }, 3, false, true);
    public static Stavba s521 = new Stavba(5, "Velká spediční společnost", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 1 }, { 9, 0 } }, 3, false, false);
    */
}
