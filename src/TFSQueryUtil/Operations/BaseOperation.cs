using System;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSQueryUtil.Operations {
    /// <summary>
    /// Summary description for BaseOperation.
    /// </summary>
    /// <remarks>
    /// 2009-06-22 dan: Created
    /// </remarks>
    public abstract class BaseOperation : IOperation {
        /* *******************************************************************
         *  Properties 
         * *******************************************************************/
        #region protected CommandLineParameters Parameters
        /// <summary>
        /// Gets the Parameters of the BaseOperation
        /// </summary>
        /// <value></value>
        protected CommandLineParameters Parameters {
            get { return _parameters; }
        }
        private readonly CommandLineParameters _parameters;
        #endregion
        #region protected TeamFoundationServer Tfs
        /// <summary>
        /// Gets the Tfs of the BaseOperation
        /// </summary>
        /// <value></value>
        protected TeamFoundationServer Tfs {
            get { return _tfs ?? (_tfs = new TeamFoundationServer(Parameters.TfsServerUrl)); }
        }
        private TeamFoundationServer _tfs;
        #endregion
        #region protected WorkItemStore Wit
        /// <summary>
        /// Gets the Wit of the BaseOperation
        /// </summary>
        /// <value></value>
        protected WorkItemStore Wit {
            get { return _wit ?? (_wit = Tfs.GetService(typeof (WorkItemStore)) as WorkItemStore); }
        }
        private WorkItemStore _wit;
        #endregion
        #region protected Project Project
        /// <summary>
        /// Gets the Project of the BaseOperation
        /// </summary>
        /// <value></value>
        protected Project Project {
            get { return _project ?? (_project = Wit.Projects[Parameters.ProjectName]); }
        }
        private Project _project;
        #endregion
        /* *******************************************************************
         *  Constructors 
         * *******************************************************************/
        #region protected BaseOperation(CommandLineParameters parameters)
        /// <summary>
        /// Initializes a new instance of the <b>BaseOperation</b> class.
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="ArgumentNullException">If <paramref name="parameters"/> is null.</exception>
        protected BaseOperation(CommandLineParameters parameters) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }
            _parameters = parameters;
        }
        #endregion
        /* *******************************************************************
         *  Methods 
         * *******************************************************************/
        #region public virtual bool ValidateParameters()
        /// <summary>
        /// Validates the parameters
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidateParameters() {
            if (string.IsNullOrEmpty(Parameters.TfsServerUrl))
                Parameters.Errors.Add("You have to supply a TfsServerUrl using /t <tfsservername>");
            if (string.IsNullOrEmpty(Parameters.ProjectName))
                Parameters.Errors.Add("You have to supply a Project name using /p <projectname>");
            return Parameters.Errors.Count == 0;
        }
        #endregion
        #region public abstract void PerformOperation()
        /// <summary>
        /// Performs the operation
        /// </summary>
        public abstract void PerformOperation();
        #endregion
        #region public virtual void ShowHelp()
        /// <summary>
        /// 
        /// </summary>
        public virtual void ShowHelp() {
            Parameters.ShowHelp();
        }
        #endregion
        #region protected StoredQuery GetStoredQuery(string queryName)
        /// <summary>
        /// Gets the StoredQuery in the current project by it's name
        /// </summary>
        /// <param name="queryName">The name of the query to get.</param>
        /// <returns></returns>
        protected StoredQuery GetStoredQuery(string queryName)
        {
            return Project.StoredQueries.Cast<StoredQuery>()
                .FirstOrDefault(query => string.Equals(query.Name, queryName, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion
 
    }
}