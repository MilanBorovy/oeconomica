using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hrac {

    public string name { get; private set; }
    public string firma { get; private set; }
    public int id { get; private set; }
    public int finance { get; private set; }
    public int charita { get; private set; }
    public int uver { get; private set; }
    public Color color { get; private set; }

    public Hrac(string jmeno, string jmenoFirmy, int ic, Color Color)
    {
        name = jmeno;
        firma = jmenoFirmy;
        id = ic;
        finance = 6;
        charita = 0;
        uver = 0;
        color = Color;
    }

    public Hrac(string jmeno, string jmenoFirmy, int ic)
    {
        name = jmeno;
        firma = jmenoFirmy;
        id = ic;
        finance = 6;
        charita = 0;
        uver = 0;
        color = new Color(Mathf.Clamp(Random.value, 0.5f, 1.0f), Mathf.Clamp(Random.value, 0.5f, 1.0f), Mathf.Clamp(Random.value, 0.5f, 1.0f));
    }

    public void Finance(int fin, bool vprospech)
    {
        if (!vprospech)
            fin *= -1;
        finance += fin;
    }

    public void Charita()
    {
        charita++;
    }

}
