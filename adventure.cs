using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/*
 Cool ass stuff people could implement:
 > jumping
 > attacking
 > randomly moving monsters
 > smarter moving monsters
*/
namespace asciiadventure {
  public class Game {
    private Random random = new Random();
    private static Boolean Eq(char c1, char c2){
      return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private static string Menu() {
      return "WASD to move\nIJKL to attack\nQ to quit\nEnter command: ";
    }

    private static void PrintScreen(Screen screen, string message, string menu) {
      Console.Clear();
      Console.WriteLine(screen);
      Console.WriteLine($"\n{message}");
      Console.WriteLine($"\n{menu}");
    }
    public void Run() {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"Please enter map size.\n small: 1  medium: 2  large: 3  huge: 4");
      char c = Console.ReadKey(true).KeyChar;
      while (!(c=='1'||c=='2'||c=='3'||c=='4')) {
        Console.WriteLine("Please enter again");
        c = Console.ReadKey(true).KeyChar;
      }
      Screen screen;
      if (c=='1') {  
         screen = new Screen(5, 5);
        // add a couple of walls
        for (int i=1; i < 4; i++){
          new Wall(1, i, screen);
          new Wall(3, i, screen);
        }

      } else if (c == '2') {  
        screen = new Screen(10, 10);
        for (int i=2; i < 8; i++){
          new Wall(1, i, screen);
          new Wall(5, i, screen);
        }
        for (int i=0; i < 3; i++){
          new Wall(3, i, screen);
          new Wall(3, 9-i, screen);
          new Wall(7, i, screen);
          new Wall(7, 9-i, screen);
        }
      } else if (c == '3') {  
        screen = new Screen(15, 15);
        for (int i=4; i < 11; i++){
          new Wall(1, i, screen);
          new Wall(5, i, screen);
          new Wall(9, i, screen);
          new Wall(13, i, screen);
        }
        for (int i=0; i < 4; i++){
          new Wall(3, i+1, screen);
          new Wall(3, 13-i, screen);
          new Wall(7, i, screen);
          new Wall(7, 14-i, screen);
          new Wall(11, i+1, screen);
          new Wall(11, 13-i, screen);
        }
      } else {  
         screen = new Screen(20, 20);
        for (int i=5; i < 15; i++){
          new Wall(2, i, screen);
          new Wall(7, i, screen);
          new Wall(11, i, screen);
          new Wall(16, i, screen);
        }
        for (int i=0; i < 6; i++){
          new Wall(4, i+1, screen);
          new Wall(4, 18-i, screen);
          new Wall(9, i, screen);
          new Wall(9, 19-i, screen);
          new Wall(14, i+1, screen);
          new Wall(14, 18-i, screen);
        }
      }
      Console.WriteLine($"Please enter difficulty mode.\n easy: 1  medium: 2  hard: 3  extreme: 4  impossible: 5");
      c = Console.ReadKey(true).KeyChar;
      while (!(c=='1'||c=='2'||c=='3'||c=='4'||c=='5')) {
        Console.WriteLine("Please enter again");
        c = Console.ReadKey(true).KeyChar;
      }
      if (c=='1') {
        new Mob(screen.NumRows-1, screen.NumCols-1, screen);
      } else if (c == '2') {
        for (int i=0; i < screen.NumRows; i+=3)
        new Mob(screen.NumRows-1, screen.NumCols-1-i, screen);
      } else if (c == '3') {
        for (int i=0; i < screen.NumRows; i+=2)
        new Mob(screen.NumRows-1, screen.NumCols-1-i, screen);
      } else if (c == '4') {
        for (int i=0; i < screen.NumRows; i++)
        new Mob(screen.NumRows-1, screen.NumCols-1-i, screen);
      } else {
        for (int i=0; i < screen.NumRows; i++) {
        new Mob(screen.NumRows-2, screen.NumCols-1-i, screen);
        new Mob(screen.NumRows-1, screen.NumCols-1-i, screen);
        }
      }
      // add a player
      Player player = new Player(0, screen.NumCols/2, screen, "Zelda");
      
      // initially print the game board
      PrintScreen(screen, "Welcome!", Menu());
      Boolean gameOver = false;
      while (!gameOver) {
          char input = Console.ReadKey(true).KeyChar;

          String message = "";

          if (Eq(input, 'q')) {
            break;
          } else if (Eq(input, 'w')) {
            player.Move(-1, 0);
          } else if (Eq(input, 's')) {
            player.Move(1, 0);
          } else if (Eq(input, 'a')) {
            player.Move(0, -1);
          } else if (Eq(input, 'd')) {
            player.Move(0, 1);
          } else if (Eq(input, 'i')) {
            player.Action(-1, 0);
          } else if (Eq(input, 'k')) {
            player.Action(1, 0);
          } else if (Eq(input, 'j')) {
            player.Action(0, -1);
          } else if (Eq(input, 'l')) {
            player.Action(0, 1);
          } else {
            message = $"Unknown command: {input}";
          }
           
              List<Mobbullet> mbuls = new List<Mobbullet>();
              for (int i = 0; i < screen.NumRows; i++) {
                 for (int j = 0; j < screen.NumCols; j++) {
                   if (screen[i,j] is Mobbullet) {
                      mbuls.Add((Mobbullet)screen[i,j]);
                    }
                  }
               }
               foreach (Mobbullet mbul in mbuls) {
                 mbul.Move(mbul.deltarow, mbul.deltacol);
               }
               List<Bullet> buls = new List<Bullet>();
               for (int i = 0; i < screen.NumRows; i++) {
                 for (int j = 0; j < screen.NumCols; j++) {
                   if (screen[i,j] is Bullet) {
                      buls.Add((Bullet)screen[i,j]);
                    }
                  }
               }
               foreach (Bullet bul in buls) {
                 bul.Move(bul.deltarow, bul.deltacol);
               }
               List<Mob> mobs = new List<Mob>();
          for (int i = 0; i < screen.NumRows; i++) {
            for (int j = 0; j < screen.NumCols; j++) {
              if (screen[i,j] is Mob) {
                mobs.Add((Mob)screen[i,j]);
              }
            }
          }
          foreach (Mob mob in mobs) {
                int up;
                if (player.Row > mob.Row) {
                  up = 1;
                } else if (player.Row == mob.Row) {
                  up = 0;
                } else {
                  up = -1;
                }
                int right;
                if (player.Col > mob.Col) {
                  right = 1;
                } else if (player.Col == mob.Col) {
                  right = 0;
                } else  {
                  right = -1;
                }
                if (screen[mob.Row, mob.Col+right] is Player) {
                  mob.Move(0, right);
                }
                else if (screen[mob.Row+up, mob.Col] is Player) {
                  mob.Move(up, 0);
                }
                else if (up != 0 && screen[mob.Row + up, mob.Col] == null) {
                  int n = random.Next(3);
                  if (n != 0) {
                    mob.Move(up, 0);
                  } else {
                    mob.Action(up, 0);
                  }
                } else if (up != 0 && screen[mob.Row + up, mob.Col] is Wall) {
                  if (screen[mob.Row, mob.Col + right] == null) {
                    mob.Move(0, right);
                  } else {
                    mob.Action(up, 0);
                  }
                } else if (right != 0) {
                  int n = random.Next(3);
                  if (n != 0) {
                    mob.Move(0, right);
                  } else {
                    mob.Action(0, right);
                  }
                } else if (right == 0) {
                  if (screen.IsInBounds(mob.Row, mob.Col+1) && screen[mob.Row, mob.Col+1] == null) {
                    mob.Move(0, 1);
                  }
                  else if (screen.IsInBounds(mob.Row, mob.Col-1) && screen[mob.Row, mob.Col-1] == null) {
                    mob.Move(0, -1);
                  }
                } 
              }
          message = $"Remaining HP:{player.Hp}";
          if (mobs.Count == 0) {
            gameOver = true;
            message += $"\n\nYou win!";
          }
          PrintScreen(screen, message, Menu());
          if (player.Hp <= 0) {
            gameOver = true;
          }
      }
      Console.WriteLine("Y to play again, Q to quit");
      char cont = Console.ReadKey(true).KeyChar;
      while (cont!='y'&&cont!='q') {
        cont = Console.ReadKey(true).KeyChar;
        Console.WriteLine("Please enter again");
      }
      if (cont == 'y') this.Run();
    }
    public static void Main(string[] args){
      Game game = new Game();
      game.Run();
    }
  }
}