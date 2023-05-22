using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

namespace LevanPangInterview
{
    public class CameraShake : MonoBehaviour
	{
        public async Task Shake()
        {
            await this.Shake(1f, 10);
        }

        public async Task Shake(float durationSeconds, float magnitude)
        {
            Vector3 orignalPosition = transform.position;
            float   elapsed         = 0f;

            while (elapsed < durationSeconds)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.position  = new Vector3(x, y, -10f);
                elapsed            += Time.deltaTime;
                await Task.Yield();;
            }

            transform.position = orignalPosition;
        }
    }   
}