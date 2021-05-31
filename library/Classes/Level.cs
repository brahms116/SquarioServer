using System.Collections.Generic;
namespace squario.library{
  public class Level : ILevel
  {
    public List<IEntity> wallLocs { get; set; }
    public List<IDynamicSquare> squareStartLocs { get; set; }
    public List<ISquare> squareFinishLocs { get; set; }
  }

}