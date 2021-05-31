
using System.Collections.Generic;

namespace squario.library{
  public class GameData : IGameData
  {
    public GameData(){}

    public GameData(ILevel level, List<IPlayer> player){
      levelData = level;
      players = player;
    }

    public ILevel levelData {get;set;}
    public List<IPlayer> players {get;set;}
  }
}