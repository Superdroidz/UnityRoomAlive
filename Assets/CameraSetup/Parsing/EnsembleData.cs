using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Assets.Interop;

namespace Assets.Parsing {

    [Serializable]
    public class EnsembleData {

        public string name;
        public List<CameraData> cameras;
        public List<ProjectorData> projectors;

        public EnsembleData(string ensembleFile) {
            XElement root = XElement.Load(ensembleFile);

            // Deserialize ensemble file
            try {
                XNamespace ns = root.Name.Namespace;
                name = root.Element(ns + "name").Value;
                cameras =
                    (from camera in root.Descendants(ns + "ProjectorCameraEnsemble.Camera")
                    select new CameraData {
                        calibration = MakeCalibration(camera, ns),
                        hostNameOrAddress = camera.Element(ns + "hostNameOrAddress").Value,
                        name = camera.Element(ns + "name").Value,
                        pose = CalibrationMatrix.ToMatrix4x4(camera.Element(ns + "pose"))
                    }).ToList();
                projectors =
                    (from projector in root.Descendants(ns + "ProjectorCameraEnsemble.Projector")
                    select new ProjectorData {
                        cameraMatrix = CalibrationMatrix.ToMatrix4x4(projector.Element(ns + "cameraMatrix")),
                        displayIndex = Convert.ToInt32(projector.Element(ns + "displayIndex").Value),
                        width = Convert.ToInt32(projector.Element(ns + "width").Value),
                        height = Convert.ToInt32(projector.Element(ns + "height").Value),
                        hostNameOrAddress = projector.Element(ns + "hostNameOrAddress").Value,
                        lensDistortion = CalibrationMatrix.ToMatrix4x4(projector.Element(ns + "lensDistortion")),
                        lockIntrinsics = Convert.ToBoolean(projector.Element(ns + "lockIntrinsics").Value),
                        name = projector.Element(ns + "name").Value,
                        pose = CalibrationMatrix.ToMatrix4x4(projector.Element(ns + "pose"))
                    }).ToList();
            } catch (NullReferenceException) {
                throw new NullReferenceException("File incorrect format.");
            }
        }

        CameraCalibration MakeCalibration(XElement cameraNode, XNamespace ns) {
            XElement calibration = cameraNode.Element(ns + "calibration");
            return new CameraCalibration(CalibrationMatrix.ToMatrix4x4(calibration.Element(ns + "colorCameraMatrix")),
                                         CalibrationMatrix.ToMatrix4x4(calibration.Element(ns + "colorLensDistortion")),
                                         CalibrationMatrix.ToMatrix4x4(calibration.Element(ns + "depthCameraMatrix")),
                                         CalibrationMatrix.ToMatrix4x4(calibration.Element(ns + "depthLensDistortion")),
                                         CalibrationMatrix.ToMatrix4x4(calibration.Element(ns + "depthToColorTransform")));
        }

    }
}
