using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public class GlobalInstance : MonoBehaviour {

	    public static Dictionary<string, List<GlobalInstance>> dictionary = new Dictionary<string, List<GlobalInstance>>();

        public string type;

        public virtual void OnEnable() {
            List<GlobalInstance> instanceList;
            if (dictionary.TryGetValue(type, out instanceList)) {
                instanceList.Add(this);
            }
            else {
                instanceList = new List<GlobalInstance> { this };
                dictionary.Add(type, instanceList);
            }
        }
        public virtual void OnDisable() {
            List<GlobalInstance> instanceList;
            if (dictionary.TryGetValue(type, out instanceList)) {
                instanceList.Remove(this);
                if (instanceList.Count == 0) {
                    dictionary.Remove(type);
                }
            }
        }

        public static List<GlobalInstance> GetInstanceListOfType(string type) {
            dictionary.TryGetValue(type, out List<GlobalInstance> instanceList);
            if (instanceList == null) {
                return new List<GlobalInstance>();
            }
            return instanceList;
        }
        public List<GlobalInstance> GetInstanceListOfType() {
            return GetInstanceListOfType(type);
        }
        public List<GameObject> GetGameObjectListOfType() {
            return GetInstanceListOfType().ConvertAll(i => i.gameObject);
        }

    }

}
