namespace squario.library{
  public class PlayerData : IPlayerData
  {
    public PlayerData(){}

    public PlayerData(IPlayer player){
      this.colour = player.colour;
      this.squareIndex = player.squareIndex;
      this.button1 = player.button1;
      this.button2 = player.button2;
    }
    public string colour { get; set; }
    public int squareIndex { get; set; }
    public Direction button1 { get; set; }
    public Direction button2 { get; set; }
  }
}