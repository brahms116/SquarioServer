
using System.Collections.Generic;

namespace squario.library{
  public interface IGameData{
    ILevel levelData{get;set;}
    List<IPlayer> players{get;set;}
  }
}