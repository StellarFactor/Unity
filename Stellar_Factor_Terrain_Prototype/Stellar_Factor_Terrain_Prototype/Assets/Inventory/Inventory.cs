using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

namespace StellarFactor
{
    [RequireComponent(typeof(PlayerControl))]
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Transform dropPoint;

        private List<IAcquirable> allItems = new();

        // Return a copy of the list, rather than a ref to the list.
        public List<Artifact> ArtifactsAcquired => allItems
            .Where(item => item is Artifact).ToList()
            .ConvertAll(item => item as Artifact);

        public void AquireItem(IAcquirable item)
        {
            if (allItems.Contains(item)) { return; }

            if (item is Component c)
            {
                c.gameObject.SetActive(false);

                if (c is Artifact artifact)
                {
                    Assert.IsNotNull(ArtifactInventoryUI.MGR);
                    ArtifactInventoryUI.MGR.FillArtifactSlot(artifact);
                }
            }
            allItems.Add(item);
        }

        public void RemoveItem(IAcquirable item)
        {
            if (!allItems.Contains(item)) { return; }

            if (item is Artifact artifact)
            {
                Artifact removed = ArtifactInventoryUI.MGR.EmptyArtifactSlot(artifact);
                removed.transform.position = dropPoint.position;
            }
            allItems.Remove(item);
        }

        private void PrintArtifactListDebug()
        {
            List<string> artifactNames = new()
        {
            "New artifact list:"
        };
            foreach (Artifact a in ArtifactsAcquired)
            {
                artifactNames.Add(a.ArtifactName);
            }
            Debug.Log(string.Join("\n", artifactNames));
        }
    }
}