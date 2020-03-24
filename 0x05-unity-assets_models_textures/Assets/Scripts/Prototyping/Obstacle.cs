using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class Obstacle
{

  private bool _active;

  protected Obstacle(string kind, bool active = false)
  {
    Kind = kind;
    Active = active;
  }
  
  public string Kind { get; }

  public bool Active
  {
    get => _active;
    set => _active = value;
  }

  void ApplyEffect(ref GameObject target)
  {
    if (!_active)
      return;
  }
  
  
}



