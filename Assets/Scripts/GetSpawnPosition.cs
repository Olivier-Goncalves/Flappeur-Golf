using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetSpawnPosition : MonoBehaviour
{
    private void Awake() => Spawns.spawns[(int)Char.GetNumericValue(transform.name[transform.name.Length - 1]) - 1] = transform.position;
}
