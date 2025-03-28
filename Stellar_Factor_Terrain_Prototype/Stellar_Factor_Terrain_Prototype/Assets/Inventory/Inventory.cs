using StellarFactor;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<IAcquirable> allItems = new();
    private List<Artifact> artifacts = new();

    // Return a copy of the list, rather than a ref to the list.
    public List<Artifact> ArtifactsAquired => new(artifacts);

    public void AquireItem(IAcquirable item)
    {
        if (allItems.Contains(item)) { return; }

        if (item is Component c)
        {
            c.gameObject.SetActive(false);

            if (c is Artifact artifact
                && !artifacts.Contains(artifact))
            {
                artifacts.Add(artifact);


                List<string> artifactNames = new();
                foreach (Artifact a in artifacts)
                {
                    artifactNames.Add(a.GetFillData().name);
                }
                Debug.Log($"{name} added {artifact.name} to its inventory.\n" +
                    $"New artifact list:\n" +
                    $"{string.Join("\n", artifactNames)}");
                // add to slot;
            }
        }
        allItems.Add(item);
    }

    public void RemoveItem(IAcquirable item)
    {
        if (!allItems.Contains(item)) { return; }

        allItems.Remove(item);
    }
}
