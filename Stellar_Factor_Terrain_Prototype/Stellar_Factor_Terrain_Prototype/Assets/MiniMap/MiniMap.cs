using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overtown
{
    public class MiniMap : Singleton<MiniMap>
    {
        [Header("Debug")]
        [SerializeField] Logger log;

        [Header("Camera")]
        [SerializeField] private Camera cam;

        [Header("UI")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image background;
        [SerializeField] private RawImage map;

        [Header("Prefabs")]
        [SerializeField] private GameObject unvisitedPrefab;
        [SerializeField] private GameObject visitedPrefab;

        private List<MiniMapLocation> locations = new();
        private Dictionary<MiniMapLocation, GameObject> activeNodes = new();
        private Dictionary<MiniMapLocation, Vector2> nodePositions = new();



        private void Update()
        {
            if (locations.Count <= 0) { return; }

            //updateNodePositions();
            StartCoroutine(updateNodes());
        }

        private void LateUpdate()
        {
            //applyNodePositions();
        }

        private IEnumerator updateNodes()
        {
            applyNodePositions();

            yield return new WaitForEndOfFrame();
            updateNodePositions();

            yield return null;
        }

        private void applyNodePositions()
        {
            foreach (MiniMapLocation loc in activeNodes.Keys)
            {
                GameObject node = activeNodes[loc];

                log.Print($"Setting {node}'s position to {nodePositions[loc]}");
                node.GetComponent<RectTransform>().anchoredPosition = nodePositions[loc];
            }
        }

        private void updateNodePositions()
        {
            for (int i = 0; i < locations.Count; i++)
            {
                MiniMapLocation loc = locations[i];

                if (!activeNodes.ContainsKey(loc))
                {
                    log.Throw($"NODE ERROR. Failed to find {loc.gameObject.name}'s node");
                    return;
                }

                Vector2 nodeScreenPos = cam.WorldToScreenPoint(loc.transform.position);
                nodePositions[loc] = nodeScreenPos;
            }
        }

        private void spawnNode(MiniMapLocation location)
        {
            if (locations.Contains(location))
            {
                return;
            }

            locations.Add(location);

            //GameObject prefab = location.BeenVisited ? visitedPrefab : unvisitedPrefab;

            GameObject prefab = unvisitedPrefab;

            GameObject newNodeObj = Instantiate(prefab, map.transform);
            activeNodes[location] = newNodeObj;

            Vector2 nodeScreenPos = cam.WorldToScreenPoint(location.transform.position);
            nodePositions[location] = nodeScreenPos;
        }

        private void despawnNode(MiniMapLocation location)
        {
            if (!locations.Contains(location))
            {
                return;
            }

            locations.Remove(location);

            GameObject node = activeNodes[location];

            activeNodes.Remove(location);
            nodePositions.Remove(location);

            Destroy(node);
        }

        private void forceSpawnNode(MiniMapLocation location)
        {
            if (locations.Contains(location))
            {
                despawnNode(location);
            }

            spawnNode(location);
        }

        public void DisplayNodeFor(MiniMapLocation location)
        {
            forceSpawnNode(location);
        }

        public void HideNodeFor(MiniMapLocation location)
        {
            despawnNode(location);
        }
    }
}
