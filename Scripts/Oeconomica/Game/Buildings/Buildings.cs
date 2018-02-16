using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
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
        E_SOLAR,

        [Buildings(
            Branches.ENERGETICS,
            "Uhelná elektrárna",
            new int[8]
            {
            2, 0,
            0, 0,
            0, 0,
            0, 4
            },
            false,
            false,
            2,
            "placeholder")]
        E_COAL,

        [Buildings(
            Branches.ENERGETICS,
            "Jaderná elektrárna",
            new int[8]
            {
            3, 0,
            0, 0,
            0, 0,
            0, 6
            },
            false,
            false,
            3,
            "placeholder")]
        E_NUCLEAR,

        [Buildings(
            Branches.IT,
            "IT servis",
            new int[8]
            {
            0, 1,
            0, 0,
            0, 0,
            4, 0
            },
            false,
            false,
            1,
            "placeholder")]
        I_SERVIS,

        [Buildings(
            Branches.IT,
            "Hosting",
            new int[8]
            {
            0, 1,
            0, 0,
            0, 1,
            10, 0
            },
            false,
            false,
            2,
            "placeholder")]
        I_HOSTING,

        [Buildings(
            Branches.IT,
            "ISP",
            new int[8]
            {
            0, 2,
            0, 0,
            0, 0,
            10, 0
            },
            false,
            false,
            3,
            "placeholder")]
        I_ISP,

        [Buildings(
            Branches.IT,
            "Vývojářské studio",
            new int[8]
            {
            0, 1,
            0, 0,
            0, 0,
            6, 0
            },
            false,
            false,
            3,
            "placeholder")]
        I_DEVELOP,

        [Buildings(
            Branches.EDUCATION,
            "Základní škola",
            new int[8]
            {
            0, 1,
            1, 0,
            0, 0,
            0, 0
            },
            false,
            false,
            1,
            "placeholder")]
        S_ELEMENTARY,

        [Buildings(
            Branches.EDUCATION,
            "SŠE",
            new int[8]
            {
            0, 1,
            1, 0,
            0, 0,
            1, 0
            },
            false,
            false,
            2,
            "placeholder")]
        S_HIGH_ECO,

        [Buildings(
            Branches.EDUCATION,
            "SPŠ",
            new int[8]
            {
            0, 2,
            2, 0,
            0, 0,
            0, 0
            },
            false,
            false,
            2,
            "placeholder")]
        S_HIGH_IND,

        [Buildings(
            Branches.EDUCATION,
            "VŠE",
            new int[8]
            {
            0, 1,
            1, 0,
            0, 0,
            0, 0
            },
            true,
            false,
            3,
            "placeholder")]
        S_UNI_ECO,

        [Buildings(
            Branches.EDUCATION,
            "VŠT",
            new int[8]
            {
            0, 0,
            2, 0,
            0, 1,
            0, 0
            },
            false,
            false,
            3,
            "placeholder")]
        S_UNI_IND,

        [Buildings(
            Branches.HEALTHCARE,
            "Ordinace",
            new int[8]
            {
            0, 0,
            0, 1,
            0, 0,
            5, 0
            },
            false,
            false,
            1,
            "placeholder")]
        H_SURGERY,

        [Buildings(
            Branches.HEALTHCARE,
            "ZZS",
            new int[8]
            {
            0, 1,
            0, 1,
            0, 0,
            9, 0
            },
            false,
            false,
            2,
            "placeholder")]
        H_AMBULANCE,

        [Buildings(
            Branches.HEALTHCARE,
            "Zdravotní středisko",
            new int[8]
            {
            0, 0,
            0, 2,
            0, 0,
            10, 0
            },
            false,
            false,
            2,
            "placeholder")]
        H_MED_CENTRE,

        [Buildings(
            Branches.HEALTHCARE,
            "LZS",
            new int[8]
            {
            0, 0,
            0, 1,
            0, 0,
            3, 0
            },
            false,
            true,
            3,
            "placeholder")]
        H_AIR_AMBULANCE,

        [Buildings(
            Branches.HEALTHCARE,
            "Nemocnice",
            new int[8]
            {
            0, 0,
            0, 1,
            0, 0,
            7, 0
            },
            false,
            false,
            3,
            "placeholder")]
        H_HOSPITAL,

        [Buildings(
            Branches.AUTOMOTIVE,
            "Výrobna motocyklů",
            new int[8]
            {
            0, 0,
            0, 1,
            1, 0,
            0, 0
            },
            false,
            false,
            1,
            "placeholder")]
        A_MOTORCYCLE,

        [Buildings(
            Branches.AUTOMOTIVE,
            "Automobilka",
            new int[8]
            {
            0, 1,
            0, 0,
            1, 0,
            0, 0
            },
            false,
            false,
            2,
            "placeholder")]
        A_CAR,

        [Buildings(
            Branches.AUTOMOTIVE,
            "Výrobna autobusů",
            new int[8]
            {
            0, 0,
            0, 2,
            2, 0,
            0, 0
            },
            false,
            false,
            2,
            "placeholder")]
        A_BUS,

        [Buildings(
            Branches.AUTOMOTIVE,
            "Luxusní automobilka",
            new int[8]
            {
            0, 0,
            0, 1,
            1, 0,
            0, 0
            },
            true,
            false,
            3,
            "placeholder")]
        A_LUXURY,

        [Buildings(
            Branches.AUTOMOTIVE,
            "Výrobna tahačů",
            new int[8]
            {
            0, 1,
            0, 1,
            2, 0,
            0, 0
            },
            false,
            false,
            3,
            "placeholder")]
        A_TRUCK,

        [Buildings(
            Branches.TRANSPORT,
            "Přepravní společnost",
            new int[8]
            {
            0, 0,
            0, 0,
            0, 1,
            6, 0
            },
            false,
            false,
            1,
            "placeholder")]
        T_TRANSPORT,

        [Buildings(
            Branches.TRANSPORT,
            "Taxi služba",
            new int[8]
            {
            0, 0,
            0, 0,
            0, 2,
            12, 0
            },
            false,
            false,
            2,
            "placeholder")]
        T_TAXI,

        [Buildings(
            Branches.TRANSPORT,
            "Malá spediční společnost",
            new int[8]
            {
            0, 0,
            0, 1,
            0, 1,
            11, 0
            },
            false,
            false,
            2,
            "placeholder")]
        T_SMALL_SHIPPING,

        [Buildings(
            Branches.TRANSPORT,
            "MHD",
            new int[8]
            {
            0, 0,
            0, 0,
            0, 1,
            4, 0
            },
            false,
            true,
            3,
            "placeholder")]
        T_PUBLIC,

        [Buildings(
            Branches.TRANSPORT,
            "Velká spediční společnost",
            new int[8]
            {
            0, 0,
            0, 0,
            0, 1,
            9, 0
            },
            false,
            false,
            3,
            "placeholder")]
        T_LARGE_SHIPPING
    }
}