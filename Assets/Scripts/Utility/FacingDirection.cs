using UnityEngine;

public static class FacingDirection {
    public static Vector3 Up { get; } = new Vector3(0, 0);
    public static Vector3 Down { get; } = new Vector3(0, 180);
    public static Vector3 Left { get; } = new Vector3(0, -90);
    public static Vector3 Right { get; } = new Vector3(0, 90);
}
