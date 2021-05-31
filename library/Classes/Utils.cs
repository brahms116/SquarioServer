using System.Collections.Generic;
using System;
namespace squario.library{
  public static class Utils{

    private static readonly Random _random = new Random();
    public static bool checkEntityCollision(IEntity obj1, IEntity obj2){
      bool xOverlap = obj2.x>=obj1.x&&obj2.x<=obj1.x+obj1.width || obj2.x+obj2.width>=obj1.x&&obj2.x+obj2.width<=obj1.x+obj1.width;

      bool yOverlap = obj2.y>=obj1.y&&obj2.y<=obj1.y+obj1.height || obj2.y+obj2.height>=obj1.y&&obj2.y+obj2.height<=obj1.y+obj1.height;

      return xOverlap && yOverlap;

    }

    public static bool checkBoundaryCollision(IEntity obj){
      bool xOutOfBounds = obj.x<0 || obj.x+obj.width > Constants.gameWidth;
      bool yOutOfBounds = obj.y<0 || obj.y+obj.height > Constants.gameHeight;
      return xOutOfBounds || yOutOfBounds;
    }

    public static List<int> scrambleList(List<int> arr){
      //shallow copy
      List<int> temp = new List<int>(arr);
      List<int> result = new List<int>();
      while(temp.Count!=0){
        int index = _random.Next(0,temp.Count-1);
        result.Add(temp[index]);
        temp.RemoveAt(index);
      }
      return result;
    }

    public static List<IEntity> reduceToEntityList<T>(List<T> arr) where T:IEntity{
      List<IEntity> temp = new List<IEntity>();
      arr.ForEach(d=>{
        temp.Add(d);
      });
      return temp;
    }

    public static bool checkSquareFinished(IDynamicSquare square, ISquare finish){
      bool isX = square.x>=finish.x&&square.x+square.width<=finish.x+finish.width;
      bool isY = square.y>=finish.y&&square.y+square.height<=finish.y+finish.height;
      return isX && isY;

    }


    public static void generateSquare<T> (int size, string colour, List<IEntity> existingEntities, List<T> output) where T:ISquare{
          bool isValid = false;
          while(!isValid){
            int genX = _random.Next(0,Constants.gameWidth-size);
            int genY = _random.Next(0,Constants.gameHeight-size);

            //ughh please fix this, its horrible
            ISquare square = Factory.CreateDynamicSquare(genX,genY,size,colour) ;
            int index = existingEntities.FindIndex(d=>Utils.checkEntityCollision(d,square));
            if(index==-1){
              output.Add((T)square);
              isValid = true;
            }
          }
    }
  }
}