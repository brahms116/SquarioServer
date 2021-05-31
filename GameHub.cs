using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using squario.services;
namespace squario{
  public class GameHub:Hub{

    private IRoomService _roomService;
    public GameHub(IRoomService service){
      _roomService=service;
    }
    public override async Task OnConnectedAsync(){
      await Clients.Client(Context.ConnectionId).SendAsync("id",Context.ConnectionId);
      await base.OnConnectedAsync();
    }

    public void CreateRoom(string connectionId, string username){
       _roomService.CreateRoom(connectionId,username);
      
    }

    public void JoinRoom(string connectionId, string roomName, string username){
      _roomService.JoinRoom(connectionId,roomName,username,false);
    }

    public void JoinAsSpectator(string connectionId, string roomName, string username){
      _roomService.JoinRoom(connectionId,roomName,username,true);
    }

    public void ChangeDirection(string connectionId, int direction){
      _roomService.KeyDown(connectionId,direction);
    }

    public void PlayerList(string connectionId){
      _roomService.PlayerList(connectionId);
    }

    public void StartGame(string connectionId){
      _roomService.StartGame(connectionId);
    }

    public override async Task OnDisconnectedAsync(Exception exception){
      _roomService.LeaveRoom(Context.ConnectionId);
      await base.OnDisconnectedAsync(exception);
    }
  }
}