using UnityEngine;
using TMPro;
using System.Text;


namespace LevanPangInterview
{
    public class TextAnimator : MonoBehaviour
    {
        //////// Members

        [Header("References")]
        public TextMeshProUGUI Text;

        [Header("Values")]
        public float frequency = 5f;
        public float amplitude = 10f;
        public float delta     = 1f;
        public float speed     = 1f;

        // Privaet
        private string sourceString;

        StringBuilder Builder = new StringBuilder();

        //////// Lifecycle
        
        private void Reset()
        {
            this.Text = this.GetComponent<TextMeshProUGUI>();
        }

        void Awake()
        {
            sourceString = Text.text;
        }

        void Update()
        {
            Builder.Clear();


            for (int i = 0; i < sourceString.Length; i++)
            {
                float offset = Mathf.Sin((Time.time * speed + (i+1) * delta) * frequency) * amplitude;

                Builder.Append("<voffset=");
                Builder.Append(offset);
                Builder.Append("px>");
                Builder.Append(sourceString[i]);
                Builder.Append("</voffset>");
            }

            Text.text = Builder.ToString();
        }

    }
}