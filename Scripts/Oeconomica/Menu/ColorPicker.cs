using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Oeconomica.Menu
{
    class ColorPicker : NetworkBehaviour
    {
        private List<Color> availableColors;
        public int startingColorIndex;
        [SyncVar (hook = "OnColorChanged")]private int selectedColorIndex;

        public Color SelectedColor
        {
            get
            {
                return availableColors[selectedColorIndex];
            }
        }

        private void Start()
        {
            availableColors = new List<Color>();
            for (float r = 0.5f; r < 1.5f; r += 0.5f)
                for (float g = 0.5f; g < 1.5f; g += 0.5f)
                    for (float b = 0.5f; b < 1.5f; b += 0.5f)
                        availableColors.Add(new Color(r, g, b));
            selectedColorIndex = startingColorIndex;
            if (!NetworkServer.active)
                OnColorChanged(startingColorIndex);
        }

        public void NextColor()
        {
            if (NetworkServer.active)
                CmdNextColor();
            else
                SrvNextColor();
        }

        [Command]
        private void CmdNextColor()
        {
            SrvNextColor();
        }

        private void SrvNextColor()
        {
            int index = selectedColorIndex;
            if (++index >= availableColors.Count)
                index = 0;
            selectedColorIndex = index;
            if(!NetworkServer.active)
                OnColorChanged(index);
        }

        private void OnColorChanged(int value)
        {
            selectedColorIndex = value;

            ColorBlock colors = gameObject.GetComponent<Button>().colors;
            colors.normalColor = SelectedColor;
            colors.highlightedColor = SelectedColor * new Color(1.1f, 1.1f, 1.1f);
            colors.pressedColor = SelectedColor * new Color(0.9f, 0.9f, 0.9f);
            gameObject.GetComponent<Button>().colors = colors;
        }
    }
}
