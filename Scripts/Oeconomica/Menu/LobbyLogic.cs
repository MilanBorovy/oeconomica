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
    class LobbyLogic : NetworkBehaviour
    {
        private void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject go = GameObject.Find("Player" + i);
                RectTransform trans = go.GetComponent<RectTransform>();
                RectTransform but_trans = go.transform.FindChild("Color").GetComponent<RectTransform>();
                RectTransform name_trans = go.transform.FindChild("Name").GetComponent<RectTransform>();
                RectTransform company_trans = go.transform.FindChild("Company").GetComponent<RectTransform>();
                trans.offsetMin = new Vector2(0, 65 - (60 * i));
                trans.offsetMax = new Vector2(0, 115 - (60 * i));
            }
            Availability();
        }

        private void Update()
        {
            PlayersCheck();
            Availability();
        }

        /// <summary>
        /// Redistribute player slots
        /// </summary>
        public void PlayersCheck()
        {
            if (!isServer)
                return;
            List<NetworkConnection> conns = new List<NetworkConnection>(NetworkServer.connections);

            NetworkIdentity identity, but_identity, name_identity, company_identity;
            for (int i = 0; i < 4; i++)
            {
                identity = GameObject.Find("Player" + i).GetComponent<NetworkIdentity>();
                but_identity = identity.transform.FindChild("Color").GetComponent<NetworkIdentity>();
                if (!identity.hasAuthority && identity.clientAuthorityOwner != null)
                {
                    identity.RemoveClientAuthority(identity.clientAuthorityOwner);
                    but_identity.RemoveClientAuthority(but_identity.clientAuthorityOwner);
                }
            }
            for(int i = 0; i < conns.Count; i++)
            {
                if (conns[i] != null)
                {
                    identity = GameObject.Find("Player" + i).GetComponent<NetworkIdentity>();
                    but_identity = identity.transform.FindChild("Color").GetComponent<NetworkIdentity>();
                    identity.AssignClientAuthority(conns[i]);
                    but_identity.AssignClientAuthority(conns[i]);
                }
            }
        }

        /// <summary>
        /// Locks, unclocks player edit
        /// </summary>
        private void Availability()
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject go = GameObject.Find("Player" + i);
                NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                bool authority = (ni.hasAuthority && !isServer) || (isServer && i == 0);
                go.transform.FindChild("Name").GetComponent<InputField>().interactable = authority;
                go.transform.FindChild("Company").GetComponent<InputField>().interactable = authority;
                Button color_button = go.transform.FindChild("Color").GetComponent<Button>();
                color_button.interactable = authority;
                ColorBlock colors = color_button.colors;
                colors.disabledColor = colors.normalColor;
                color_button.colors = colors;
            }
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
        public void StartGameMP()
        {
            GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>().SetupPlayers(CollectPlayerData());
            NetworkManager.singleton.ServerChangeScene("OEconomica");
        }
    }
}
