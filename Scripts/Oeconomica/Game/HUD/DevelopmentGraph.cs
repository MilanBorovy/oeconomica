using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Oeconomica.Game.CommoditiesNS;
using UnityEngine.EventSystems;

namespace Oeconomica.Game.HUD
{
    class DevelopmentGraph : MonoBehaviour
    {
        private Texture2D graph; //Generated graph
        private int _commodity; //Displayed commodity 0-Electricity, 1-Labour, 2-Vehicles
        public int Commodity
        {
            set
            {
                if(value == _commodity && Visible)
                {
                    Visible = false;
                    return;
                }
                _commodity = Mathf.Clamp(value, 0, 2);
                Visible = true;
            }
        }
        private List<Prices.Development> Development //Development of _commodity
        {
            get
            {
                return _commodity == 0 ?
                    Prices.ElectricityDevelopment :
                    _commodity == 1 ?
                    Prices.LabourDevelopment :
                    Prices.VehiclesDevelopment;
            }
        }

        private bool Visible
        {
            get
            {
                return gameObject.transform.parent.GetComponent<RectTransform>().localScale.x != 0;
            }
            set
            {
                gameObject.transform.parent.GetComponent<RectTransform>().localScale = new Vector3(value ? 1 : 0, 1, 1);
                Refresh();
            }
        }

        private float height //Height of graph
        {
            get
            {
                return gameObject.GetComponent<RectTransform>().rect.height;
            }
        }
        private float width //Width of graph
        {
            get
            {
                return gameObject.GetComponent<RectTransform>().rect.width;
            }
        }

        private void Start()
        {
            Prices.OnDevelopmentUpdate += new Prices.DevelopmentUpdate(Refresh);
            Commodity = 0;
            Visible = false;
            DrawGraph();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                Visible = false;
        }

        //Refresh graph after new round
        private void Refresh()
        {
            if(Visible)
                DrawGraph();
        }

        /// <summary>
        /// Draws graph of development of production, consumption & price
        /// </summary>
        private void DrawGraph()
        {
            graph = new Texture2D(Mathf.CeilToInt(width), Mathf.CeilToInt(height)); //New graph

            DrawAxises(); //Axises x & y

            List<int> production = new List<int>();
            List<int> consumption = new List<int>();
            List<int> price = new List<int>();
            foreach (Prices.Development development in Development)
            {
                production.Add(development.production);
                consumption.Add(development.consumption);
                price.Add(development.price);
            }

            //Maximum of production & consumption
            int maximum = Mathf.Clamp(new int[] { production.Max(), consumption.Max() }.Max(), 10, int.MaxValue);

            DrawCharacteristics(production.ToArray(), 20, maximum, Color.green); //Production graph
            DrawCharacteristics(consumption.ToArray(), 20, maximum, Color.red); //Consumption graph
            DrawCharacteristics(price.ToArray(), 20, 5 + _commodity, Color.yellow); //Price graph

            gameObject.GetComponent<RawImage>().texture = graph; //Apply graph
        }

        /// <summary>
        /// Draws characteristics graph of values
        /// </summary>
        /// <param name="values">Collection of values to draw</param>
        /// <param name="count">Count of displayed values</param>
        /// <param name="max">Upper limit</param>
        /// <param name="color">Color of line</param>
        private void DrawCharacteristics(int[] values, int count, int max, Color color)
        {
            //Conversion unit
            float y_unit = (float)(height - 62) / max;
            float x_unit = (float)(width - 62) / (count - 1);

            for (int idx = 1; idx < count; idx++)
            {
                if(idx < values.Length)
                {
                    DrawLine(
                        new Vector2(31 + (idx - 1) * x_unit, 32 + values[Mathf.Clamp(values.Length - count, 0, values.Length) + idx - 1] * y_unit),
                        new Vector2(31 + idx * x_unit, 32 + values[Mathf.Clamp(values.Length - count, 0, values.Length) + idx] * y_unit),
                        color,
                        3);
                }
            }
        }

        /// <summary>
        /// Draws axises x & y
        /// </summary>
        private void DrawAxises()
        {
            //x-axis
            DrawLine(
                new Vector2(29, 30),
                new Vector2(width - 30, 30),
                Color.black,
                3
                );

            //y-axis
            DrawLine(
                new Vector2(30, 29),
                new Vector2(30, height - 30),
                Color.black,
                3
                );
        }

        /// <summary>
        /// Draws line into the graph
        /// </summary>
        /// <param name="P1">First point</param>
        /// <param name="P2">Second point</param>
        /// <param name="color">Color of line</param>
        /// <param name="thickness">Thicknes of line</param>
        private void DrawLine(Vector2 P1, Vector2 P2, Color color, int thickness)
        {
            //Check whether line thickness is even
            bool even = thickness % 2 == 0;
            thickness += thickness % 2 - 1;

            bool dYbtdX = Mathf.Abs(P1.x - P2.x) < Math.Abs(P1.y - P2.y) ? true : false; //delta Y bigger than delta X

            //Draw lines multiple lines to get thicker line
            for (int i = -((thickness - 1) / 2); i <= (thickness - 1) / 2; i++)
            {
                int x = dYbtdX ? i : 0;
                int y = dYbtdX ? 0 : i;
                DrawLine(
                    new Vector2(P1.x + x, P1.y + y),
                    new Vector2(P2.x + x, P2.y + y),
                    color
                    );
            }

            //If thickness is even
            //then draw 2 lines of 0.5 alpha
            if(even)
            {
                int x = dYbtdX ? 0 : ((thickness + 1) / 2);
                int y = dYbtdX ? ((thickness + 1) / 2) : 0;
                DrawLine(
                    new Vector2(P1.x - x, P1.y),
                    new Vector2(P2.x - x, P2.y),
                    color * new Vector4(1, 1, 1, 0.5f)
                    );
                DrawLine(
                    new Vector2(P1.x, P1.y + y),
                    new Vector2(P2.x, P2.y + y),
                    color * new Vector4(1, 1, 1, 0.5f)
                    );
            }
        }

        /// <summary>
        /// Draws line into graph
        /// </summary>
        /// <param name="P1">First point</param>
        /// <param name="P2">Second point</param>
        /// <param name="color">Color of line</param>
        private void DrawLine(Vector2 P1, Vector2 P2, Color color)
        {
            bool dYbtdX = Mathf.Abs(P1.x - P2.x) < Math.Abs(P1.y - P2.y) ? true : false; //delta Y bigger than delta X

            //If dY>dX
            //then give control to Y
            if(dYbtdX)
            {
                P1 = new Vector2(P1.y, P1.x);
                P2 = new Vector2(P2.y, P2.x);
            }

            //If P1 is more to the right (top) than P2
            //then swap those points
            if(P1.x > P2.x)
            {
                Vector2 temp = P2;
                P2 = P1;
                P1 = temp;
            }

            for (float x = P1.x; x <= P2.x; x++)
            {
                float y = ((x - P1.x) / (P2.x - P1.x)) * (P2.y - P1.y) + P1.y;

                if(dYbtdX)
                    graph.SetPixel(Mathf.CeilToInt(y), Mathf.CeilToInt(x), color);
                else
                    graph.SetPixel(Mathf.CeilToInt(x), Mathf.CeilToInt(y), color);
            }
            graph.Apply();
        }
    }
}
