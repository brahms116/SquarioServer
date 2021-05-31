

namespace squario.library{
  public class User:IUser{
    public string name{set;get;}

    public bool isSpectator{get;set;}
    public string room{get;set;}
    public string connectionId{get;set;}
  }
}