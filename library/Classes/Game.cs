using System.Collections.Generic;
using System;
namespace squario.library{
  public class Game:IGame{

    public ILevel levelData{get;set;}
    public List<IDynamicSquare> squareLocs{get;set;} = new List<IDynamicSquare>();

    // private Random _random;

    public List<ISquare> finishedSquares {get;set;} = new List<ISquare>();

    public Game(int numSquare){

      
      List<ISquare> finishLocs = new List<ISquare>();
      // generate squares and finisheLocs
      for(int i =0;i<numSquare;i++){
        
        List<IEntity> entities = new List<IEntity>();
        entities.AddRange(Utils.reduceToEntityList(squareLocs));
        entities.AddRange(Utils.reduceToEntityList(finishLocs));
        
        //generate square
        Utils.generateSquare<IDynamicSquare>(Constants.sqaureSize,Constants.squareColours[i],entities,squareLocs);

        //generate squareFinish
        Utils.generateSquare<ISquare>(Constants.finishSquareSize,Constants.squareColours[i],entities,finishLocs);

      }


      //TODO: Generate Walls
      levelData = Factory.CreateLevel(new List<IEntity>(),finishLocs,squareLocs);

      
    }

    
  }
}