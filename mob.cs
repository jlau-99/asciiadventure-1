using System;

namespace asciiadventure {
    public class Mob : MovingGameObject {
        public Mob(int row, int col, Screen screen) : base(row, col, 3, "#", screen) {}
        public override void Move(int deltaRow, int deltaCol) {
          int newRow = deltaRow + Row;
          int newCol = deltaCol + Col;
          if (!Screen.IsInBounds(newRow, newCol)) {
            Screen[Row,Col]=null;
            return;
          }
          GameObject gameObject = Screen[newRow, newCol];
          if (gameObject == null) {
            int originalRow = Row;
              int originalCol = Col;
              Row = newRow;
              Col = newCol;
              Screen[originalRow, originalCol] = null;
              Screen[Row, Col] = this;
              return;
          } else if (gameObject is Wall) {
            gameObject.Hp--;
            if (gameObject.Hp==0) {
              Screen[newRow, newCol]=null;
            }
          }
          else if (gameObject is Bullet) {
            if (gameObject.Hp == Hp) {
              Screen[newRow, newCol] = null;
              Screen[Row, Col] = null;
            } else if (gameObject.Hp > Hp) {
              gameObject.Hp-=Hp;
              Screen[Row, Col] = null;
            } else {
              Screen[newRow, newCol] = null;
              Hp-=gameObject.Hp;
              int originalRow = Row;
              int originalCol = Col;
              Row = newRow;
              Col = newCol;
              Screen[originalRow, originalCol] = null;
              Screen[Row, Col] = this;
              return;
            }
          }
          else if (gameObject is Player) {
            gameObject.Hp-=Hp;
            Screen[Row,Col]=null;
          }
        }
        public void Action(int deltaRow, int deltaCol) {
          int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol)){
                return;
            }
            GameObject other = Screen[newRow, newCol];
            if (other == null){
              Screen[newRow, newCol] = new Mobbullet(newRow, newCol,deltaRow, deltaCol, Screen);
              return;
            }
            if (other is Mobbullet || other is Mob) {
              return;
            }
            if (other is Bullet) {
              other.Hp--;
              if (other.Hp==0)
                Screen[newRow, newCol]=null;
              return;
            }
            if (other is Wall) {
              other.Hp--;
              if (other.Hp==0)
                Screen[newRow, newCol]=null;
              return;
            }
            if (other is Player) {
              other.Hp--;
              return;
            }
        }
    }
    public class Mobbullet : MovingGameObject {
      public int deltarow, deltacol;
        public Mobbullet(int row, int col, int r, int c, Screen screen) : base(row, col, 1, ".", screen) {
          deltarow=r;
          deltacol=c;
          }
          public override void Move(int deltaRow, int deltaCol) {
          int newRow = deltaRow + Row;
          int newCol = deltaCol + Col;
          if (!Screen.IsInBounds(newRow, newCol)) {
            Screen[Row,Col]=null;
            return;
          }
          GameObject gameObject = Screen[newRow, newCol];
          if (gameObject == null) {
            int originalRow = Row;
              int originalCol = Col;
              Row = newRow;
              Col = newCol;
              Screen[originalRow, originalCol] = null;
              Screen[Row, Col] = this;
              return;
          } else if (gameObject is Wall) {
            gameObject.Hp--;
            if (gameObject.Hp==0) {
              Screen[newRow, newCol]=null;
            }
            Screen[Row, Col]=null;
          }
          else if (gameObject is Bullet) {
            gameObject.Hp--;
            if (gameObject.Hp==0)
              Screen[newRow, newCol]=null;
            Screen[Row,Col]=null;
          }
          else if (gameObject is Mob) {
            Screen[Row,Col]=null;
          }
          else if (gameObject is Player) {
            gameObject.Hp--;
            Screen[Row,Col]=null;
          }
        }
    }
    public class Bullet : MovingGameObject {
      public int deltarow, deltacol;
        public Bullet(int row, int col, int r, int c, Screen screen) : base(row, col, 1, "o", screen) {
          deltarow=r;
          deltacol=c;
          }
        public override void Move(int deltaRow, int deltaCol) {
          int newRow = deltaRow + Row;
          int newCol = deltaCol + Col;
          if (!Screen.IsInBounds(newRow, newCol)) {
            Screen[Row,Col]=null;
            return;
          }
          GameObject gameObject = Screen[newRow, newCol];
          if (gameObject == null) {
            int originalRow = Row;
              int originalCol = Col;
              Row = newRow;
              Col = newCol;
              Screen[originalRow, originalCol] = null;
              Screen[Row, Col] = this;
              return;
          } else if (gameObject is Mob || gameObject is Mobbullet || gameObject is Wall) {
            if (gameObject.Hp == Hp) {
              Screen[newRow, newCol] = null;
              Screen[Row, Col] = null;
            } else if (gameObject.Hp > Hp) {
              gameObject.Hp-=Hp;
              Screen[Row, Col] = null;
            } else {
              Screen[newRow, newCol] = null;
              Hp-=gameObject.Hp;
              int originalRow = Row;
              int originalCol = Col;
              Row = newRow;
              Col = newCol;
              Screen[originalRow, originalCol] = null;
              Screen[Row, Col] = this;
              return;
            }
          }
        }
    }
}