using GWLPXL.Movement.Character.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookingPoint : MonoBehaviour
{
    public Transform Player;
    public FollowVars Vars;
    Follow follow;
    private void Start()
    {
        follow = new Follow(this.transform, Player, Vars);
    }
    private void OnDestroy()
    {
        follow.RemoveTicker();
    }
}
