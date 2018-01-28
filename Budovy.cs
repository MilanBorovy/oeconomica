using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stavba
{
    public int odvetvi { get; private set; }
    public string nazev { get; private set; }
    public int[,] prodCons { get; private set; }
    public int uroven { get; private set; }
    public bool akceNavic { get; private set; }
    public bool charita { get; private set; }

    public Stavba(int odv, string naz, int[,] pro, int uro, bool akc, bool cha)
    {
        odvetvi = odv;
        nazev = naz;
        prodCons = pro;
        uroven = uro;
        akceNavic = akc;
        charita = cha;
    }
}

public class Odvetvi
{
    public int id { get; private set; }
    public string nazev { get; private set; }
    public List<Stavba> platneStavby { get; private set; }

    public Odvetvi(int idn, string naz, List<Stavba> pla)
    {
        id = idn;
        nazev = naz;
        platneStavby = pla;
    }
}

public class Budovy {

    public static Stavba sX = new Stavba(6, "Prázdná parcela", new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } }, 0, false, false);
    public static Stavba s000 = new Stavba(0, "Solární elktrárna", new int[4, 2] { { 1, 0 }, { 0, 0 }, { 0, 0 }, { 0, 2 } }, 1, false, false);
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
    public List<Odvetvi> odvetvi = new List<Odvetvi>(new Odvetvi[] { o0, o1, o2, o3, o4, o5, oX });
    public static Odvetvi o0 = new Odvetvi(0, "Energetika", new List<Stavba>(new Stavba[] { s000, s010, s020 }));
    public static Odvetvi o1 = new Odvetvi(1, "IT", new List<Stavba>(new Stavba[] { s100, s110, s120, s121 }));
    public static Odvetvi o2 = new Odvetvi(2, "Školství", new List<Stavba>(new Stavba[] { s200, s210, s211, s220, s221 }));
    public static Odvetvi o3 = new Odvetvi(3, "Zdravotnictví", new List<Stavba>(new Stavba[] { s300, s310, s311, s320, s321 }));
    public static Odvetvi o4 = new Odvetvi(4, "Automobilka", new List<Stavba>(new Stavba[] { s400, s410, s411, s420, s421 }));
    public static Odvetvi o5 = new Odvetvi(5, "Přeprava", new List<Stavba>(new Stavba[] { s500, s510, s511, s520, s521 }));
    public static Odvetvi oX = new Odvetvi(6, "Nic", new List<Stavba>(new Stavba[] { sX }));
}
