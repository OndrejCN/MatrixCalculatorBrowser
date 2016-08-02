using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLib;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using RationalLib;

namespace MatrixReaderLib
{
    /// <summary>
    /// Class for reading JSON file content and turning it into Matrix class instance.
    /// </summary>
    public class JsonMatrixReader : IMatrixReader
    {
        /// <summary>
        /// Reads stream and turns it into Matrix class instance.
        /// </summary>
        /// <param name="stream">Stream, which contains Matrix data</param>
        /// <returns>Matrix instance</returns>
        public Matrix Load(Stream stream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MatrixJson));

            //stream.Position = 0;
            MatrixJson myObject = (MatrixJson)serializer.ReadObject(stream);

            Matrix matrix = new Matrix();
            
            foreach (MatrixRowJson matrixRowJson in myObject.Rows)
            {
                Rational[] coefs = new Rational[matrixRowJson.Coefs.Length];
                for (int i = 0; i < coefs.Length; i++)
                {
                    coefs[i] = (Rational)matrixRowJson.Coefs[i];
                }
                matrix.Rows.Add(new MatrixRow(coefs, matrixRowJson.Result));
            }
            return matrix;
        }
    }

    /// <summary>
    /// Class used for reading MatrixRows from XML file
    /// </summary>
    [DataContract]
    public class MatrixJson
    {
        /// <summary>Variable for storing Height value of Matrix</summary>
        [DataMember(Name = "Height")]
        public int Height { get; set; }

        /// <summary>Array for storing Rows values of Matrix</summary>
        [DataMember(Name = "Rows")]
        public MatrixRowJson[] Rows { get; set; }

        /// <summary>Variable for storing Width value of Matrix</summary>
        [DataMember(Name = "Width")]
        public int Width { get; set; }
    }

    /// <summary>
    /// Class used for reading MatrixRows from JSON file
    /// </summary>
    [DataContract]
    public class MatrixRowJson
    {
        /// <summary>Array for storing Coefficients values of each Matrix row</summary>
        [DataMember(Name = "Coefs")]
        public double[] Coefs { get; set; }

        /// <summary>Variable for storing Result value of each Matrix row</summary>
        [DataMember(Name = "Result")]
        public double Result { get; set; }
    }
}