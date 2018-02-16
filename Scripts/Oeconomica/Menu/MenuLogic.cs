using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

namespace Oeconomica.Menu
{
    class MenuLogic : MonoBehaviour
    {

        public void Quit()
        {
            Application.Quit();
            Debug.Log("Game quit.");
        }

        /// <summary>
        /// Hides UI element
        /// </summary>
        public void Hide(GameObject tohide)
        {
            tohide.transform.localScale = new Vector3(0, 1, 1);
        }

        /// <summary>
        /// Shows UI element
        /// </summary>
        public void Show(GameObject toshow)
        {
            toshow.transform.localScale = new Vector3(1, 1, 1);
        }

        /// <summary>
        /// Collects data about players from LocalMPSetPlayers menu
        /// </summary>
        public List<Player> CollectPlayerData()
        {
            Transform playerData = GameObject.Find("PlayerData").transform;
            List<Player> players = new List<Player>();
            for(int i = 0; i < playerData.childCount; i++)
            {
                Transform playerinfo = playerData.GetChild(i);

                //Player name
                string name = playerinfo
                    .Find("Name")
                    .Find("Text")
                    .GetComponent<Text>()
                    .text;

                //Company name
                string company = playerinfo
                    .Find("Company")
                    .Find("Text")
                    .GetComponent<Text>()
                    .text;

                //Player color
                Color color = playerinfo
                    .Find("Color")
                    .GetComponent<ColorPicker>()
                    .SelectedColor;

                Player player = new Player(
                    name == "" ? "Hráč " + (i + 1) : name,
                    company == "" ? "Společnost hráče " + (i + 1) : company,
                    i,
                    color);

                players.Add(player);
            }
            return players;
        }

        /// <summary>
        /// Starts new local MP game
        /// </summary>
        public void StartGameLocMP()
        {
            GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>().SetupPlayers(CollectPlayerData());
            GameObject.Find("NetworkLogic").GetComponent<NetworkManager>().StartHost();
            SceneManager.LoadScene("OEconomica", LoadSceneMode.Single);
        }
    }
}
