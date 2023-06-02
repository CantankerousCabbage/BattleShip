using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random=System.Random;
using System.IO;
namespace BattleShip
{
    public class Ship
    {   
        
        public Board board;
        public string _name;
        // int _width;
        private int _length;

        public List<Cell> _placement;

        // Start is called before the first frame update
        public Ship(Board board, string name, int length)
        {
            this.board = board;
            this._name = name;
            this._length = length;
            this._placement = new List<Cell>();
        }

        // public bool Hit(Cell cell)
        // {
        //     this._placement.RemoveAll(x => (x.X == cell.X && x.Y == cell.Y));
        //     if(this._placement.Count == 0)
        //     {
        //         board._ships.RemoveAll(ship => ship._name == this._name);
        //     }
        // }

        public void Spawn()
        {
            List<List<Cell>> options = new List<List<Cell>>();

            int edgeOffset = board._dimensions - this._length + 1;
            for(int y = 0; y < board._dimensions; y++)
            {
                for(int x = 0; x < edgeOffset; x++)
                {   
                        List<Cell> candidate = new List<Cell>();
                        bool clearPlacement = true;
                        for(int z = x; z < x + this._length && clearPlacement; z++)
                        {
                            clearPlacement = clearPlacement && !board._cellList[z, y].Occupied;
                            candidate.Add(board._cellList[z, y]);
                        }
                        if(clearPlacement) options.Add(candidate); 
                }
            }

            for(int x = 0; x < board._dimensions; x++)
            {
                for(int y = 0; y < edgeOffset; y++)
                {   
                     List<Cell> candidate = new List<Cell>();
                        bool clearPlacement = true;
                        for(int z = y; z < y + this._length && clearPlacement; z++)
                        {
                            clearPlacement = clearPlacement && !board._cellList[x, z].Occupied;
                            candidate.Add(board._cellList[x, z]);
                        }
                        if(clearPlacement) options.Add(candidate); 
                }
            }
              
            Random rnd = new Random();
            // Debug.Log("Ship length: " + this._length);
            // Debug.Log("Count: " + options.Count);

            List<Cell> location = options[rnd.Next(options.Count)];
            

            foreach (Cell cell in location)
            {
                cell.Occupied = true;
                cell._ship = this;
                this._placement.Add(cell);
            }
        }
    }
}
