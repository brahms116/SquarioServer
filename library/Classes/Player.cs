

namespace squario.library{
  public class Player:IPlayer{
    public string connectionId{get;set;}
    public bool isSpectator{get;set;}
    public string name{get;set;}
    public string room{get;set;}
    public string colour{get;set;}
    public int squareIndex{get;set;}
    public Direction button1 {get;set;}
    public Direction button2 {get;set;}

    public Player(){}

    public Player(IUser user){
      connectionId = user.connectionId;
      name = user.name;
      room = user.room;
    }
  }
}