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

        // [SerializeField] Game game;
        public Game game;
        public int _attempts;
        List<int> _actions;
        Cell lastTurn;
        

        void Start()
        {   
            this.game = GetComponent<Game>();
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
            AddReward(0.5f);
        } 

        public void AdjacentHitReward()
        {
            AddReward(0.5f);
        } 

        public void TimePenalty()
        {   //Off
            AddReward(-0.1f);
        } 

        public void WinGame()
        {
            AddReward(((100.0f - (float)this._attempts) * 0.1f));
        } 


        public override void CollectObservations(VectorSensor sensor)
        {
            //Observation 2d matrix observations
            // for(int y = 0; y < game._dimensions; y++)
            // {
            //     for(int x = 0; x < game._dimensions; x++)
            //     {   
            //         Cell cell = game.board._cellList[x, y];
            //         sensor.AddObservation(cell._status);
            //     }
            // }
            if(_attempts > 0)
            {   
                //Observations X, Y of last shot. Status (hit or miss)
                sensor.AddObservation(_actions[_actions.Count - 1]);
                sensor.AddObservation(lastTurn.X);
                sensor.AddObservation(lastTurn.Y); 
                sensor.AddObservation(lastTurn._status); 
            }

        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            _attempts++;
            // Debug.Log("Choice");
            int choice = actionBuffers.DiscreteActions[0]; 
            _actions.Add(choice);  
            int x_chosen = choice % game._dimensions;
            int y_chosen = choice / game._dimensions;
            Cell currentChoice = game.board._cellList[x_chosen, y_chosen];

            //Reward for selecting adjacent to a hit
            if(_attempts > 1)
            {
                if(Adjacent(this.lastTurn, currentChoice) && lastTurn.Occupied)
                {   
                    if(lastTurn.Occupied)
                    {
                       AdjacentHitReward(); 
                    }
                }
                
            }
            
            this.lastTurn = currentChoice;

            game.fire(currentChoice);
            if(game.board._ships.Count == 0)
            {
                WinGame();
                EndEpisode();
            }
            
                
        }

        public bool Adjacent(Cell old, Cell current)
        {
            return Mathf.Abs((old.Y - current.Y) + (current.X - old.X)) == 1;
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {   
            // Debug.Log("Mask");
            // Debug.Log("Attemps: " + _attempts);
            // Debug.Log("Actions: " + _actions.Count);
            foreach(int action in this._actions)
            {        
                actionMask.SetActionEnabled(0, action, false);
            }           
        }

    }
}
