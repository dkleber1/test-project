//Author: Dana Kleber
//FileName: Queue.cs
//Project Name: PASS4
//Creation Date: Dec 28 2020
//Modified Date: Jan 27 2021
//Description: create a queue object to create queues to utilize in the game when the user adds their commands to a queue

using System;
using System.Collections.Generic;

class GameQueue
{
  // store the user directions as a list of strings
  private List<string> dir = new List<string>();

  public GameQueue()
  {

  }  
 
  // pre : the action the user enters as a string
  // post : none
  // desc: adds to a queue
  public void Enqueue(string action)
  {
    //adds the direction to the queue
    dir.Add(action);       
  }
 
  // pre : none
  // post : the beggining first action the user enters as a string
  // desc: removes/takes off and returns the first item at the beggining of the queue 
  public string Dequeue()
  {
    // store the users direction of command
    string direction;

    // assigns as the beggining value and removes it from the queue
    direction = dir[0];
    dir.RemoveAt(0);
    
    // returns the value back to the user
    return direction;
  }
 
  // pre : none
  // post : the beggining first action the user enters as a string
  // desc: onlu returns the first item at the beggining of the queue
  public string Peek()
  {
    // return the beggining first action the user enters as a string
    return dir[0];
  }
 
  // pre : none
  // post : the size of the queue as an integer
  // desc: returns size of the queue as an integer
  public int Size()
  {
    // return size of the queue as an integer
    return dir.Count;
  }
 
  // pre : none
  // post : if the queue is empty stored as a boolean variable
  // desc: returns boolean variable stating if queue is empty
  public bool IsEmpty()
  {
    // returns boolean variable stating if queue is empty
    return dir.Count == 0;
  }
}