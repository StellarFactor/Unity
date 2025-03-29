using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StellarFactor
{
    public class ArtifactInventoryUI : Singleton<ArtifactInventoryUI>
    {
        [SerializeField] private List<ArtifactInventorySlot> artifactSlots;

        private Dictionary<string, ArtifactInventorySlot> slotDictionary = new();

        [field: SerializeField] public HighlightableColorSet EmptyColorSet { get; private set; }
        [field: SerializeField] public HighlightableColorSet FullColorSet { get; private set; }
        [field: SerializeField] public HighlightableColorSet FullColorSelectedSet { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InitSlotDictionary();
        }

        public void FillArtifactSlot(Artifact artifact)
        {
            string key = artifact.ArtifactName;

            if (!slotDictionary.ContainsKey(key))
            {
                Debug.LogError(
                    $"The slot dictionary does not contain the key {key}. " +
                    $"Make sure everything is spelled correctly, and that " +
                    $"the list in the inspector has the correct number of " +
                    $"ArtifactInventorySlots",
                    this);
                return;
            }

            if (slotDictionary[key].Artifact != null)
            {
                Debug.LogError(
                    $"The slot {slotDictionary[key]} already contains " +
                    $"{slotDictionary[key].Artifact.name}" +
                    $"Make sure everything is spelled correctly, and that " +
                    $"the list in the inspector has the correct number of " +
                    $"ArtifactInventorySlots",
                    slotDictionary[key]);
                return;
            }

            artifact.gameObject.SetActive(false);
            slotDictionary[key].FillWith(artifact);
        }

        public void EmptyArtifactSlot(Artifact artifact)
        {
            string key = artifact.ArtifactName;

            if (!slotDictionary.ContainsKey(key))
            {
                Debug.LogError(
                    $"The slot dictionary does not contain the key {key}. " +
                    $"Make sure everything is spelled correctly, and that " +
                    $"the list in the inspector has the correct number of " +
                    $"ArtifactInventorySlots",
                    this);
                return;
            }

            if (slotDictionary[key].Artifact != artifact)
            {
                Debug.LogError(
                    $"The slot {slotDictionary[key]} already contains " +
                    $"{slotDictionary[key].Artifact.name}" +
                    $"Make sure everything is spelled correctly, and that " +
                    $"the list in the inspector has the correct number of " +
                    $"ArtifactInventorySlots",
                    slotDictionary[key]);
                return;
            }

            Artifact removedArtifact = slotDictionary[key].Empty();
            removedArtifact.gameObject.SetActive(true);
            removedArtifact.transform.SetParent(null);
        }

        private void InitSlotDictionary()
        {
            foreach (ArtifactInventorySlot slot in artifactSlots)
            {
                if (!slotDictionary.TryAdd(slot.MatchingArtifactName, slot))
                {
                    Debug.LogWarning(
                        $"Something went wrong trying to add an Artifact Slot to " +
                        $"{name}'s Dictionary<string, ArtifactInventorySlot...",
                        this);
                }
            }
        }
    }
}