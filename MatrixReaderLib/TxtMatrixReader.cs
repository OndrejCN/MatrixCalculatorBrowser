﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLib;
using RationalLib;
using System.IO;
using Common;

namespace MatrixReaderLib
{
    /// <summary>
    /// Class for reading TXT file content and turning it into Matrix class instance.
    /// </summary>
    public class TxtMatrixReader : IMatrixReader
    {
        /// <summary>
        /// Reads stream and turns it into Matrix class instance.
        /// </summary>
        /// <param name="stream">Stream, which contains Matrix data</param>
        /// <returns>Matrix instance</returns>
        public Matrix Load(Stream stream)
        {
            Matrix matrix = new Matrix();
            string text = Utils.TxtToString(stream);
            List<Rational[]> listOfRationalRows = ReadLinesOfRational(text);

            for (int i = 0; i < listOfRationalRows.Count; i++)
            {
                Rational[] coefficients = GetCoefficients(listOfRationalRows[i]);
                matrix.Rows.Add(new MatrixRow(coefficients, listOfRationalRows[i][listOfRationalRows[i].Length - 1]));
            }

            if (!matrix.Validate())
            {
                throw new ApplicationException("Matrix validation not successful when loading matrix.");
            }
            return matrix;
        }

        private Rational[] GetCoefficients(Rational[] row)
        {
            Rational[] coefficients = new Rational[row.Length - 1];

            for (int j = 0; j < coefficients.Length; j++)
            {
                coefficients[j] = row[j];
            }
            return coefficients;
        }

        //private string TxtToString(Stream stream)
        //{
        //    var rdr = new StreamReader(stream);
        //    return rdr.ReadToEnd();
        //}

        private static List<Rational[]> ReadLinesOfRational(string text)
        {
            ReplaceUnwantedSymbols(text);
            string[] arrayLines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            var lines = arrayLines.OfType<string>();
            return ParseLines(lines);
        }

        private static void ReplaceUnwantedSymbols(string text)
        {
            text = text.Replace(".", ",");
        }

        private static List<Rational[]> ParseLines(IEnumerable<string> lines)
        {
            List<Rational[]> listOfRows = new List<Rational[]>();

            foreach (string line in lines)
            {
                string[] stringNumbers = ParseLine(line);
                Rational[] rationalRow = new Rational[stringNumbers.Count()];
                for (int i = 0; i < stringNumbers.Count(); i++)
                {
                    rationalRow[i] = Rational.Parse(stringNumbers[i]);
                }
                listOfRows.Add(rationalRow);
            }
            return listOfRows;
        }

        private static string[] ParseLine(string line)
        {
            char[] delimiterChars = { ' ', '\t' };
            string[] stringNumbers = line.Split(delimiterChars);
            return stringNumbers;
        }
    }
}
