using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSQueryUtil.Operations {
    /// <summary>
    /// Lists all queries in a project
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class ListQueryOperation : BaseOperation {
        /* *******************************************************************
         *  Constructors
         * *******************************************************************/
        #region public ListQueryOperation(CommandLineParameters parameters)
        /// <summary>
        /// Initializes a new instance of the <b>ListQueryOperation</b> class.
        /// </summary>
        /// <param name="parameters"></param>
        public ListQueryOperation(CommandLineParameters parameters) : base(parameters) { }
        #endregion
        #region public override void PerformOperation()
        /// <summary>
        /// 
        /// </summary>
        public override void PerformOperation() {
            var data = new List<string[]>
            {
                new []{"Scope","Name","Description"},
                new []{"-------","-------","-------------"},
            };
            data.AddRange(from StoredQuery query in Project.StoredQueries
                          select new[] {query.QueryScope.ToString(), query.Name, query.Description});

            PadData(data, 2);

            foreach (string[] s in data) {
                foreach (string s1 in s) {
                    Console.Write(s1);
                }
                Console.WriteLine();
            }
        }
        #endregion
        #region private static void PadData(IList<string[]> data, int padding)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="padding"></param>
        //TODO:Move to Meridium.StringUtil
        private static void PadData(IList<string[]> data, int padding) {
            var maxWidth = new List<int>();
            //fid maxWidth of each column
            int col = 0;
            bool foundColumn;
            do {
                foundColumn = false;
                foreach (string[] s in data) {
                    if (s.Length <= col) {
                        continue;
                    }
                    if (maxWidth.Count <= col) {
                        maxWidth.Add(0);
                    }
                    foundColumn = true;
                    string st = s[col];
                    if (!string.IsNullOrEmpty(st))
                        maxWidth[col] = Math.Max(maxWidth[col], st.Length);
                }
                col++;

            } while (foundColumn);

            for (int i = 0; i < data.Count; i++) {
                string[] s = data[i];
                var newS = new string[s.Length];
                for (int j = 0; j < s.Length - 1; j++) {
                    string oldSt = s[j];
                    int len = maxWidth[j] + padding;
                    string newSt = "";
                    if (!string.IsNullOrEmpty(oldSt)) {
                        newSt = oldSt;
                    }
                    if (newSt.Length < len) {
                        newSt = newSt + new string(' ', len - newSt.Length);
                    }
                    newS[j] = newSt;
                }
                //copy last data
                newS[s.Length - 1] = s[s.Length - 1];
                data[i] = newS;
            }
        }
        #endregion
    }
}