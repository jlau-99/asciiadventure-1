using System;

namespace asciiadventure {
    class Player : MovingGameObject {
        public Player(int row, int col, Screen screen, string name) : base(row, col, 10, "@", screen) {
            Name = name;
        }
        public string Name {
            get;
            protected set;
        }
        public override Boolean IsPassable(){
            return true;
        }
        public override void Move(int deltaRow, int deltaCol) {
          int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol)){
                return;
            }
            GameObject other = Screen[newRow, newCol];
            if (other == null){
              int originalRow = Row;
              int originalCol = Col;
              Row = newRow;
              Col = newCol;
              Screen[originalRow, originalCol] = null;
              Screen[Row, Col] = this;
              return;
            } else if (other is Mobbullet || other is Mob) {
              int originalRow = Row;
              int originalCol = Col;
              Row = newRow;
              Col = newCol;
              Screen[originalRow, originalCol] = null;
              Screen[Row, Col] = this;
              Hp--;
              return;
            }
        }
        public void Action(int deltaRow, int deltaCol){
            int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol)){
                return;
            }
            GameObject other = Screen[newRow, newCol];
            if (other == null){
              Screen[newRow, newCol] = new Bullet(newRow, newCol,deltaRow, deltaCol, Screen);
              return;
            }
            if (other is Mobbullet || other is Mob || other is Wall) {
              Bullet bul = new Bullet(newRow, newCol,deltaRow, deltaCol, Screen);
              if (other.Hp == 1) {
                Screen[newRow, newCol] = null;
                return;
              } else if (other.Hp > bul.Hp) {
                other.Hp -= bul.Hp;
                Screen[newRow, newCol] = other;
                return;
              } else {
                bul.Hp -= other.Hp;
                return;
              }
            }
            // TODO: Interact with the object
            if (other is Treasure){
                other.Delete();
                return;
            }
        }
    }
}