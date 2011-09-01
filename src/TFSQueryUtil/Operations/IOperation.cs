namespace TFSQueryUtil.Operations {
    /// <summary>
    /// Summary description for IOperation.
    /// </summary>
    /// <remarks>
    /// 2009-06-22 dan: Created
    /// </remarks>
    public interface IOperation {
        /* *******************************************************************
		 *  Methods 
		 * *******************************************************************/
        #region bool ValidateParameters()
        /// <summary>
        /// Validates the parameters of the operation
        /// </summary>
        /// <returns></returns>
        bool ValidateParameters();
        #endregion
        #region void PerformOperation()
        /// <summary>
        /// Performs the operation of the operation
        /// </summary>
        void PerformOperation();
        #endregion
        #region void ShowHelp()
        /// <summary>
        /// Displays the help of the operation
        /// </summary>
        void ShowHelp();
        #endregion
    }
}