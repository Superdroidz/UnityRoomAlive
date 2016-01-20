using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Interop {
    class CalibrationMatrix {
        public static Matrix4x4 ToMatrix4x4(XElement matrixNode) {
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

        public static Vector4 ParseColumn(XElement columnNode) {
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
