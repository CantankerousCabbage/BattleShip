using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BattleShip
{

    public class Board : MonoBehaviour
    {
        // [SerializeField] public int _dimensions = 10;
        // [SerializeField] Cell _cell;
        // [SerializeField] Camera _camera;
        // [SerializeField] bool shipsVisible;

        public int _dimensions = 10;
        public Cell _cell;
        public Camera _camera;
        public Game _game;
        public List<Ship> _ships;
        public bool shipsVisible;

        public Cell[,] _cellList;
        public int _turn;

        private List<Cell> _hitList;
        private List<Cell> _missList;
        private Vector2 _origin;
        private float _offset = 0.5f;




        public Board(Game game, int dimensions, Cell cell, Camera camera, bool visible, Vector2 origin)  
        {   
            Debug.Log("New Game");
            Debug.Log("---------------------------------------");
            this._game = game;
            this._origin = origin;
            this._dimensions = dimensions;
            this._cell = cell;
            this._camera = camera;
            this.shipsVisible = visible;
            this._turn = 0;

            _cellList = new Cell[_dimensions, _dimensions];
            _ships = new List<Ship>();
            _hitList = new List<Cell>();
            _missList = new List<Cell>();

            GenerateBoard();

            _ships.Add(new Ship(this, "Destroyer", 2));
            _ships.Add(new Ship(this, "Submarine", 3));
            _ships.Add(new Ship(this, "Cruiser", 4));
            _ships.Add(new Ship(this, "Battleship", 5));
            _ships.Add(new Ship(this, "Carrier", 6));

            PlaceShips();
        }

        public void fire(int X, int Y)
        {   

            this._turn += 1;
            Cell cell = _cellList[X,Y];
            cell.marked = true;
            cell.Fire();
            if(cell.Occupied)
            {   
                //** Reward
                _game.agent.HandleHits();

                Debug.Log("Hit:  (" + X + ", " + Y + ")");
                Ship target = cell._ship; 

                this._hitList.Add(cell);
                //Remove cell from from ships placement list.
                target._placement.RemoveAll(x => (x.X == cell.X && x.Y == cell.Y));
                //If no placements remaining remove ship form boards ship list
                if(target._placement.Count == 0)
                {
                    this._ships.RemoveAll(ship => ship._name == target._name);
                    SunkShipNotice(target);
                }
            }
            else
            {   
                Debug.Log("Miss: (" + X + ", " + Y + ")");
                this._missList.Add(cell);
            }
        }

        void PlaceShips()
        {
            foreach (Ship ship in _ships)
            {
                ship.Spawn();
            }
        }

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
                    var boardCell = Instantiate(_cell, new Vector3(x, y), Quaternion.identity);
                    boardCell.name = $"Cell {countX} {countY}";
                    bool offset = ((x + y) % 2 == 0) ? true : false;
                    boardCell.Init(this, offset, this.shipsVisible, countX, countY);


                    _cellList[countX, countY] = boardCell;
                }
                countY = 0;
            }

            // Camera to hover over board
            float center = (float)_dimensions/2 - _offset;
            _camera.transform.position = new Vector3(center, center, -10);
        }

        void SunkShipNotice(Ship ship)
        {
            Debug.Log("Sunk: " + ship._name + ", Turn: " + this._turn);
        }
    }



}

