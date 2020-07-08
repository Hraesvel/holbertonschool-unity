using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBehavior 
{
    public struct HeldData
    {
        public HeldType Type;
        public Vector3 OffsetPosition;
        public Quaternion OffsetRotation;
    }
    
}


/// <summary>
/// types of grids categorized by object primary shape or type 
/// </summary>
public enum HeldType
{
    Cube,
    Sphere,
    Book,
    Pipe,
    ChessPiece,
    Flashlight,
}