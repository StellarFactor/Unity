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

        public bool AcquireItem(IAcquirable item)
        {
            Debug.Log($"{name} trying to obtain {item}", item as UnityEngine.Object);
            Type itemType = item.GetType();

            if (item is Component c)
            {
                c.gameObject.SetActive(false);
            }

            if (!ContainsItem(item) || item.CanStack)
            {
                itemsByType[itemType] ??= new();
                itemsByType[itemType].Add(item);
                item.OnAcquired(this);
                return true;
            }

            return false;
        }

        public bool RemoveItem(IAcquirable item)
        {
            return RemoveItem(item, defaultDropPoint.position, defaultDropPoint.localEulerAngles);
        }

        public bool RemoveItem(IAcquirable item, Vector3 dropPos, Vector3 dropEulers)
        {
            if (ContainsItem(item)
                && itemsByType[item.GetType()].Remove(item))
            {
                item.OnRemoved(this, dropPos, dropEulers);

                if (item is Component c)
                {
                    c.gameObject.SetActive(true);
                }

                return true;
            }

            return false;
        }

        public bool ContainsItem(IAcquirable toCheck)
        {
            Type itemType = toCheck.GetType();

            if (!itemsByType.ContainsKey(itemType))
            {
                itemsByType[itemType] = new();
            }

            return itemsByType[itemType].Contains(toCheck);
        }

        public int StackSize(Type type)
        {
            if (!itemsByType.Keys.Contains(type)
                || itemsByType[type] == null)
            {
                return 0;
            }

            return itemsByType[type].Count;
        }

        ///<summary>
        /// Returns an empty list if no items of the type are found in this inventory
        ///</summary>
        public List<IAcquirable> GetCurrentItemsOfType(Type type)
        {
            // If the dict contains a key for this type,
            return itemsByType.ContainsKey(type)
                ? itemsByType[type] ?? new()    // return said list (if not null) or an empty list.
                : new();                        // if no key for this type, return an empty list.
        }

        public bool ContainsItem(IAcquirable toCheck)
        {
            return allItems.Contains(toCheck);
        }

        public bool ContainsArtifact(string artifactName)
        {
            List<Artifact> artifacts = allItems.Cast<Artifact>()
                .Where(item => item.ArtifactName == artifactName).ToList();

            return artifacts != null && artifacts.Count > 0 && artifacts[0] != null;
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
