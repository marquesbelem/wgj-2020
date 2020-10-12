using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GroundMaterial : MonoBehaviour {
    
    public enum Type {
        Grass,
        Stone
    }
    public Type type;

}
