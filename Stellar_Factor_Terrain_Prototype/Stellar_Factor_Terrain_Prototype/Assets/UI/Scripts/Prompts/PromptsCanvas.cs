using StellarFactor;
using UnityEngine;

public class PromptsCanvas : MonoBehaviour
{
    [field: SerializeField] public PromptWindow InteractionPromptWindow { get; private set; }
    [field: SerializeField] public PromptWindow PausePromptWindow { get; private set; }
}
