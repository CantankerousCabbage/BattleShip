using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BattleShip
{

    public class Board 
    {
        //Board componenets and configuration variables.
        public int _dimensions = 10;
        public Cell _cell;
        public Camera _camera;
        public Game _game;
        public List<Ship> _ships;
        public bool shipsVisible;
        public Cell[,] _cellList;
        public int _hitCount;
        public int _missCount;
        private Vector2 _origin;
      
        public Board(Game game, int dimensions, Cell cell, Camera camera, bool visible, Vector2 origin)  
        {   
            
            this._game = game;
            this._origin = origin;
            this._dimensions = dimensions;
            this._cell = cell;
            this._camera = camera;
            this.shipsVisible = visible;

            _cellList = new Cell[_dimensions, _dimensions];
            _ships = new List<Ship>();
            _hitCount = 0;
            _missCount = 0;
            //Builds board cells
            GenerateBoard();

            //Create ships
            _ships.Add(new Ship(this, "Destroyer", 2));
            _ships.Add(new Ship(this, "Submarine", 3));
            _ships.Add(new Ship(this, "Cruiser", 4));
            _ships.Add(new Ship(this, "Battleship", 5));
            _ships.Add(new Ship(this, "Carrier", 3));

            //Find spawn location for ships
            PlaceShips();
        }

        //Free board memory on board reset 
        public void Reset()
        {

        for (int i = 0; i < _ships.Count; i++)
            {
                _ships[i].Reset();
                _ships[i] = null;
            }
            _ships.Clear();

            // Release cells
            for (int x = 0; x < _dimensions; x++)
            {
                for (int y = 0; y < _dimensions; y++)
                {
                    Cell cell = _cellList[x, y];
                    cell.Reset();
                    GameObject.Destroy(cell.gameObject);
                }
            }
            _cellList = null;

        }

        //Fires on cell. Updates cell status counters and invokes hit and sink rewards.
        public void fire(int X, int Y)
        {   
            Cell cell = _cellList[X,Y];

            cell.Fire();

            //If hit 
            if(cell.Occupied)
            {   
                //Hit reward
                this._game.agent.HandleHits();
                Ship target = cell._ship; 
                this._hitCount++;
                //Remove cell from from ships placement list.
                target._placement.RemoveAll(x => (x.X == cell.X && x.Y == cell.Y));
                //If no placements remaining remove ship form boards ship list
                if(target._placement.Count == 0)
                {
                    this._ships.RemoveAll(ship => ship._name == target._name);
                    //Sink Reward
                    _game.agent.SunkShipReward();
                }
            }
            else
            {   
                this._missCount++;
            }
        }

        void PlaceShips()
        {
            foreach (Ship ship in _ships)
            {
                ship.Spawn();
            }
        }

        //Populates board with cells
        void GenerateBoard()
        {
            int _beginX = (int)_origin.x;
            int _beginY = (int)_origin.y;
            int endX = _beginX + _dimensions;
            int endY = _beginY + _dimensions;
            int countX = 0;
            int countY = 0;

            for(int x = _beginX; x < endX; x++, countX++)
            {
                for(int y = _beginY; y < endY; y++, countY++)
                {
                    var boardCell = GameObject.Instantiate(_cell, new Vector3(x, y), Quaternion.identity, _game.transform);
                    boardCell.name = $"Cell {countX} {countY}";
                    bool offset = ((x + y) % 2 == 0) ? true : false;
                    boardCell.Init(this, offset, this.shipsVisible, countX, countY);

                    _cellList[countX, countY] = boardCell;
                }
                countY = 0;
            }
        }
    }
}
