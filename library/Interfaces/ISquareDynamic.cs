namespace squario.library{
  public interface IDynamicSquare:ISquare{

    Direction direction{get;set;}

    //hidden properties because of JSON serialization, so legit dumb

    new string colour{get;set;}
    new int x{get;set;}
    new int y{get;set;}
    new int width{get;set;}
    new int height{get;set;}
  }
}