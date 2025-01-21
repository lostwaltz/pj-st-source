using UnityEngine;

namespace CustomStageTool
{
#if UNITY_EDITOR
    // 커스텀 에디터용 싱글톤
    [ExecuteInEditMode]
    public class SingletonForTool<T> : MonoBehaviour where T : MonoBehaviour 
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                        instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
                
                return instance;
            }
        }

        protected virtual void Awake()
        {
            // 혹시 씬에 남아있을지 모르니 파괴시킴.
            Destroy(gameObject);
            return;
        }
    }
#endif
}