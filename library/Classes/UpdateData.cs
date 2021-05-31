using System.Collections.Generic;
using System.Linq;
namespace squario.library
{
  public class UpdateData : IUpdateData
  {
    
    public List<IDynamicSquare> squareLocs { get; set; }
    public List<IPlayerData> playerDatas { get; set; }

    public List<ISquare> finishedSquares {get;set;}
  }
}