using System.Collections.Generic;
namespace squario.library{
  public interface IGame{
    ILevel levelData{get;set;}
    List<IDynamicSquare> squareLocs{get;set;}

    List<ISquare> finishedSquares{get;set;}

    
  }
}