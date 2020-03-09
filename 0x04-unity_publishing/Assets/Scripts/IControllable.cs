using UnityEngine;
using UnityEngine.UI;


public interface IControllable : IKeyControllable, ITouchControllable
{
}

public interface ITouchControllable {
    Image StartImage { get; set; }

    Image ToImage { get; set; }
    Vector2 StartPos { get; set; }
    Vector2 ToPos { get; set; }
    float Sensitivity { get; set; }
    bool TouchController(ref Vector3 dir);

}

public interface IKeyControllable
{
    bool KeyController(ref Vector3 dir);
    
}
