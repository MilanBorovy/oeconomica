using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oeconomica.Game.BuildingsNS;
using Oeconomica.Game.BranchesNS;

namespace Oeconomica.Game.HUD
{
    public class BuildingUpgrade : MonoBehaviour
    {

        private Budovy buildings = new Budovy();
        bool wait = false;
        private Branches branch;
        private List<Branches> branches;
        private bool upgrade = false;
        private Building buildingLogic;
        private int selected;

        private List<Buildings> displayedBuildings;

        public void Zobrazit(Building building, bool upgrade)
        {
            this.buildingLogic = building;
            this.upgrade = upgrade;
            this.branch = BuildingsExtensions.GetBranch(building.ActualBuilding);
            this.branches = BranchesExtensions.AllBranches();
            this.branch = branch != Branches.NONE ? branch : branches[0];
            this.selected = -1;

            //Shows upgrade&downgrade menu
            gameObject.transform.localScale = new Vector3(1, 1, 1);

            //Shows actual building
            GameObject.Find("Actual").GetComponent<Text>().text = string.Format("Aktuálně: {0}", BuildingsExtensions.GetName(building.ActualBuilding));

            //Available only while building new building or at 1st level
            GameObject.Find("NextBranch").GetComponent<Button>().interactable = (upgrade && BuildingsExtensions.GetGrade(building.ActualBuilding) <= 1);
            GameObject.Find("PreviousBranch").GetComponent<Button>().interactable = (upgrade && BuildingsExtensions.GetGrade(building.ActualBuilding) <= 1);


            ShowBranch();
            HighlightSelected();
            wait = true;
        }

        /// <summary>
        /// Clears available buildings
        /// </summary>
        private void ClearBranch()
        {
            //Clear building names
            gameObject.transform.Find("Selection0").Find("Name").GetComponent<Text>().text = "";
            gameObject.transform.Find("Selection1").Find("Name").GetComponent<Text>().text = "";
            gameObject.transform.Find("Selection2").Find("Name").GetComponent<Text>().text = "";

            //Get all production and consumption icons
            List<GameObject> productionAndConsumption = new List<GameObject>();
            productionAndConsumption.AddRange(GameObject.FindGameObjectsWithTag("ProductionSel0"));
            productionAndConsumption.AddRange(GameObject.FindGameObjectsWithTag("ProductionSel1"));
            productionAndConsumption.AddRange(GameObject.FindGameObjectsWithTag("ProductionSel2"));
            productionAndConsumption.AddRange(GameObject.FindGameObjectsWithTag("ConsumptionSel0"));
            productionAndConsumption.AddRange(GameObject.FindGameObjectsWithTag("ConsumptionSel1"));
            productionAndConsumption.AddRange(GameObject.FindGameObjectsWithTag("ConsumptionSel2"));

            //Clear those icons
            foreach (GameObject p in productionAndConsumption)
            {
                p.GetComponent<RawImage>().texture = (Texture2D)Resources.Load("Icons/none");
                p.transform.FindChild("Value").GetComponent<Text>().text = null;
            }

        }

        /// <summary>
        /// Displays available buildings in currently selected branch
        /// </summary>
        private void ShowBranch()
        {
            //Collection of valid buildings
            List<Buildings> validBuildings = new List<Buildings>();

            ClearBranch(); //Clear window from buildings

            //Get set of valid buildings
            foreach (Buildings b in BranchesExtensions.GetBuildings(branch))
            {
                //Upgrade
                if (upgrade && (BuildingsExtensions.GetGrade(b) == BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) ||
                    BuildingsExtensions.GetGrade(b) == BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) + 1) &&
                    b != buildingLogic.ActualBuilding)
                {
                    validBuildings.Add(b);
                }

                //Downgrade
                else if (!upgrade && BuildingsExtensions.GetGrade(b) == BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) - 1 &&
                    b != buildingLogic.ActualBuilding)
                {
                    validBuildings.Add(b);
                }
            }

            //Display branch name
            gameObject.transform.Find("Branch").GetComponent<Text>().text = string.Format("Odvětví: {0}", BranchesExtensions.GetName(branch));

            //Display valid buildings
            for (int selectionIdx = 0; selectionIdx < validBuildings.Count; selectionIdx++)
            {
                gameObject.transform.Find("Selection" + selectionIdx).Find("Name").GetComponent<Text>().text = BuildingsExtensions.GetName(validBuildings[selectionIdx]);
                DisplayProductionConsumption(selectionIdx, validBuildings[selectionIdx]);
            }

            displayedBuildings = validBuildings;
        }

        /// <summary>
        /// Display information about production&consumption rates of building
        /// </summary>
        /// <param name="building">Building index in array</param>
        /// <param name="buildings">Buildings array</param>
        private void DisplayProductionConsumption(int building, Buildings buildings)
        {
            BuildingsExtensions.DrawPC("ProductionSel" + building, "ConsumptionSel" + building, buildings);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Click outside window
                if (!RectTransformUtility.RectangleContainsScreenPoint(gameObject.GetComponent<RectTransform>(), Input.mousePosition, Camera.main) && !wait)
                    Hide();
                //Select building
                else
                    for (int i = 0; i < 3; i++)
                        if (RectTransformUtility.RectangleContainsScreenPoint(gameObject.transform.Find("Selection" + i).GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
                            !wait &&
                            gameObject.transform.Find("Selection" + i).Find("Name").GetComponent<Text>().text != "")
                            selected = i;

                HighlightSelected();
            }
            if (wait)
            {
                wait = false;
            }
        }

        /// <summary>
        /// Hides upgrade window
        /// </summary>
        public void Hide()
        {
            gameObject.GetComponent<CanvasRenderer>().transform.localScale = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Displays next branch in selection tab
        /// </summary>
        public void NextBranch()
        {
            branches.RemoveAt(0);
            branches.Add(branch);
            branch = branches[0];
            ShowBranch();
            selected = -1; //Unselect
            HighlightSelected();
        }

        /// <summary>
        /// Displays previous branch in selection tab
        /// </summary>
        public void PreviousBranch()
        {
            branch = branches[branches.Count - 1];
            branches.RemoveAt(branches.Count - 1);
            branches.Insert(0, branch);
            branch = branches[0];
            ShowBranch();
            selected = -1; //Unselect
            HighlightSelected();
        }

        private void HighlightSelected()
        {
            //Change color of selected
            for (int i = 0; i < 3; i++)
            {
                gameObject.transform.Find("Selection" + i).GetComponent<Image>().color = new Color(0, 0, i == selected ? 0.5f : 0, 0.5f);
            }

            if (selected != -1 && ((GameLogic.HasTurn.Money >= 4 &&
                BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) == 0) ||
                GameLogic.HasTurn.Money >= 1 &&
                BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) != 0) &&
                GameLogic.actions > 0)
            {
                gameObject.transform.Find("Confirm").GetComponent<Button>().interactable = true;
                if (BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) == 0)
                    GameObject.Find("ConfirmTag").GetComponent<Text>().text = string.Format("POTVRDIT ({0},000,000 Kč)", 4);
                else
                    GameObject.Find("ConfirmTag").GetComponent<Text>().text = string.Format("POTVRDIT ({0},000,000 Kč)", 1);
            }
            else
            {
                GameObject.Find("Confirm").GetComponent<Button>().interactable = false;
                GameObject.Find("ConfirmTag").GetComponent<Text>().text = string.Format("POTVRDIT (nelze)");
            }
        }

        /// <summary>
        /// Applies new building
        /// </summary>
        public void ConfirmBuild()
        {
            Hide(); //Hide this window

            //Take money&action for building
            GameLogic.Action();
            if (BuildingsExtensions.GetGrade(buildingLogic.ActualBuilding) == 0)
                GameLogic.HasTurn.Money = -4;
            else
                GameLogic.HasTurn.Money = -1;

            //Refresh player info
            GameLogic.ShowPlayerInfo();

            //Apply building
            buildingLogic.ChangeBuilding(displayedBuildings[selected]);
        }
    }
}