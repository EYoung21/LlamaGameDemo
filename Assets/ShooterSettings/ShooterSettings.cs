using UnityEngine;

public enum ShooterAttack
{
    Solo,
    Tripple,
    Circle
}

[CreateAssetMenu(fileName = "ShooterSettings", menuName = "Scriptable Objects/ShooterSettings")]
public class ShooterSettings : ScriptableObject
{
    public ShooterAttack attack;
    public float cooldown = 3;
    public Color color = Color.white;
    public float size = 1;
    public bool follow = false;
    public bool rotate = true;
    public float speed = 1;
    public float followRadius = 2f;
    public int hp = 10;
}
