using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Oeconomica.Game
{
    class PlayerLogic : NetworkBehaviour
    {
        public static GameObject local; //Game object of this player

        public void Start()
        {
            if (isLocalPlayer)
            {
                GameObject.Find("NextTurnButton").GetComponent<Button>().onClick.AddListener(NextTurn);
                GameObject.Find("CharityAdd").GetComponent<Button>().onClick.AddListener(AddCharity);
                
                local = gameObject;
            }

            NetworkConnection conn = gameObject.GetComponent<NetworkIdentity>().connectionToClient;

            //Add corresponding buildings under control of that player
            if (isServer)
            {
                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                foreach (GameObject building in buildings)
                {
                    int id = int.Parse(building.transform.parent.name[building.transform.parent.name.Length - 1].ToString());
                    if (conn != null)
                        if (conn.connectionId == id)
                            building.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
                }
                if (conn != null)
                    GameLogic.players[conn.connectionId].gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
            }
        }

        /// <summary>
        /// End turn and start new one
        /// </summary>
        private void NextTurn()
        {
            if (!isLocalPlayer)
                return;
            CmdNextTurn();
        }

        private void AddCharity()
        {
            if (!isLocalPlayer)
                return;
            CmdAddCharity();
        }

        [Command]
        private void CmdAddCharity()
        {
            GameLogic.executive_logic.AddCharity();
        }

        [Command]
        private void CmdNextTurn()
        {
            GameLogic.executive_logic.NextTurn();
        }

        [Command]
        private void CmdPricesSync()
        {
            GameLogic.executive_logic.PricesSync();
        }
    }
}
