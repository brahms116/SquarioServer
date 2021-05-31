

namespace squario.library{
  public interface IUser
  {
      string connectionId{get;set;}
      string name {get;set;}
      string room {get;set;}
      bool isSpectator{get;set;}
  }
}