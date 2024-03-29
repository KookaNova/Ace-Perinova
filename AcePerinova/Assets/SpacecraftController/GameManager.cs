using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AcePerinova.Controller;
using Fusion;

namespace AcePerinova.GameManagement{
    /// <summary>
    /// Handles the initialization of gamemodes and spawning in players.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public GameObject playerPrefab, aiPrefab;
        public int playersA, playersB;
        public int scoreA, scoreB;
        public int startCountdown = 3;
        NetworkRunner runner;

        public List<TargetableObject> allTargets;
        #region Game Start

        private void Awake() {
            //Only start if client is host
            
            CheckGameRequirements();
            
        }

        private void CheckGameRequirements(){
            //check for minimum players
            //check if gamemode is selected

            //Start countdown
            StartCoroutine(CountDown());

        }

        //RPC
        private void StartGame(){
            //host must start the game.
            //start counting down to the end of the game.

            //clients spawn themselves.
            SpawnPlayer();
            //master client spawns AI.



        }

        #endregion

        #region Selection and Spawning

        private void OpenSelect(){

        }
        private void CloseSelect(){
            SpawnPlayer();
        }

        private void SpawnPlayer(){
            //if game isn't started, return. This tells select not to spawn.
            //if select is loaded, return. Select will do the spawning.
            Instantiate(playerPrefab);
            //instantiate at spawn points. This will be Network.Instantiate in the future.
        }
        public void RegisterTarget(TargetableObject newTarget){
            allTargets.Add(newTarget);
            if(newTarget.team == 0){
                playersA++;
            }
            if(newTarget.team == 1){
                playersB++;
            }
        }

        #endregion

        IEnumerator CountDown() {
            int seconds = startCountdown;
            while (seconds > 0) {
                yield return new WaitForSeconds(seconds);
                seconds--;
            }
            StartGame();
        }
    }
}


