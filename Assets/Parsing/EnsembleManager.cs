using UnityEngine;

namespace Assets.Parsing {
    public class EnsembleManager : MonoBehaviour {

        public static EnsembleManager Manager { get; private set; }

        public EnsembleData data;

        void Start() {
            Manager = this;
        }
    }
}
