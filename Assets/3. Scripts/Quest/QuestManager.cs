using UnityEngine;

namespace Quest
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}