using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class FPS : MonoBehaviour
    {
        public float updateInterval = 0.5F;
        private double lastInterval;
        private int frames;
        private float fps;

        public TextMeshProUGUI Label;
        void Start()
        {
            lastInterval = Time.realtimeSinceStartup;
            frames = 0;
        }

        void Update()
        {
            ++frames;
            float timeNow = Time.realtimeSinceStartup;
            if (timeNow > lastInterval + updateInterval)
            {
                fps = (int)(frames / (timeNow - lastInterval));
                frames = 0;
                Label.text = fps.ToString();
                lastInterval = timeNow;
            }
        }
    }
}