using System;

namespace Meridium.Configuration.CommandLine {
    /// <summary>Implements a basic command-line switch by taking the
    /// switching name and the associated description.</summary>
    /// <remarks>Author: Ray Hayes, http://www.codeproject.com/KB/recipes/commandlineparser.aspx</remarks>
    /// <example></example>
    /// <remark>Only currently is implemented for properties, so all
    /// auto-switching variables should have a get/set method supplied.</remark>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineSwitchAttribute : Attribute {
        /* *******************************************************************
         *  Properties
         * *******************************************************************/
        #region public string Name
        /// <summary>Accessor for retrieving the switch-name for an associated
        /// property.</summary>
        /// <value></value>
        public string Name { get { return _name; } }
        private readonly string _name = "";
        #endregion
        #region public string Description
        /// <summary>Accessor for retrieving the description for a switch of
        /// an associated property.</summary>
        /// <value></value>
        public string Description { get { return _description; } }
        private readonly string _description = "";
        #endregion

        /* *******************************************************************
         *  Constructors
         * *******************************************************************/
        #region public CommandLineSwitchAttribute(string name, string description)
        /// <summary>Attribute constructor.</summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public CommandLineSwitchAttribute(string name,
                                           string description) {
            _name = name;
            _description = description;
        }
        #endregion
    }

    /// <summary>
    /// This class implements an alias attribute to work in conjunction
    /// with the <see cref="CommandLineSwitchAttribute">CommandLineSwitchAttribute</see>
    /// attribute.  If the CommandLineSwitchAttribute exists, then this attribute
    /// defines an alias for it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = true)]
    public class CommandLineAliasAttribute : Attribute {
        #region public string Alias
        /// <summary>
        /// Gets the Alias of the CommandLineAliasAttribute
        /// </summary>
        /// <value></value>
        public string Alias {
            get { return _alias; }
        }
        private readonly string _alias = "";
        #endregion
        #region public CommandLineAliasAttribute(string alias)
        /// <summary>
        /// Initializes a new instance of the <b>CommandLineAliasAttribute</b> class.
        /// </summary>
        /// <param name="alias"></param>
        public CommandLineAliasAttribute(string alias) {
            _alias = alias;
        }
        #endregion
    }
}