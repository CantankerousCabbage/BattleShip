using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BattleShip
{

    public class Game : MonoBehaviour
    {   
        
        [SerializeField] Cell _cell;
        [SerializeField] Camera _camera;
        [SerializeField] bool shipsVisible;

        Vector2 _location;
        public int _dimensions = 10;
        private float _offset = 0.50f;
        public Board board;
        public BoomAgent agent;
        public GameManager manager;
        // Start is called before the first frame update
        void Start()
        {   
            //Initialise board and agent varibles
            this._location = (Vector2)transform.position;
            this.manager = transform.parent.gameObject.GetComponent<GameManager>();
            this.board = new Board(this, _dimensions, _cell, _camera, shipsVisible, _location); 
            this.agent = GetComponent<BoomAgent>(); 

            // Camera to hover over board
            float center = (float)_dimensions / 2 - _offset;
            _camera.transform.position = transform.position + new Vector3(center, center, -10);
        }

        //Free memory from hierarchy
        public void Reset() 
        {
            board.Reset();
            board = null;
            this.board = new Board(this, _dimensions, _cell, _camera, shipsVisible, _location);
        }

        //Manages firing and records
        public void fire(int X, int Y)
        {
            board.fire(X, Y);
            if(board._ships.Count == 0)
            {
                if(manager.record)
                {
                    RecordCount(); 
                    // RecordAttempts();
                }
            }
        }

        //Overloaded fire method
        public void fire(Cell current)
        {
            fire(current.X, current.Y);
        }
        
        public void RecordCount()
        {   
           manager.Record(agent._attempts);
        }

        //Deprecated Attemps record keeper. For recording failed shots when actions
        //were drawn from two discrete branches with no action mask
        public void RecordAttempts()
        {   
            string outputFile = "Logs/Results/" + this.name + "_Attempts.txt";
            string turns = "";
            turns += agent._attempts;
            StreamWriter results;

            if (!File.Exists(outputFile))
            {
                results = new StreamWriter(outputFile);
            }
            else
            {
                results = File.AppendText(outputFile);
            }

            results.WriteLine(turns);
            results.Close();
        }
    }
}