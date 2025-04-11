using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine;

namespace StellarFactor
{
    [RequireComponent(typeof(PlayerControl))]
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Transform defaultDropPoint;

        private Dictionary<Type, List<IAcquirable>> itemsByType = new();

        public event Action<IAcquirable> ItemAdded;
        public event Action<IAcquirable> ItemRemoved;

        public void AquireItem(IAcquirable item)
        {
            Debug.LogWarning($"AcquireItem called on a {item.GetType()} item");
            Type itemType = item.GetType();

            // If the list is null, create a new one,
            // otherwise, use the existing one.
            if (itemsByType.ContainsKey(itemType))
            {
                itemsByType[itemType] ??= new();
            }
            else
            {
                itemsByType[itemType] = new();
            }

            // Add the item to the list of items of like items.
            itemsByType[itemType].Add(item);

            if (item is Component c)
            {
                Debug.Log("Recognized as Component");
                c.gameObject.SetActive(false);

                if (c is Artifact artifact)
                {
                    Debug.LogWarning("Regognized as Artifact");
                    Assert.IsNotNull(ArtifactInventoryUI.MGR);
                    ArtifactInventoryUI.MGR.FillArtifactSlot(artifact);
                }
            }
        }

        public bool RemoveItem(IAcquirable item)
        {
            Type itemType = item.GetType();

            // If the list is null, create a new one,
            // otherwise, use the existing one.
            if (itemsByType.ContainsKey(itemType))
            {
                itemsByType[itemType] ??= new();
            }
            else
            {
                itemsByType[itemType] = new();
            }

            return itemsByType[itemType].Remove(item);
        }

        public bool RemoveItem<T>(T item) where T : Component, IAcquirable
        {
            return RemoveItem(item, defaultDropPoint.localPosition, false);
        }

        public bool RemoveItem<T>(T item, Vector3 dropPoint, bool worldSpace) where T : Component, IAcquirable
        {
            if (RemoveItem(item))
            {
                Drop(item, dropPoint, worldSpace);
                return true;
            }

            return false;
        }

        public bool ContainsItem(IAcquirable toCheck)
        {
            foreach(Type itemType in itemsByType.Keys)
            {
                if (itemsByType[itemType] == null || !itemsByType[itemType].Contains(toCheck))
                    return false;
            }
            return true;
        }

        public List<IAcquirable> GetCurrentItemsOfType(Type type)
        {
            return itemsByType[type] ?? new();
        }

        private void Drop<T>(T item, Vector3 dropPoint, bool worldSpace) where T : Component, IAcquirable
        {
                item.gameObject.SetActive(true);

                if (worldSpace)
                {
                    item.transform.position = dropPoint;
                }
                else
                {
                    item.transform.localPosition = dropPoint;
                }
        }

        protected virtual void OnItemAdded(IAcquirable item)
        {
            ItemAdded.Invoke(item);
        }

        protected virtual void OnItemRemoved(IAcquirable item)
        {
            ItemRemoved.Invoke(item);
        }

        private void PrintArtifactListDebug()
        {
            if (itemsByType[typeof(Artifact)] == null)
                return;

            List<string> artifactNames = new()
            {
                "New artifact list:"
            };
            foreach (Artifact a in itemsByType[typeof(Artifact)])
            {
                artifactNames.Add(a.ArtifactName);
            }

            Debug.Log(string.Join("\n", artifactNames));
        }
    }
}
