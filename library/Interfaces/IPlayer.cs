namespace squario.library{
  public interface IPlayer: IPlayerData,IUser{

    //new needed because asp.net doesn't serialize json properly
     new string connectionId{get;set;}
      new string name {get;set;}
      new string room {get;set;}
      new bool isSpectator{get;set;}

      new string colour{get;set;}
      new int squareIndex{get;set;}
      new Direction button1{get;set;}
      new Direction button2{get;set;}
  }
}