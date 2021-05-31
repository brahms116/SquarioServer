using System.Collections.Generic;
using System;
namespace squario.library{

  public enum GameState{
    Lobby,
    Game,
    GameLose,
    GameWin
  }
  public interface IRoom{
    string name{get;set;}

    IPlayer host{get;}

    GameState gameState{get;set;}

    List<IUser> spectators{get;}

    List<IPlayer> players{get;}
    bool isEmpty{get;}
    bool hasSpectators{get;}
    void addPlayer(IUser user);

    void removeSpectator(string connectionId);
    void onKeyDown(string connectionId, int direction);
    void addSpectator(IUser user);
    void removePlayer(string connectionId);

    IGameData gameData{get;}
    IUpdateData updateData{get;}
    void gameLoop(double diff);
    void newLevel();
    void restart();
  }
}