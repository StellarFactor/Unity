using StellarFactor;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactInventorySlot : MonoBehaviour, IFillable<Artifact>
{
    [SerializeField] Image background;
    [SerializeField] SelectableSprite spr;
    [field: SerializeField] public string MatchingArtifactName { get; private set; }
    public Artifact Artifact { get; private set; }

    public void FillWith(Artifact artifact)
    {
        Artifact = artifact;
    }

    public Artifact Empty()
    {
        Artifact toEmpty = Artifact;
        Artifact = null;
        return toEmpty;
    }
}
