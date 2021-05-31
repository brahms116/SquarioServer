namespace squario.library{
  public interface ISquare:IEntity{
    string colour{get;set;}

    //hidden properties because of JSON serialization, so legit dumb

    new int x{get;set;}
    new int y{get;set;}
    new int width{get;set;}
    new int height{get;set;}
    
  }
}