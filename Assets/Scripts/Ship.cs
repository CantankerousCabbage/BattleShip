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
        //Variables and list of cells ship occupies
        public Board board;
        public string _name;
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

        //Free's memory
        public void Reset()
        {   
            this.board = null;
            _placement.Clear();
            _placement = null;
        }

        //Generates all valid spawn locations for ship then randomly selects one
        public void Spawn()
        {
            List<List<Cell>> options = new List<List<Cell>>();
            //Valid location with x-axis orientation
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
            //Valid location with y-axis orientation
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