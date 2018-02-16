using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oeconomica.Game
{
    public enum Charity
    {

        //public static string[] level = new string[9] { "Nic", "Útulek", "Hřiště", "Knihovna", "MŠ", "Hospic", "Farmaceutický výzkum", "Humanitární pomoc", "Kosmický výzkum" };
        
        [String("Nic")]
        NONE = 0,
        [String("Útulek")]
        SHELTER = 1,
        [String("Hřiště")]
        PLAYGROUND = 2,
        [String("Knihovna")]
        LIBRARY = 3,
        [String("MŠ")]
        NURSERY = 4,
        [String("Hospic")]
        HOSPICE = 5,
        [String("Farmaceutický výzkum")]
        PHARMACY = 6,
        [String("Humanitární pomoc")]
        HUM_AID = 7,
        [String("Kosmický výzkum")]
        SPACE = 8
    }
}
