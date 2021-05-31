using System.Collections.Generic;
namespace squario.library
{
    public interface ILevel{
      List<IEntity> wallLocs{get;set;}
      List<IDynamicSquare> squareStartLocs{get;set;}
      List<ISquare> squareFinishLocs{get;set;}
    }
}