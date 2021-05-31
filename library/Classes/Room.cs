using System;
using System.Collections.Generic;
using System.Linq;
namespace squario.library{

  
  public class Room:IRoom{

    private Random _random = new Random();

    private IGame _game;
    public List<IPlayer> players{get;private set;} = new List<IPlayer>();

    public List<IUser> spectators{get;private set;} = new List<IUser>();

    public IPlayer host {get; private set;}
    public string name{get;set;}
    public GameState gameState{get;set;}

    public IGameData gameData{get=>Factory.CreateGameData(_game.levelData,players);}

    public IUpdateData updateData{get{
      return Factory.CreateUpdateData(_game.squareLocs,players,_game.finishedSquares);
    }}

    public void gameLoop(double diff){
      void gameOver(string msg){
        gameState = GameState.GameLose;
      }
      void gameWin(){
        gameState = GameState.GameWin;
      }
      _game.squareLocs.ForEach(square=>{

        int scale = 1000;
        //movement 
        switch ((int)square.direction)
        {
            case 1:{
              square.y-=(int)Math.Floor(Constants.squareSpeed*diff*scale);
              break;
            }
            case 2:{
              square.x+=(int)Math.Floor(Constants.squareSpeed*diff*scale);
              break;
            }
            case 3:{
              square.y+=(int)Math.Floor(Constants.squareSpeed*diff*scale);
              break;
            }
            case 4:{
              square.x-=(int)Math.Floor(Constants.squareSpeed*diff*scale);
              break;
            }
            default:{break;}
        }
        //check boundary collision
        if(Utils.checkBoundaryCollision(square)) {
          gameOver("A square fell out of the world!");
          return;
        }
        //check wall collision
        int index = _game.levelData.wallLocs.FindIndex(wall=>Utils.checkEntityCollision(wall,square));
        if(index!=-1){
          gameOver("A square collided with a wall!");
          return;
        }
        //check square collision
        //INEFFICIENT, finding index
        index = _game.squareLocs.FindIndex(e=>square.colour==e.colour);
        int index2 = _game.squareLocs.FindIndex(e=>Utils.checkEntityCollision(e,square));
        if(index2!=-1&&index2!=index){
          gameOver("A square collided with another square!");
          return;
        }

        //check sqaure complete, i still have Index
        if(Utils.checkSquareFinished(square,_game.levelData.squareFinishLocs[index])){
          _game.squareLocs.RemoveAt(index);
          _game.finishedSquares.Add(square);
          //game win if no squares left in squarelocs
          if(!_game.squareLocs.Any()){
            gameWin();
          }
        }
        
      });
      
    }

    public void restart(){
      _game.squareLocs = _game.levelData.squareStartLocs;
    }
    public Room(string roomName){
      name=roomName;
      
      gameState = GameState.Lobby;
    }
    public bool isEmpty{get{return !players.Any();}}
    public bool hasSpectators{get{return spectators.Any();}}

    public void newLevel(){
      _game = Factory.CreateGame(2);

      if(players.Count<4){
        List<int> sequence = Utils.scrambleList(new List<int>(){0,1});
        for(int i=0;i<players.Count;i++){
          int mode = sequence[i%2];
          players[i].button1 = mode==0? (Direction) 1: (Direction )2;
          players[i].button2 = mode==0? (Direction) 3: (Direction )4;
          players[i].colour = Constants.playerColours[i];
        }
      }
      else{
        List<int> sequence = Utils.scrambleList(new List<int>(){0,1,2,3});
        for(int i=0;i<players.Count;i++){
          players[i].button1 =(Direction)sequence[(i%4+1)];
          players[i].colour = Constants.playerColours[i];
        }
      }
    }

    
    public void addSpectator(IUser user){
      spectators.Add(user);
    }


    public void addPlayer(IUser user){
        IPlayer player = Factory.PlayerFromUser(user);
        if(isEmpty){
          host= player;
        }
        players.Add(player);
    }

    public void removeSpectator(string connectionId){
      int index = spectators.FindIndex(d=>d.connectionId==connectionId);
      spectators.RemoveAt(index);
    }
    
    public void onKeyDown(string connectionId, int direction){
      int index = players.FindIndex(e=>e.connectionId==connectionId);
      if(index==-1) return;
      _game.squareLocs[players[index].squareIndex].direction = (Direction) direction;
    }

    public void removePlayer(string connectionId){
      int index = players.FindIndex(d=>d.connectionId==connectionId);
      bool isHost = connectionId==host.connectionId;
      players.RemoveAt(index);
      if( isHost && !isEmpty){
        host = players[0];
      }
    }
  }
}