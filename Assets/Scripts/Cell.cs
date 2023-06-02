using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleShip
{
     public class Cell : MonoBehaviour
     {
     
          [SerializeField] private Color _baseColor, _offsetColor;
          [SerializeField] private SpriteRenderer _renderer;
          private Board _board;
          public int X {get; set;}
          public int Y {get; set;}
          public bool _offset {get; set;}
          public Ship _ship {get; set;}
          public bool marked {get; set;}

          private bool _occupied;
          public bool Occupied
          {
               get => _occupied;
               set
               {    
                    this._occupied = value;
                    if(this.Visible)GetComponent<SpriteRenderer>().color = Color.gray;
               }
          }

          private bool _visible;
          public bool Visible
          {    
               get => _visible;
               set
               {    
                    this._visible = value;
                   
                    if(this.Visible && this.Occupied)
                    {    
                         GetComponent<SpriteRenderer>().color =  Color.gray;  
                    }
                    else
                    {
                         GetComponent<SpriteRenderer>().color = (this._offset) ? _offsetColor : _baseColor;     
                    }
                }
          }
          

          public void Init(Board board, bool isOffset, bool visible, int x, int y)
          {    
               this.marked = false;
               this.X = x;
               this.Y = y;
               this._board = board;
               this._offset = isOffset;
               this.Visible = visible;
               

          }

          public void Fire()
          {
               GetComponent<SpriteRenderer>().color = (this.Occupied) ? Color.red : Color.white;


          }

          private void OnMouseDown() 
          {   
               this._board._game.fire(X, Y);
          }
     }
}
