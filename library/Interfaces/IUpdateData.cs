using System.Collections.Generic;

namespace squario.library{
  public interface IUpdateData{
    List<IDynamicSquare> squareLocs {get;set;}
    List<IPlayerData> playerDatas{get;set;}

    List<ISquare> finishedSquares{get;set;}
  }
}