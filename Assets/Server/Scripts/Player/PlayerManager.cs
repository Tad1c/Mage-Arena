using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager
{

    public delegate void SetId(int id);

    public static event SetId OnSetId;

    public static void SendId(int id)
    {
        OnSetId?.Invoke(id);
    }

}
