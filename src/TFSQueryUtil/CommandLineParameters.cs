using Meridium.Configuration.CommandLine;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSQueryUtil.Operations;

namespace TFSQueryUtil {
    /// <summary>
    /// Contains the command line
    /// </summary>
    /// <remarks>
    /// 2009-06-22 dan: Created
    /// </remarks>
    /// <example></example>
    public class CommandLineParameters : CommandLineParametersBase {

        /* *******************************************************************
         *  Properties 
         * *******************************************************************/
        #region public string TfsServerUrl
        /// <summary>
        /// Get/Sets the TfsServerUrl of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("tfsserver", "The url of the tfsserver")]
        [CommandLineAlias("t")]
        public string TfsServerUrl { get; set; }
        #endregion
        #region public string ProjectName
        /// <summary>
        /// Get/Sets the ProjectName of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("project", "The name of the tfs project")]
        [CommandLineAlias("p")]
        public string ProjectName { get; set; }
        #endregion
        #region public QueryOperation QueryOperation
        /// <summary>
        /// Get/Sets the QueryOperation of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("operation", "The type of operation to perform")]
        [CommandLineAlias("o")]
        public QueryOperation QueryOperation { get; set; }
        #endregion
        #region public string QueryName
        /// <summary>
        /// Get/Sets the QueryName of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("query", "The name of the query that the operation affects")]
        [CommandLineAlias("q")]
        public string QueryName { get; set; }
        #endregion
        #region public string FileName
        /// <summary>
        /// Get/Sets the QueryName of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("file", "The name of the file that the operation affects")]
        [CommandLineAlias("f")]
        public string FileName { get; set; }
        #endregion
        #region public QueryScope QueryScope
        /// <summary>
        /// Get/Sets the QueryScope of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("queryscope", "The scope of the query, Public or Private (default)")]
        [CommandLineAlias("qs")]
        public QueryScope QueryScope {
            get { return _queryScope; }
            set { _queryScope = value; }
        }
        private QueryScope _queryScope = QueryScope.Private;
        #endregion
        #region public string QueryDescription
        /// <summary>
        /// Get/Sets the QueryDescription of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("querydescription", "The description of the query, null by default")]
        [CommandLineAlias("qd")]
        public string QueryDescription { get; set; }
        #endregion

        /* *******************************************************************
         *  Methods 
         * *******************************************************************/
        #region public CommandLineParameters()
        /// <summary>
        /// Initializes a new instance of the <b>CommandLineParameters</b> class.
        /// </summary>
        public CommandLineParameters() {
            HelpMessage = @"TFSQueryUtil (C) Copyright 2009 Meridium AB. All rights reserved.
CommandParsing based on work by Ray Hayes, http://tinyurl.com/lg4d3d

Manages queries for a TFS server/project

Use:

TFSQueryUtil /t <tfs> /p <project> /o <operation> [/q <queryName>] 
[/f <fileName>] [/qs <queryscope>] [/qd <querydescription>] [/y]
/t  Specifies the name of the Team Foundation Server. This can also be a  
    fully specified URL such as http://tfs:8181.
/p  Specifies the Team Project on the Team Foundation Server to which the
    query is affected.
/o  Specifies the operation to use. Avaliable operations are:
      List    Lists all queries for the project
      Export  Exports a query to a file
      Import  Imports a query from a file
/q  Specifies the name of the query (only used in export/import operations)
/f  Specifies the name of the file (only used in export/import operations)
/qs Specifies the scope of the query to import (Public or Private (default))
/qd Specifies the description of the query to import (default null)
/y  Answers yes on all questions
";
        }
        #endregion
        #region public void Execute()
        /// <summary>
        /// Executes the commandLine
        /// </summary>
        public override int Execute() {
            //should we display parameter help?
            if (DisplayHelp) {
                ShowHelp();
                return 0;
            }

            //find operation
            IOperation operation = CreateOperation();
            if (operation == null) {
                ShowHelp();
                return 1;
            }

            //validate operation parameters
            if (!operation.ValidateParameters()) {
                operation.ShowHelp();
                return 1;
            }

            //perform operation
            operation.PerformOperation();
            return 0;
        }
        #endregion
        /* *******************************************************************
         *  Event methods 
         * *******************************************************************/
        #region private IOperation CreateOperation()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IOperation CreateOperation() {
            switch (QueryOperation) {
                case QueryOperation.List:
                    return new ListQueryOperation(this);
                case QueryOperation.Export:
                    return new ExportQueryOperation(this);
                case QueryOperation.Import:
                    return new ImportQueryOperation(this);
                default:
                    Errors.Add("Operation " + QueryOperation + " is not supported.");
                    return null;
            }
        }
        #endregion
   }
}