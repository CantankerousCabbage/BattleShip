using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BattleShip
{

    public class Game : MonoBehaviour
    {   
        [SerializeField] Vector2 _location;
        [SerializeField] Cell _cell;
        [SerializeField] Camera _camera;
        [SerializeField] bool shipsVisible;
        [SerializeField] string name;
        [SerializeField] public float hitReward;
        [SerializeField] public BoomAgent agent;

        public int _dimensions = 10;
        public Board board;
        // Start is called before the first frame update
        void Start()
        {   
            // Vector2 location1 = new Vector2(0,0);
            this.board = new Board(this, _dimensions, _cell, _camera, shipsVisible, _location);   
        }

        public void Reset() 
        {
            Destroy(board);
            this.board = new Board(this, _dimensions, _cell, _camera, shipsVisible, _location);

        }

        public void fire(int X, int Y)
        {
            board.fire(X, Y);
            if(board._ships.Count == 0)
            {
                Debug.Log("Game Over, Turn Count: " + board._turn);
                RecordCount();
                RecordAttempts();
            }
        }
        
        public void RecordCount()
        {   
            string outputFile = "Logs/Results/" + this.name + ".txt";
            string turns = "";
            turns += board._turn;
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
