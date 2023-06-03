using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BattleShip
{

    public class Game : MonoBehaviour
    {   
        // [SerializeField] Vector2 _location;
        [SerializeField] Cell _cell;
        [SerializeField] Camera _camera;
        [SerializeField] bool shipsVisible;
        // [SerializeField] string name;
        [SerializeField] public float hitReward;
        // [SerializeField] public BoomAgent agent;
        
        // [SerializeField] bool record;
        Vector2 _location;
        public int _dimensions = 10;
        private float _offset = 1.15f;
        public Board board;
        public BoomAgent agent;
        public GameManager manager;
        // Start is called before the first frame update
        void Start()
        {   
            // Vector2 location1 = new Vector2(0,0);
            this._location = (Vector2)transform.position;
            // this._location = new Vector2(transform.x, transform.y);
            this.manager = transform.parent.gameObject.GetComponent<GameManager>();
            this.board = new Board(this, _dimensions, _cell, _camera, shipsVisible, _location); 
            this.agent = GetComponent<BoomAgent>(); 

            // Camera to hover over board
            float center = (float)_dimensions / 2 - _offset;
            // float x = transform.position.x + center;
            // float y = transform.position.y + center;
            _camera.transform.position = transform.position + new Vector3(center, center, -10);
        }

        public void Reset() 
        {
            board = null;
            this.board = new Board(this, _dimensions, _cell, _camera, shipsVisible, _location);

        }

        public void fire(int X, int Y)
        {
            board.fire(X, Y);
            if(board._ships.Count == 0)
            {
                // Debug.Log("Game Over, Turn Count: " + board._turn);
                if(manager.record)
                {
                    RecordCount(); 
                    // RecordAttempts();
                }
            }
        }

        public void fire(Cell current)
        {
            fire(current.X, current.Y);
        }
        
        public void RecordCount()
        {   
           manager.Record(agent._attempts);
        }

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

            // Write to the file:
            // results.WriteLine(DateTime.Now);
            results.WriteLine(turns);
            // results.WriteLine();

            // Close the stream:
            results.Close();
        }
    }
}
