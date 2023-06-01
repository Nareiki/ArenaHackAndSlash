using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpable
{
    public bool JumpUsed { get; }
    public void OnJumpPressed();
}
