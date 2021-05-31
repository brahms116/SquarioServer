using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using squario.library;
using System;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using System.Timers;
using System.Text.Json;
namespace squario.services{

  public interface IRoomService
  {
    void CreateRoom(string connectionId, string username);
    void JoinRoom(string connectionId, string roomName, string username, bool isSpectator);

    void PlayerList(string connectionId);
    void LeaveRoom(string connectionId);
    void KeyDown(string connectionId, int direction);
    void StartGame(string connectionId);

  }
  public class RoomService:IRoomService{

    private Random _random = new Random();
    
    private List<IRoom> _roomList = new List<IRoom>();
    private List<IUser> _userList = new List<IUser>();

    private System.Timers.Timer _timer;
    private DateTime _lastLoop = DateTime.Now;

    private readonly IHubContext<GameHub> _hubContext;

    private void _gameLoop(Object source, ElapsedEventArgs e){
      TimeSpan diff = DateTime.Now - e.SignalTime;
      Parallel.ForEach(_roomList,room=>{
        if(room.gameState!=GameState.Game) return;
        room.gameLoop(diff.Duration().TotalMilliseconds);
        Parallel.ForEach(room.players,player=>{
          _hubContext.Clients.Client(player.connectionId).SendAsync("onGameData", room.updateData);
        });
        if(room.hasSpectators){
          Parallel.ForEach(room.spectators, user=>{
            _hubContext.Clients.Client(user.connectionId).SendAsync("onGameData", room.updateData);
          });
        }
        
      });
    }


    public RoomService(IHubContext<GameHub> hubContext){
      _hubContext = hubContext;
      _timer = new System.Timers.Timer(25);
      _timer.Elapsed += _gameLoop;
      _timer.AutoReset = true;
      _timer.Enabled = true;
    }

    private void _sendError(string msg, string connectionId){
      _hubContext.Clients.Client(connectionId).SendAsync("onError",msg);
    }
    public void PlayerList(string connectionId){
      IUser user = _userList.Where(u=>u.connectionId==connectionId).FirstOrDefault();
      if(user==null){
        _sendError("User Not Found",connectionId);
        return;
      }
      int index = _roomList.FindIndex(d=>d.name==user.room);
      if(index==-1){
        if(string.IsNullOrWhiteSpace(user.room)){
          _sendError("Not in room",connectionId);
          return;
        }
        _hubContext.Clients.Client(connectionId).SendAsync("onPlayerList",new List<object>());
        return;
      }
      List<IPlayer> players = _roomList[index].players;
      IPlayer host = _roomList[index].host;
      _hubContext.Clients.Client(connectionId).SendAsync("onPlayerList",new {players,host});
    }
    public void StartGame(string connectionId){
      int index = _roomList.FindIndex(d=>d.host.connectionId==connectionId);
      if(index==-1){
        _sendError("Host Not Found",connectionId);
        return;
      }
      if(_roomList[index].gameState!=GameState.Lobby){
        _sendError("Not in lobby",connectionId);
        return;
      }
      if(_roomList[index].players.Count<2){
        _sendError("Not enough players",connectionId);
        return;
      }

      
      _roomList[index].newLevel();
      _roomList[index].gameState = GameState.Game;
      Parallel.ForEach(_roomList[index].players,player=>{
        _hubContext.Clients.Client(player.connectionId).SendAsync("onGameStart", _roomList[index].gameData);
      });
      if(_roomList[index].hasSpectators){
        Parallel.ForEach(_roomList[index].spectators, user=>{
          _hubContext.Clients.Client(user.connectionId).SendAsync("onGameStart", _roomList[index].gameData);
        });
      }

    }

    public void KeyDown(string connectionId, int direction){
      if(direction<1 || direction>4){
        _sendError("Invalid Key",connectionId);
        return;
      }
      //duplicate code with leaveroom
      IUser user = _userList.Where(user=>user.connectionId==connectionId).FirstOrDefault();
      if(user==null) {
        _sendError("User Not Found",connectionId);
        return;
      };
      int index = _roomList.FindIndex(room=>room.name==user.room);
      if(index==-1) {
        _sendError("Room Not Found",connectionId);
        return;
      };;
      
      _roomList[index].onKeyDown(connectionId,direction);

    }

    public void CreateRoom(string connectionId, string username){
      string roomName="";
      char offset = 'A';
      bool isValid = false;
      while(!isValid){
        StringBuilder builder = new StringBuilder();
        for(int i=0;i<5;i++){
          builder.Append((char) _random.Next(offset,offset+26));
        }
        roomName = builder.ToString();
        if(!_roomList.Where(room=>room.name==roomName).Any()){
          isValid=true;
        }
      }
      IRoom room = Factory.CreateRoom(roomName);
      IUser user = Factory.CreateUser(username,connectionId,roomName,false);
      room.addPlayer(user);
      _userList.Add(user);
      _roomList.Add(room);
      _hubContext.Clients.Client(connectionId).SendAsync("onRoomCreated",roomName);
    }

    public void JoinRoom(string connectionId, string roomName, string username, bool isSpectator){
      int index = _roomList.FindIndex(room=>room.name==roomName);
      if(index!=-1){
        bool doesUserExist = _roomList[index].players.Where(d=>d.connectionId==connectionId).Any();
        if(doesUserExist) {
          _sendError("You are already part of a room",connectionId);
          return;
        }
        if(_roomList[index].players.Count==Constants.maxPlayers){
          _sendError("Room Full",connectionId);
          return;
        }
        if(_roomList[index].gameState!=GameState.Lobby){
          _sendError("Game In Progress",connectionId);
          return;
        }
        IUser user = Factory.CreateUser(username,connectionId,roomName,isSpectator);
        _userList.Add(user);
        if(!isSpectator){
          _roomList[index].addPlayer(user);
          Parallel.ForEach(_roomList[index].players, player=>{
            if(player.connectionId==connectionId){
              _hubContext.Clients.Client(connectionId).SendAsync("onRoomJoined",roomName);
            }else{
              _hubContext.Clients.Client(player.connectionId).SendAsync("onPlayerJoined", $"{username} Joined");
            }
          });
          if(_roomList[index].hasSpectators){
            Parallel.ForEach(_roomList[index].spectators, user=>{
              _hubContext.Clients.Client(user.connectionId).SendAsync("onPlayerJoined", $"{username} Joined");
            });
          }
        }else{
          _roomList[index].addSpectator(user);
          _hubContext.Clients.Client(user.connectionId).SendAsync("onRoomJoined",roomName);
        }
      }
    }
    public void LeaveRoom(string connectionId){
      IUser user = _userList.Where(user=>user.connectionId==connectionId).FirstOrDefault();
      if(user==null) {
        _sendError("User Not Found",connectionId);
        return;
      };
      int index = _roomList.FindIndex(room=>room.name==user.room);
      if(index==-1) {
        _sendError("Room Not Found",connectionId);
        return;
      };;
      if(user.isSpectator){
        _roomList[index].removeSpectator(connectionId);
      }else{
        _roomList[index].removePlayer(connectionId);
        Parallel.ForEach(_roomList[index].players,player=>{
          _hubContext.Clients.Client(player.connectionId).SendAsync("onPlayerLeave",$"{user.name} has left");
        });
        if(_roomList[index].hasSpectators){
          Parallel.ForEach(_roomList[index].spectators, user=>{
              _hubContext.Clients.Client(user.connectionId).SendAsync("onPlayerLeave",$"{user.name} has left");
          });
        }
        if(_roomList[index].isEmpty){
          Parallel.ForEach(_roomList[index].spectators, user=>{
            _hubContext.Clients.Client(user.connectionId).SendAsync("onEmptyRoom","All Lonely");
          });
          _roomList.RemoveAt(index);
        }
      }

    }
  }
}