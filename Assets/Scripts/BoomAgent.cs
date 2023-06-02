using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace BattleShip
{
    public class BoomAgent : Agent // Note that BirdAgent implements 'Agent', not 'MonoBehaviour'
    {   

        [SerializeField] Game game;
        

        void Start()
        {
            // TODO: Allows training to run in the background when the Unity window loses focus.
            Application.runInBackground = true;
        }

        public override void OnEpisodeBegin()
        {
            game.Reset();
        }

        public void HandleHits()
        {
            AddReward(1.0f);
        } 


        public override void CollectObservations(VectorSensor sensor)
        {
            // TODO: This is just a dummy observation of size 1. Replace!
            sensor.AddObservation(game.hitReward);
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            // TODO: Replace with logic to control the bird based on the input recevied.
            int x_chosen = actionBuffers.DiscreteActions[0];
            int y_chosen = actionBuffers.DiscreteActions[1];
            // Debug.Log("Action chosen = " + action_chosen);
            Cell cell = game.board._cellList[x_chosen, y_chosen];

            if(!cell.marked)
            {
                game.fire(x_chosen,y_chosen);
            }
            
            // switch (action_chosen)
            // {
            //     case 0: 
            //         game.fire(0,0);
            //         break;
            //     case 1:
            //         game.fire(5,4);
            //         break;
            //     case 2:
            //         game.fire(8,3);
            //         break;
            //     case 3:
            //         game.fire(4,2);
            //         break;
            //     case 4:
            //         game.fire(1,1);
            //         break;
            // }
        }
    }
}
