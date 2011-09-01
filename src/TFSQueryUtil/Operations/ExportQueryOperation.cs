using System;
using System.IO;
using System.Linq;
using System.Text;
using Meridium.Xml.Serialization;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSQueryUtil.Operations {
    public class ExportQueryOperation : BaseOperation {
        /* *******************************************************************
         *  Constructors
         * *******************************************************************/
        #region public ExportQueryOperation(CommandLineParameters parameters)
        /// <summary>
        /// Initializes a new instance of the <b>ExportQueryOperation</b> class.
        /// </summary>
        /// <param name="parameters"></param>
        public ExportQueryOperation(CommandLineParameters parameters)
            : base(parameters) {
        }
        #endregion
        /* *******************************************************************
         *  Methods
         * *******************************************************************/
        #region public override bool ValidateParameters()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool ValidateParameters() {
            if (string.IsNullOrEmpty(Parameters.QueryName))
                Parameters.Errors.Add("You have to supply a name of the query to export using /q <query name>");
            if (string.IsNullOrEmpty(Parameters.FileName))
                Parameters.FileName = Parameters.QueryName + ".wiq";
            return base.ValidateParameters();
        }
        #endregion
        #region public override void PerformOperation()
        /// <summary>
        /// Performs the export operation
        /// </summary>
        public override void PerformOperation() {
            if (Equals(Parameters.QueryName, "*")) {
                int nrOfExportedQueries = Project.StoredQueries.Cast<StoredQuery>().Count(query => ExportQuery(query, query.Name + ".wiq"));
                Console.WriteLine("Exported " + nrOfExportedQueries + " queries.");
            } else {

                //Get query to export
                StoredQuery q = GetStoredQuery(Parameters.QueryName);

                if (q == null) {
                    Console.WriteLine("No query named " + Parameters.QueryName + " was found in the tfs project " + Project.Name + "@" + Tfs.Name);
                    return;
                }

                ExportQuery(q, Parameters.FileName);
            }
        }
        #endregion
        #region private bool ExportQuery(StoredQuery q, string filename)
        /// <summary>
        /// Exports the supplied query
        /// </summary>
        /// <param name="q">The <see cref="StoredQuery"/> to export</param>
        /// <param name="filename">The name of the file to export it to.</param>
        /// <returns>True if the query was exported, false otherwise.</returns>
        private bool ExportQuery(StoredQuery q, string filename) {
            //overwrite file?
            if (File.Exists(filename))
                if (!Parameters.AskYesNo("The filename " + filename + " already exists, do you want to overwrite it?"))
                    return false;

            //Create item and serialize
            var wiq = new WorkItemQuery {
                Query = q.QueryText
            };
            XmlSerializerUtil.SerializeToXmlFile(wiq, filename,new UTF8Encoding(false));
            Console.WriteLine("Exported query " + q.Name + " to file " + filename);
            return true;
        }
        #endregion
    }
}