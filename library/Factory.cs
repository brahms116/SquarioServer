using System.Collections.Generic;
using System;
namespace squario.library{
  public class Factory{
    public static IPlayer PlayerFromUser(IUser user){
      return new Player(user);
    }
    public static IRoom CreateRoom(string name){
      return new Room(name);
    }
    public static ISquare CreateSquare(int x,int y, int size, string colour){
      return new Square(){x=x,y=y,width=size,height=size,colour=colour};
    }
    public static IDynamicSquare CreateDynamicSquare(int x,int y, int size, string colour){
      return new DynamicSquare(){x=x,y=y,width=size,height=size,colour=colour};
    }

    public static IGameData CreateGameData(ILevel level, List<IPlayer> players){
      return new GameData(level,players);
    }

    public static IUpdateData CreateUpdateData(List<IDynamicSquare> squareLocs, List<IPlayer> playerDatas, List<ISquare> finishedSquares){
      List<IPlayerData> temp= new List<IPlayerData>();
      playerDatas.ForEach(d=>{
        temp.Add(d);
      });
      return new UpdateData(){squareLocs=squareLocs,playerDatas=temp, finishedSquares=finishedSquares};
    }

    public static IGame CreateGame(int numSquare){
      if(numSquare>Constants.maxSquares) {
        throw new ArgumentException();
      }
      return new Game(numSquare);
    }
    public static ILevel CreateLevel(List<IEntity> wallLocs, List<ISquare> squareFinishLocs, List<IDynamicSquare> squareStartLocs){
      return new Level(){wallLocs=wallLocs,squareFinishLocs=squareFinishLocs,squareStartLocs=squareStartLocs};
    }
    public static IUser CreateUser(string name, string connectionId, string roomName, bool isSpectator){
      return new User{name=name,connectionId=connectionId,room=roomName,isSpectator=isSpectator};
    }

  }
}