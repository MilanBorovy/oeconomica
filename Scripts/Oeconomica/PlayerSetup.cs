using UnityEngine;
using System.Collections;
using Oeconomica.Game;
using System.Collections.Generic;

namespace Oeconomica
{
    public class PlayerSetup : MonoBehaviour
    {
        public static List<Player> players;

        private void Start()
        {
            Object.DontDestroyOnLoad(gameObject);
            players = new List<Player>();
        }

        /// <summary>
        /// Set players for game
        /// </summary>
        public void SetupPlayers(List<Player> players)
        {
            PlayerSetup.players = new List<Player>();
            foreach (Player p in players)
                if (PlayerSetup.players.Count < 4)
                    PlayerSetup.players.Add(p);
        }

        /// <summary>
        /// Destroys own owner ;)
        /// </summary>
        public void CommitSuicide()
        {
            Destroy(gameObject);
        }
    }
}
