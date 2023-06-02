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
        public int _attempts;
        List<int> _actions;
        

        void Start()
        {   
            this._attempts = 0;
            this._actions = new List<int>();
            // TODO: Allows training to run in the background when the Unity window loses focus.
            Application.runInBackground = true;
        }

        public override void OnEpisodeBegin()
        {   
            _actions.Clear();
            this._attempts = 0;
            game.Reset();
        }

        public void HandleHits()
        {
            AddReward(1.0f);
        } 

        public void TimePenalty()
        {
            AddReward(-0.1f);
        } 

        public void WinGame()
        {
            AddReward(2.0f);
        } 


        public override void CollectObservations(VectorSensor sensor)
        {
            // TODO: This is just a dummy observation of size 1. Replace!
            for(int y = 0; y < game._dimensions; y++)
            {
                for(int x = 0; x < game._dimensions; x++)
                {   
                    Cell cell = game.board._cellList[x, y];
                    sensor.AddObservation(cell._status);
                }
            }
            
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            // TODO: Replace with logic to control the bird based on the input recevied.
            TimePenalty();
            _attempts++;
            Debug.Log("Choice");
            int choice = actionBuffers.DiscreteActions[0];
            _actions.Add(choice);
            int x_chosen = choice % game._dimensions;
            int y_chosen = choice / game._dimensions; 
            Cell cell = game.board._cellList[x_chosen, y_chosen];

            if(!cell.marked)
            {
                game.fire(x_chosen,y_chosen);
                if(game.board._ships.Count == 0)
                {
                    WinGame();
                    EndEpisode();
                }
            }     
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {   
            Debug.Log("Mask");
            foreach(int action in this._actions)
            {        
                actionMask.SetActionEnabled(0, action, false);
            }           
        }
    }
}
