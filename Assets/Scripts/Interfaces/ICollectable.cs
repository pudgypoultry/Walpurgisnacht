using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    GameObject gameObject { get; }
    Transform transform { get; }

    void CollectMe(PlayerController character);

}
