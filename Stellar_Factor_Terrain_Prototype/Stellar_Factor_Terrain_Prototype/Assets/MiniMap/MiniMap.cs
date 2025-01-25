using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StellarFactor.Minimap
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

        private Dictionary<IMapLocation, RectTransform> activeNodes = new();
        private List<IMapLocation> staleNodes = new();

        // =============================
        #region Unity
        // =============================
        private void Update()
        {
            if (activeNodes.Count <= 0) { return; }

            UpdateStaleNodes();
            UpdateNodePositions();
        }

        #endregion Unity

        public void AddNodeFor(IMapLocation location)
        {
            if (activeNodes.ContainsKey(location))
            {
                RemoveNodeFor(location);
            }

            RectTransform prefab = location.GetNodeToDisplay();

            activeNodes[location] = Instantiate(prefab, map.transform);
        }

        public void RemoveNodeFor(IMapLocation location)
        {
            if (!activeNodes.ContainsKey(location))
            {
                return;
            }

            GameObject nodeToDestroy = activeNodes[location].gameObject;

            activeNodes.Remove(location);

            Destroy(nodeToDestroy);
        }

        private void UpdateStaleNodes()
        {
            foreach(IMapLocation location in staleNodes)
            {
                RemoveNodeFor(location);
                AddNodeFor(location);
            }

            staleNodes.Clear();
        }

        private void UpdateNodePositions()
        {
            foreach(IMapLocation location in activeNodes.Keys)
            {
                if (activeNodes[location] != location.GetNodeToDisplay())
                {
                    staleNodes.Add(location);
                }

                Vector2 viewportVec = cam.WorldToViewportPoint(location.GetPosition());

                float clampedX = Mathf.Clamp(viewportVec.x, 0, 1);
                float clampedY = Mathf.Clamp(viewportVec.y, 0, 1);
                Vector2 clampedViewportPos = new(clampedX, clampedY);

                Vector2 nodePos = clampedViewportPos * map.rectTransform.rect.size;

                activeNodes[location].anchoredPosition = nodePos;
            }
        }
    }
}
