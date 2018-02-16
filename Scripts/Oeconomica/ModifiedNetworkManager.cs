using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Oeconomica.Game;
using Oeconomica.Menu;

namespace Oeconomica
{
    class ModifiedNetworkManager : NetworkManager
    {
        public override void OnServerConnect(NetworkConnection conn)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("OEconomica") ||
                NetworkServer.connections.Count > 4)
                conn.Disconnect();
            base.OnServerConnect(conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("NetworkLobby"))
            {
                for (int i = 0; i < 4; i++)
                {
                    NetworkIdentity identity = GameObject.Find("Player" + i).GetComponent<NetworkIdentity>();
                    if (identity.clientAuthorityOwner == conn)
                        identity.RemoveClientAuthority(conn);
                }
            }

            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("OEconomica"))
            {
                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                foreach (GameObject building in buildings)
                    if (int.Parse(building.transform.parent.name
                        [building.transform.parent.name.Length - 1].ToString())
                        == conn.connectionId)
                        building.GetComponent<NetworkIdentity>().RemoveClientAuthority(conn);
                GameLogic.players[conn.connectionId].gameObject.GetComponent<NetworkIdentity>().RemoveClientAuthority(conn);
            }
            base.OnServerDisconnect(conn);
        }
    }
}
