using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLib;
using System.Xml.Serialization;
using RationalLib;

namespace MatrixReaderLib
{
    /// <summary>
    /// Class for reading XML file content and turning it into Matrix class instance.
    /// </summary>
    public class XmlMatrixReader : IMatrixReader
    {
        /// <summary>
        /// Reads stream and turns it into Matrix class instance.
        /// </summary>
        /// <param name="stream">Stream, which contains Matrix data</param>
        /// <returns>Matrix instance</returns>
        public Matrix Load(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MatrixXml));

            MatrixXml myObject = (MatrixXml)serializer.Deserialize(stream);

            Matrix matrix = new Matrix();

            foreach (MatrixRowXml matrixRowXml in myObject.Rows)
            {
                Rational[] coefs = new Rational[matrixRowXml.Coefs.Length];
                for (int i = 0; i < coefs.Length; i++)
                {
                    coefs[i] = (Rational)matrixRowXml.Coefs[i];
                }
                matrix.Rows.Add(new MatrixRow(coefs, matrixRowXml.Result));
            }
            return matrix;
        }
    }

    /// <summary>
    /// Class used for reading MatrixRows from XML file
    /// </summary>
    [Serializable()]
    public class MatrixRowXml
    {
        /// <summary>Array for storing Coefficients values of each Matrix row</summary>
        [XmlArray("Coefs")]
        [XmlArrayItem("double", typeof(double))]
        public double[] Coefs { get; set; }

        /// <summary>Variable for storing Result value of each Matrix row</summary>
        [XmlElement("Result")]
        public double Result { get; set; }
    }

    /// <summary>
    /// Class used for reading Matrixes from XML file
    /// </summary>
    [Serializable()]
    [XmlRoot("Matrix")]
    public class MatrixXml
    {
        /// <summary>Variable for storing Width value of Matrix</summary>
        [XmlElement("Width")]
        public int Width { get; set; }

        /// <summary>Variable for storing Height value of Matrix</summary>
        [XmlElement("Height")]
        public int Height { get; set; }

        /// <summary>Array for storing Rows values of Matrix</summary>
        [XmlArray("Rows")]
        [XmlArrayItem("MatrixRow")]
        public MatrixRowXml[] Rows { get; set; }
    }
}