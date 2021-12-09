using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerInfo
{
    /// <summary>
    /// The player index.
    /// </summary>
    public int ID => Input.playerIndex;

    /// <summary>
    /// The input devices.
    /// </summary>
    public InputDevice[] Devices => Input.devices.ToArray();

    /// <summary>
    /// The main input device.
    /// </summary>
    public InputDevice MainDevice => Input.devices.Count() > 0 ? Input.devices.First() : null;

    /// <summary>
    /// The player input component.
    /// </summary>
    public PlayerInput Input;

    /// <summary>
    /// The player input game object.
    /// </summary>
    public GameObject GameObject;
}