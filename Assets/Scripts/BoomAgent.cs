using System.Collections;
using System.Collections.Generic;
using System;
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
        int[] _state;
        Cell lastTurn;
        

        void Start()
        {   
            this.game = GetComponent<Game>();
            this._attempts = 0;
            this._actions = new List<int>();
            this._state = new int[game._dimensions * game._dimensions];
            // TODO: Allows training to run in the background when the Unity window loses focus.
            Application.runInBackground = true;
        }

        public override void OnEpisodeBegin()
        {   
            Array.Clear(_state, 0, _state.Length);
            _actions.Clear();
            this._attempts = 0;
            game.Reset();
        }

        public void HandleHits()
        {
            AddReward(0.5f);
        } 

        public void SunkShipReward()
        {
           AddReward(1.0f); 
        }

        public void AdjacentHitReward()
        {
            AddReward(0.5f);
        } 

        public void TimePenalty()
        {   //Off
            AddReward(-.05f);
        } 

        public void WinGame()
        {
            AddReward((100.0f - (float)this._attempts) * 0.1f);
        } 


        public override void CollectObservations(VectorSensor sensor)
        {
            
              
            foreach(int cell in this._state)
            {
                sensor.AddObservation(cell);
            }

                // Observations X, Y of last shot. Status (hit or miss)
                // sensor.AddObservation(lastTurn.X);
                // sensor.AddObservation(lastTurn.Y); 
                // sensor.AddObservation(lastTurn._status); 
            

        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            _attempts++;
            // Debug.Log("Choice");
            int choice = actionBuffers.DiscreteActions[0];
            TimePenalty(); 
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
            _state[choice] = currentChoice._status;
                
        }

        public bool Adjacent(Cell old, Cell current)
        {
            return Mathf.Abs((old.Y - current.Y) + (current.X - old.X)) == 1;
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {   
            foreach(int action in this._actions)
            {        
                actionMask.SetActionEnabled(0, action, false);
            }           
        }

    }
}
