using System;
using System.Collections.Generic;
using System.IO;
using Meridium.Xml.Serialization;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSQueryUtil.Operations {
    public class ImportQueryOperation : BaseOperation {
        private readonly List<KeyValuePair<string,FileInfo>> _filesToImport = new List<KeyValuePair<string, FileInfo>>();
        /* *******************************************************************
         *  Constructors
         * *******************************************************************/
        #region public ImportQueryOperation(CommandLineParameters parameters)
        /// <summary>
        /// Initializes a new instance of the <b>ImportQueryOperation</b> class.
        /// </summary>
        /// <param name="parameters"></param>
        public ImportQueryOperation(CommandLineParameters parameters) : base(parameters) { }
        #endregion
        /* *******************************************************************
         *  Methods
         * *******************************************************************/
        #region public override bool ValidateParameters()
        /// <summary>
        /// Validates the parameters of the operation
        /// </summary>
        /// <returns></returns>
        public override bool ValidateParameters() {
            if (string.IsNullOrEmpty(Parameters.FileName)) {
                Parameters.Errors.Add("You have to supply a filename of the query to import");
            } else {
                if (Parameters.FileName.Contains("*")) {
                    int i = Parameters.FileName.IndexOf("*");
                    int pathEndsAt = Parameters.FileName.LastIndexOf("\\", i);
                    string path = ".";
                    if(pathEndsAt>0)
                        path = Parameters.FileName.Substring(0, pathEndsAt);
                    string wildcard = Parameters.FileName.Substring(pathEndsAt + 1);
                    string[] strings = Directory.GetFiles(path, wildcard);
                    foreach (string filename in strings) {
                        if(!AddFileToImport(filename, null))
                            break;
                    }
                } else {
                    AddFileToImport(Parameters.FileName, Parameters.QueryName);
                }
            }
            return base.ValidateParameters();
        }
        #endregion
        #region private bool AddFileToImport(string filename, string queryName)
        /// <summary>
        /// Adds the supplied file to the import list
        /// </summary>
        /// <param name="filename">The query file to import</param>
        /// <param name="queryName">The name of the query</param>
        /// <returns>True if add was successful, false otherwize</returns>
        private bool AddFileToImport(string filename, string queryName) {
            var file = new FileInfo(filename);
            if (!file.Exists) {
                Parameters.Errors.Add("File " + file.FullName + " doesn't exist");
                return false;
            }

            if (queryName == null)
                queryName = CalculateQueryName(file);
            _filesToImport.Add(new KeyValuePair<string, FileInfo>(queryName, file));
            return true;
        }
        #endregion
        #region private static string CalculateQueryName(FileInfo file)
        /// <summary>
        /// Calculates the default query name.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static string CalculateQueryName(FileInfo file) {
            int extensionStart = file.Name.LastIndexOf(".");
            return extensionStart > 0 ? file.Name.Substring(0, extensionStart) : file.Name;
        }
        #endregion
        #region public override void PerformOperation()
        /// <summary>
        /// Performs the Import operation
        /// </summary>
        public override void PerformOperation() {

            int nrOfImportedQueries = 0;
            foreach (KeyValuePair<string, FileInfo> pair in _filesToImport) {

                FileInfo file = pair.Value;
                string queryName = pair.Key;
            //Read file
            var wiq = XmlSerializerUtil.DeserializeFromXmlFile<WorkItemQuery>(file.FullName);

            //see if query already exists
            StoredQuery q = GetStoredQuery(queryName);
            
            //update existing?
            if (q != null) {
                if (!Parameters.AskYesNo("The StoredQuery " + queryName + " already exists, do you want to overwrite it?"))
                    continue;
                q.QueryText = wiq.Query;
                q.Update();
            } else {
            
                //create new
                q = new StoredQuery(Parameters.QueryScope, queryName, wiq.Query, Parameters.QueryDescription);
                Project.StoredQueries.Add(q);
            }
            Console.WriteLine("Imported file "+file.Name+ " as query " + queryName);
                nrOfImportedQueries++;
            }
            if(nrOfImportedQueries>1)
                Console.WriteLine("Imported "+nrOfImportedQueries+" queries.");

        }
        #endregion

    }
}