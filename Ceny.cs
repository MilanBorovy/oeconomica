using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ceny {

    public static int elektrina { get; private set; }

    public static int prsila { get; private set; }

    public static int auta { get; private set; }

    public static List<int> eVyvoj { get; private set; }
    public static List<int> lVyvoj { get; private set; }
    public static List<int> tVyvoj { get; private set; }

    public static void Start()
    {
        eVyvoj = new List<int>();
        lVyvoj = new List<int>();
        tVyvoj = new List<int>();
        elektrina = 3;
        prsila = 4;
        auta = 5;
        eVyvoj.Add(elektrina);
        lVyvoj.Add(prsila);
        tVyvoj.Add(auta);
    }

    public static void zvysitCenu(char zdroj, int hodnota)
    {
        switch(zdroj)
        {
            case 'e':
                elektrina = (int)Mathf.Clamp(elektrina + hodnota, 1, 5);
                eVyvoj.Add(elektrina);
                break;
            case 'l':
                prsila = (int)Mathf.Clamp(prsila + hodnota, 2, 6);
                lVyvoj.Add(prsila);
                break;
            case 't':
                auta = (int)Mathf.Clamp(auta + hodnota, 3, 7);
                tVyvoj.Add(auta);
                break;
            default:
                break;
        }
    }

    public static void snizitCenu(char zdroj, int hodnota)
    {
        switch (zdroj)
        {
            case 'e':
                elektrina = (int)Mathf.Clamp(elektrina - hodnota, 1, 5);
                eVyvoj.Add(elektrina);
                break;
            case 'l':
                prsila = (int)Mathf.Clamp(prsila - hodnota, 2, 6);
                lVyvoj.Add(prsila);
                break;
            case 't':
                auta = (int)Mathf.Clamp(auta - hodnota, 3, 7);
                tVyvoj.Add(auta);
                break;
            default:
                break;
        }
    }
}
