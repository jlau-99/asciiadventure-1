using System;


namespace asciiadventure {
    public abstract class GameObject {
        
        public int Row {
            get;
             set;
        }
        public int Col {
            get;
             set;
        }
        public int Hp {
            get;
             set;
        }

        public String Token {
            get;
            protected internal set;
        }

        public Screen Screen {
            get;
            protected set;
        }

        public GameObject(int row, int col, int hp, String token, Screen screen){
            Row = row;
            Col = col;
            Hp = hp;
            Token = token;
            Screen = screen;
            Screen[row, col] = this;
        }
        public string ToToken() {
            return Token;
        }

        public virtual Boolean IsPassable() {
            return false;
        }

        public override String ToString() {
            return this.ToToken();
        }

        public void Delete() {
            Screen[Row, Col] = null;
        }
    }

    public abstract class MovingGameObject : GameObject {

        public MovingGameObject(int row, int col, int hp, String token, Screen screen) : base(row, col, hp, token, screen) {}
        
        public virtual void Move(int deltaRow, int deltaCol) {
            int newRow = deltaRow + Row;
            int newCol = deltaCol + Col;
            if (!Screen.IsInBounds(newRow, newCol)) {
                return;
            }
            GameObject gameObject = Screen[newRow, newCol];
            if (gameObject != null && !gameObject.IsPassable()) {
                // TODO: How to handle other objects?
                // walls just stop you
                // objects can be picked up
                // people can be interactd with
                // also, when you move, some things may also move
                // maybe i,j,k,l can attack in different directions?
                // can have a "shout" command, so some objects require shouting
                return;
            }
            // Now just make the move
            int originalRow = Row;
            int originalCol = Col;
            // now change the location of the object, if the move was legal
            Row = newRow;
            Col = newCol;
            Screen[originalRow, originalCol] = null;
            Screen[Row, Col] = this;
            return;
        }
    }

    class Wall : GameObject {
        public Wall(int row, int col, Screen screen) : base(row, col, 5, "=", screen) {}
    }

    class Treasure : GameObject {
        public Treasure(int row, int col, Screen screen) : base(row, col, 1, "T", screen) {}

        public override Boolean IsPassable() {
            return true;
        }
    }
}