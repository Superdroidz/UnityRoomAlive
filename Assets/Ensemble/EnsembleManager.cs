using System.Linq;
using UnityEngine;

namespace Ensemble {
    public class EnsembleManager : MonoBehaviour {

        public Camera cameraPrefab;
        public string ensembleFolder;
        public EnsembleData Data { get; private set; }

        // Use this for initialization
        void Start() {
            string ensembleFilePath = Application.dataPath +
                "/Calibrations/" + ensembleFolder + "/calibration.xml";
            Data = new EnsembleData(ensembleFilePath);

            // Set this to choose which projector this Unity instance uses.
            int localProjectorNumber = 0;

            Camera cameraInstance = Instantiate(cameraPrefab);
            cameraInstance.worldToCameraMatrix = Data.projectors.ElementAt(localProjectorNumber).pose;
            cameraInstance.tag = "MainCamera";
        }
    }
}
