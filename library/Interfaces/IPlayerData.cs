

namespace squario.library{
  public enum Direction{
    empty,
    up,
    right,
    down,
    left
  }
  public interface IPlayerData{
    string colour{get;set;}
    int squareIndex{get;set;}
    Direction button1{get;set;}
    Direction button2{get;set;}
  }
}