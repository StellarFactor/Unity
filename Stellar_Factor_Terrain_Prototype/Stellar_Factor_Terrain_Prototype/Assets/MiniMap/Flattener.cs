using System.Collections.Generic;
using UnityEngine;

namespace Overtown.Util
{
    public class Flattener : MonoBehaviour
    {
        [SerializeField] private int notchCount;
        private List<int> notches;

        private int Interval
        {
            get
            {
                return 100 / notchCount;
            }
        }

        private List<int> Notches
        {
            get
            {
                if (notches != null && notches.Count > 0)
                {
                    return notches;
                }

                List<int> list = new();

                for (int i = 0; i < notchCount; i++)
                {
                    list.Add(i * Interval);
                }

                notches = list;
                return notches;
            }
        }

        public Texture2D Flatten(Texture2D tex)
        {
            // for each col
            for (int i = 0; i < tex.width; i++)
            {
                // for each row
                for (int j = 0; j < tex.height; j++)
                {
                    Dictionary<char, float> dict = new();

                    Color currentPixelColor = tex.GetPixel(i, j);
                    Color newPixelColor = Color.white;
                    Color pink = Color.red + (Color.white / 2);

                    dict['r'] = currentPixelColor.r * 100;
                    dict['g'] = currentPixelColor.g * 100;
                    dict['b'] = currentPixelColor.b * 100;
                    dict['a'] = currentPixelColor.a * 100;

                    foreach (char key in dict.Keys)
                    {
                        int left, right;

                        int nearestNotch = 0;

                        float val = dict[key];

                        for (int k = 0; k < Notches.Count; k++)
                        {
                            left = k * Interval;
                            right = (k + Interval > Notches.Count) ? Notches.Count - 1 : k + Interval;

                            float distanceLeft = Mathf.Abs(val - left);
                            float distanceRight = Mathf.Abs(val - right);

                            if (distanceLeft < Interval || distanceRight < Interval)
                            {
                                nearestNotch = (int)Mathf.Min(distanceLeft, distanceRight);
                                break;
                            }
                        }

                        dict[key] = nearestNotch;
                    }

                    newPixelColor = new Color(dict['r'], dict['g'], dict['b'], 1);

                    // For debugging
                    if (newPixelColor == Color.white)
                    {
                        newPixelColor = pink;
                    }

                    tex.SetPixel(i, j, newPixelColor);
                }
            }

            tex.Apply();

            return tex;
        }
    }
}
