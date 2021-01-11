using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace Com.Amaan.GunJunkie
{
    public class Manager : MonoBehaviour
    {
        public string playerPrefabs;
        public Transform spawnPosition;
        private void Start()
        {
            Spawn();

            LoadOtherPlayers();
        }
        public void Spawn() {
            PhotonNetwork.Instantiate(playerPrefabs, new Vector3(Random.Range(-65, 44), 45, Random.Range(-58, 60)), spawnPosition.rotation);
        }
        public void LoadOtherPlayers() {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject playr in players)
            {
                playr.SetActive(true);
            }
        }

    }
}