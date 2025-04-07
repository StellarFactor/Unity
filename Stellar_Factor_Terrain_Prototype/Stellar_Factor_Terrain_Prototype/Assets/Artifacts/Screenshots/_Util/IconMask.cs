using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact Icon Mask", menuName = "UI/Artifact Icon Mask")]
public class IconMask : ScriptableObject
{
    [field: SerializeField] public Sprite iconSprite { get; private set; }
    [field: SerializeField] public Material maskMat { get; private set; }
}
