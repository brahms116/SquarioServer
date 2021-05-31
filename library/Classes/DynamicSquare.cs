namespace squario.library{
  public class DynamicSquare:IDynamicSquare{

    // public static implicit operator DynamicSquare(Square sq){
    //   return new DynamicSquare(){x=sq.x,y=sq.y,width=sq.width,height=sq.height,colour=sq.colour};
    // }

    public DynamicSquare(){}
    public DynamicSquare(ISquare sq){
      x=sq.x;
      y=sq.y;
      width=sq.width;
      height=sq.height;
    }
    public Direction direction{get;set;}
    public int x{get;set;}
    public int y{get;set;}
    public int width{get;set;}
    public int height{get;set;}
    public string colour{get;set;}
  }
}