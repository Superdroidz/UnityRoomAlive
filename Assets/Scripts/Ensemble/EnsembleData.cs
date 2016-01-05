using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Ensemble {

    public class EnsembleData {
        public string name;
        public IEnumerable<CameraData> cameras;
        public IEnumerable<ProjectorData> projectors;

        public EnsembleData(string ensembleFile) {
            XElement root = XElement.Load(ensembleFile);
            XNamespace ns = root.Name.Namespace;

            // Deserialize ensemble file
            name = root.Element(ns + "name").Value;
            cameras =
                from camera in root.Descendants(ns + "ProjectorCameraEnsemble.Camera")
                select new CameraData {
                    calibration = MakeCalibration(camera, ns),
                    hostNameOrAddress = camera.Element(ns + "hostNameOrAddress").Value,
                    name = camera.Element(ns + "name").Value,
                    pose = ParseMatrix(camera.Element(ns + "pose"))
                };
            projectors =
                from projector in root.Descendants(ns + "ProjectorCameraEnsemble.Projector")
                select new ProjectorData {
                    cameraMatrix = ParseMatrix(projector.Element(ns + "cameraMatrix")),
                    displayIndex = Convert.ToInt32(projector.Element(ns + "displayIndex").Value),
                    width = Convert.ToInt32(projector.Element(ns + "width").Value),
                    height = Convert.ToInt32(projector.Element(ns + "height").Value),
                    hostNameOrAddress = projector.Element(ns + "hostNameOrAddress").Value,
                    lensDistortion = ParseMatrix(projector.Element(ns + "lensDistortion")),
                    lockIntrinsics = Convert.ToBoolean(projector.Element(ns + "lockIntrinsics").Value),
                    name = projector.Element(ns + "name").Value,
                    pose = ParseMatrix(projector.Element(ns + "pose"))
                };
        }

        CameraCalibration MakeCalibration(XElement cameraNode, XNamespace ns) {
            XElement calibration = cameraNode.Element(ns + "calibration");
            return new CameraCalibration(ParseMatrix(calibration.Element(ns + "colorCameraMatrix")),
                                         ParseMatrix(calibration.Element(ns + "colorLensDistortion")),
                                         ParseMatrix(calibration.Element(ns + "depthCameraMatrix")),
                                         ParseMatrix(calibration.Element(ns + "depthLensDistortion")),
                                         ParseMatrix(calibration.Element(ns + "depthToColorTransform")));
        }

        Matrix4x4 ParseMatrix(XElement matrixNode) {
            // parse the columns using LINQ to XML
            IEnumerable<Vector4> columns =
                from column in matrixNode.Elements().ElementAt(0).Elements()
                select ParseColumn(column);

            // copy the columns over into Unity's matrix data structure
            Matrix4x4 matrix = Matrix4x4.zero;
            for (int index = 0; index < 4; index++) {
                if (index >= columns.Count()) {
                    break;
                }
                matrix.SetColumn(index, columns.ElementAt(index));
            }

            return matrix;
        }

        Vector4 ParseColumn(XElement columnNode) {
            // Parse the values of columnNode as a sequence of floats
            IEnumerable<float> parsedColumn =
                from element in columnNode.Elements()
                select Convert.ToSingle(element.Value);

            // copy into Unity's Vector4 data structure
            Vector4 column = Vector4.zero;
            for (int index = 0; index < 4; index++) {
                if (index >= parsedColumn.Count()) {
                    break;
                }
                column[index] = parsedColumn.ElementAt(index);
            }

            return column;
        }
    }
}
