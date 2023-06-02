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
        List<int> actions;
        

        void Start()
        {
            // TODO: Allows training to run in the background when the Unity window loses focus.
            Application.runInBackground = true;
        }

        public override void OnEpisodeBegin()
        {
            this.actions.Clear();
            game.Reset();
        }

        public void HandleHits()
        {
            AddReward(1.0f);
        } 


        public override void CollectObservations(VectorSensor sensor)
        {
            // TODO: This is just a dummy observation of size 1. Replace!
            sensor.AddObservation();
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            // TODO: Replace with logic to control the bird based on the input recevied.
            int choice = actionBuffers.DiscreteActions[0];
            actions.Add(choice);

            int x_chosen = choice % game._dimensions;
            int y_chosen = choice / game._dimensions; 
            Cell cell = game.board._cellList[x_chosen, y_chosen];

            
            game.fire(x_chosen,y_chosen);
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {   
            foreach(int action in this.actions)
            {
                actionMask.SetActionEnabled(0, action, false);
            }           
        }
    }
}
