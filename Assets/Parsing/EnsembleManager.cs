using UnityEngine;

namespace Assets.Parsing {
    public class EnsembleManager : MonoBehaviour {

        public string ensembleFolder;
        public static EnsembleData Data { get; private set; }

        // Use this for initialization
        void Awake() {
            string ensembleFilePath = Application.dataPath +
                "/Calibrations/" + ensembleFolder + "/calibration.xml";
            Data = new EnsembleData(ensembleFilePath);
        }
    }
}
