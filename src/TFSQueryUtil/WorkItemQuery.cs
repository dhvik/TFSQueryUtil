using System.Xml.Serialization;

namespace TFSQueryUtil {
    /// <summary>
    /// Summary description for WorkItemQuery.
    /// </summary>
    /// <remarks>
    /// 2009-06-22 dan: Created
    /// </remarks>
    public class WorkItemQuery {
        /* *******************************************************************
         *  Properties 
         * *******************************************************************/
        #region public int Version
        /// <summary>
        /// Get/Sets the Version of the WorkItemQuery
        /// </summary>
        /// <value></value>
        [XmlAttribute]
        public int Version {
            get { return _version; }
            set { _version = value; }
        }
        private int _version = 1;
        #endregion
        #region public string Query
        /// <summary>
        /// Get/Sets the Query of the WorkItemQuery
        /// </summary>
        /// <value></value>
        [XmlElement("Wiql")]
        public string Query { get; set; }
        #endregion
    }
}