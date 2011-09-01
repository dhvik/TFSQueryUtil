using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Meridium.Configuration.CommandLine {
    /// <summary>Implementation of a command-line parsing class.  Is capable of
    /// having switches registered with it directly or can examine a registered
    /// class for any properties with the appropriate attributes appended to
    /// them.</summary>
    /// <remarks>Author: Ray Hayes, http://www.codeproject.com/KB/recipes/commandlineparser.aspx</remarks>
    public class Parser {
        /// <summary>A simple internal class for passing back to the caller
        /// some information about the switch.  The internals/implementation
        /// of this class has privillaged access to the contents of the
        /// SwitchRecord class.</summary>
        public class SwitchInfo {
            private readonly SwitchRecord _switch;

            #region public string Name
            /// <summary>
            /// Gets the Name of the SwitchInfo
            /// </summary>
            /// <value></value>
            public string Name { get { return _switch.Name; } }
            #endregion
            #region public string Description
            /// <summary>
            /// Gets the Description of the SwitchInfo
            /// </summary>
            /// <value></value>
            public string Description { get { return _switch.Description; } }
            #endregion
            #region public string[] Aliases
            /// <summary>
            /// Gets the Aliases of the SwitchInfo
            /// </summary>
            /// <value></value>
            public string[] Aliases { get { return _switch.Aliases; } }
            #endregion
            #region public Type Type
            /// <summary>
            /// Gets the Type of the SwitchInfo
            /// </summary>
            /// <value></value>
            public Type Type { get { return _switch.Type; } }
            #endregion
            #region public object Value
            /// <summary>
            /// Gets the Value of the SwitchInfo
            /// </summary>
            /// <value></value>
            public object Value { get { return _switch.Value; } }
            #endregion
            #region public object InternalValue
            /// <summary>
            /// Gets the InternalValue of the SwitchInfo
            /// </summary>
            /// <value></value>
            public object InternalValue { get { return _switch.InternalValue; } }
            #endregion
            #region public bool IsEnum
            /// <summary>
            /// Gets the IsEnum of the SwitchInfo
            /// </summary>
            /// <value></value>
            public bool IsEnum { get { return _switch.Type.IsEnum; } }
            #endregion
            #region public string[] Enumerations
            /// <summary>
            /// Gets the Enumerations of the SwitchInfo
            /// </summary>
            /// <value></value>
            public string[] Enumerations { get { return _switch.Enumerations; } }
            #endregion

            #region public SwitchInfo(object rec)
            /// <summary>
            /// Constructor for the SwitchInfo class.  Note, in order to hide to the outside world
            /// information not necessary to know, the constructor takes a System.Object (aka
            /// object) as it's registering type.  If the type isn't of the correct type, an exception
            /// is thrown.
            /// </summary>
            /// <param name="rec">The SwitchRecord for which this class store information.</param>
            /// <exception cref="ArgumentException">Thrown if the rec parameter is not of
            /// the type SwitchRecord.</exception>
            public SwitchInfo(object rec) {
                if (rec is SwitchRecord)
                    _switch = rec as SwitchRecord;
                else
                    throw new ArgumentException();
            }
            #endregion
        }

        /// <summary>
        /// The SwitchRecord is stored within the parser's collection of registered
        /// switches.  This class is private to the outside world.
        /// </summary>
        private class SwitchRecord {
            /* *******************************************************************
             *  Properties
             * *******************************************************************/
            #region public object Value
            /// <summary>
            /// Gets the Value of the SwitchRecord
            /// </summary>
            /// <value></value>
            public object Value {
                get {
                    if (ReadValue != null)
                        return ReadValue;
                    return _value;
                }
            }
            #endregion
            #region public object InternalValue
            /// <summary>
            /// Gets the InternalValue of the SwitchRecord
            /// </summary>
            /// <value></value>
            public object InternalValue {
                get { return _value; }
            }
            private object _value;
            #endregion
            #region public string Name
            /// <summary>
            /// Get/Sets the Name of the SwitchRecord
            /// </summary>
            /// <value></value>
            public string Name {
                get { return _name; }
                set { _name = value; }
            }
            private string _name = "";
            #endregion
            #region public string Description
            /// <summary>
            /// Get/Sets the Description of the SwitchRecord
            /// </summary>
            /// <value></value>
            public string Description {
                get { return _description; }
                set { _description = value; }
            }
            private string _description = "";
            #endregion
            #region public Type Type
            /// <summary>
            /// Gets the Type of the SwitchRecord
            /// </summary>
            /// <value></value>
            public Type Type {
                get { return _switchType; }
            }
            private readonly Type _switchType = typeof(bool);
            #endregion
            #region public string[] Aliases
            /// <summary>
            /// Gets the Aliases of the SwitchRecord
            /// </summary>
            /// <value></value>
            public string[] Aliases {
                get { return (_aliases != null) ? (string[])_aliases.ToArray(typeof(string)) : null; }
            }
            private System.Collections.ArrayList _aliases;
            #endregion
            #region public string Pattern
            /// <summary>
            /// Gets the Pattern of the SwitchRecord
            /// </summary>
            /// <value></value>
            public string Pattern {
                get { return _pattern; }
            }
            private string _pattern = "";
            #endregion
            #region public System.Reflection.MethodInfo SetMethod
            /// <summary>
            /// Sets the SetMethod of the SwitchRecord
            /// </summary>
            /// <value></value>
            public System.Reflection.MethodInfo SetMethod {
                set { _setMethod = value; }
            }
            private System.Reflection.MethodInfo _setMethod;
            #endregion
            #region public System.Reflection.MethodInfo GetMethod
            /// <summary>
            /// Sets the GetMethod of the SwitchRecord
            /// </summary>
            /// <value></value>
            public System.Reflection.MethodInfo GetMethod {
                set { _getMethod = value; }
            }
            private System.Reflection.MethodInfo _getMethod;
            #endregion
            #region public object PropertyOwner
            /// <summary>
            /// Sets the PropertyOwner of the SwitchRecord
            /// </summary>
            /// <value></value>
            public object PropertyOwner {
                set { _propertyOwner = value; }
            }
            private object _propertyOwner;
            #endregion
            #region public object ReadValue
            /// <summary>
            /// Gets the ReadValue of the SwitchRecord
            /// </summary>
            /// <value></value>
            public object ReadValue {
                get {
                    object o = null;
                    if (_propertyOwner != null && _getMethod != null)
                        o = _getMethod.Invoke(_propertyOwner, null);
                    return o;
                }
            }
            #endregion
            #region public string[] Enumerations
            /// <summary>
            /// Gets the Enumerations of the SwitchRecord
            /// </summary>
            /// <value></value>
            public string[] Enumerations {
                get {
                    return _switchType.IsEnum ? Enum.GetNames(_switchType) : null;
                }
            }
            #endregion
            /* *******************************************************************
             *  Constructors
             * *******************************************************************/
            #region public SwitchRecord(string name, string description)
            /// <summary>
            /// Initializes a new instance of the <b>SwitchRecord</b> class.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            public SwitchRecord(string name, string description) {
                Initialize(name, description);
            }
            #endregion
            #region public SwitchRecord(string name, string description, Type type)
            /// <summary>
            /// Initializes a new instance of the <b>SwitchRecord</b> class.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            /// <param name="type"></param>
            /// <exception cref="ArgumentException">If currently only Ints, Bool and Strings are supported.</exception>
            public SwitchRecord(string name, string description, Type type) {
                if (type == typeof(bool) ||
                     type == typeof(string) ||
                     type == typeof(int) ||
                     type.IsEnum) {
                    _switchType = type;
                    Initialize(name, description);
                } else
                    throw new ArgumentException("Currently only Ints, Bool and Strings are supported");
            }
            #endregion
            /* *******************************************************************
             *  Methods
             * *******************************************************************/
            #region private void Initialize(string name, string description)
            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="description"></param>
            private void Initialize(string name, string description) {
                _name = name;
                _description = description;

                BuildPattern();
            }
            #endregion
            #region private void BuildPattern()
            /// <summary>
            /// 
            /// </summary>
            /// <exception cref="ArgumentException"></exception>
            private void BuildPattern() {
                string matchString = Regex.Escape(Name);

                if (Aliases != null && Aliases.Length > 0)
                    foreach (string s in Aliases)
                        matchString += "|" + Regex.Escape(s);

                const string strPatternStart = @"(\s|^)(?<match>(-{1,2}|/)(";
                string strPatternEnd;  // To be defined below.

                // The common suffix ensures that the switches are followed by
                // a white-space OR the end of the string.  This will stop
                // switches such as /help matching /helpme
                //
                const string strCommonSuffix = @"(?=(\s|$))";

                if (Type == typeof(bool))
                    strPatternEnd = @")(?<value>(\+|-){0,1}))";
                else if (Type == typeof(string))
                    strPatternEnd = @")(?::|\s+))((?:"")(?<value>[^""]+)(?:"")|(?<value>\S+))";
                else if (Type == typeof(int))
                    strPatternEnd = @")(?::|\s+))((?<value>(-|\+)[0-9]+)|(?<value>[0-9]+))";
                else if (Type.IsEnum) {
                    string[] enumNames = Enumerations;
                    string eStr = Regex.Escape(enumNames[0]);
                    for (int e = 1; e < enumNames.Length; e++)
                        eStr += "|" + Regex.Escape(enumNames[e]);
                    strPatternEnd = @")(?::|\s+))(?<value>" + eStr + @")";
                } else
                    throw new ArgumentException();

                // Set the internal regular expression pattern.
                _pattern = strPatternStart + matchString + strPatternEnd + strCommonSuffix;
            }
            #endregion
            #region public void AddAlias(string alias)
            /// <summary>
            /// 
            /// </summary>
            /// <param name="alias"></param>
            public void AddAlias(string alias) {
                if (_aliases == null)
                    _aliases = new System.Collections.ArrayList();
                _aliases.Add(alias);

                BuildPattern();
            }
            #endregion
            #region public void Notify(object value)
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            public void Notify(object value) {
                if (_propertyOwner != null && _setMethod != null) {
                    object[] parameters = new object[1];
                    parameters[0] = value;
                    _setMethod.Invoke(_propertyOwner, parameters);
                }
                _value = value;
            }
            #endregion
        }

        #region public string ApplicationName
        /// <summary>
        /// Gets the ApplicationName of the Parser
        /// </summary>
        /// <value></value>
        public string ApplicationName {
            get { return _applicationName; }
        }
        private string _applicationName = "";
        #endregion

        private readonly string _commandLine = "";
        private string _workingString = "";
		#region public string[] Parameters
        /// <summary>
        /// Gets the Parameters of the Parser
        /// </summary>
        /// <value></value>
        public string[] Parameters {
            get { return _splitParameters; }
        }
        private string[] _splitParameters;
		#endregion

        
		#region public SwitchInfo[] Switches
        /// <summary>
        /// Gets the Switches of the Parser
        /// </summary>
        /// <value></value>
        public SwitchInfo[] Switches {
            get {
                if (_switches == null)
                    return null;
                SwitchInfo[] si = new SwitchInfo[_switches.Count];
                for (int i = 0; i < _switches.Count; i++)
                    si[i] = new SwitchInfo(_switches[i]);
                return si;
            }
        }
		#endregion

		#region public object this[string name]
        /// <summary>
        /// Gets the <see cref="Object"/> item identified by the given arguments of the Parser
        /// </summary>
        /// <value></value>
        public object this[string name] {
            get {
                if (_switches != null)
                    for (int i = 0; i < _switches.Count; i++)
                        if (string.Compare(_switches[i].Name, name, true) == 0)
                            return _switches[i].Value;
                return null;
            }
        }
        private List<SwitchRecord> _switches;
		#endregion

		#region private void ExtractApplicationName()
        /// <summary>
        /// Extracts the application name from the commandline
        /// </summary>
        /// <exception cref="ApplicationException">If unable to parse commandline to extract application name .</exception>
        private void ExtractApplicationName() {
            Regex r = new Regex(@"^(?<commandLine>("".+?""|(\S)+))\s*(?<remainder>.*)$",RegexOptions.ExplicitCapture);
            Match m = r.Match(_commandLine);
            if(!m.Success) {
                throw new ApplicationException("Unable to parse commandline to extract application name "+_commandLine);
            }
            _applicationName = m.Groups["commandLine"].Value;
            _workingString = m.Groups["remainder"].Value;
        }
		#endregion

        private void SplitParameters() {
            // Populate the split parameters array with the remaining parameters.
            // Note that if quotes are used, the quotes are removed.
            // e.g.   one two three "four five six"
            //						0 - one
            //						1 - two
            //						2 - three
            //						3 - four five six
            // (e.g. 3 is not in quotes).
            Regex r = new Regex(@"((\s*(""(?<param>.+?)""|(?<param>\S+))))",
                                RegexOptions.ExplicitCapture);
            MatchCollection m = r.Matches(_workingString);

            _splitParameters = new string[m.Count];
            for (int i = 0; i < m.Count; i++)
                _splitParameters[i] = m[i].Groups["param"].Value;
        }

        private void HandleSwitches() {
            if (_switches != null) {
                foreach (SwitchRecord s in _switches) {
                    Regex r = new Regex(s.Pattern,RegexOptions.ExplicitCapture| RegexOptions.IgnoreCase);
                    MatchCollection m = r.Matches(_workingString);
                        for (int i = 0; i < m.Count; i++) {
                            string value = null;
                            if (m[i].Groups != null && m[i].Groups["value"] != null)
                                value = m[i].Groups["value"].Value;

                            if (s.Type == typeof(bool)) {
                                bool state = true;
                                // The value string may indicate what value we want.
                                if (m[i].Groups != null && m[i].Groups["value"] != null) {
                                    switch (value) {
                                        case "+":
                                            break;
                                        case "-":
                                            state = false;
                                            break;
                                        case "":
                                            if (s.ReadValue != null)
                                                state = !(bool)s.ReadValue;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                s.Notify(state);
                                break;
                            }
                            if (s.Type == typeof(string))
                                s.Notify(value);
                            else if (s.Type == typeof(int))
                                s.Notify(int.Parse(value));
                            else if (s.Type.IsEnum)
                                s.Notify(Enum.Parse(s.Type, value, true));
                        }

                    _workingString = r.Replace(_workingString, " ");
                }
            }
        }





		#region public string[] UnhandledSwitches
        /// <summary>This function returns a list of the unhandled switches
        /// that the parser has seen, but not processed.</summary>
        /// <value></value>
        /// <remark>The unhandled switches are not removed from the remainder
        /// of the command-line.</remark>
        public string[] UnhandledSwitches {
            get {
                const string switchPattern = @"(\s|^)(?<match>(-{1,2}|/)(.+?))(?=(\s|$))";
                Regex r = new Regex(switchPattern,
                                     RegexOptions.ExplicitCapture
                                     | RegexOptions.IgnoreCase);
                MatchCollection m = r.Matches(_workingString);

                    string[] unhandled = new string[m.Count];
                    for (int i = 0; i < m.Count; i++)
                        unhandled[i] = m[i].Groups["match"].Value;
                    return unhandled;
            }
        }
		#endregion

		#region public void AddSwitch(string name, string description)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void AddSwitch(string name, string description) {
            if (_switches == null)
                _switches = new List<SwitchRecord>();

            SwitchRecord rec = new SwitchRecord(name, description);
            _switches.Add(rec);
        }
		#endregion

		#region public void AddSwitch(string[] names, string description)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        /// <param name="description"></param>
        public void AddSwitch(string[] names, string description) {
            if (_switches == null)
                _switches = new List<SwitchRecord>();
            SwitchRecord rec = new SwitchRecord(names[0], description);
            for (int s = 1; s < names.Length; s++)
                rec.AddAlias(names[s]);
            _switches.Add(rec);
        }
		#endregion

		#region public bool Parse()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Parse() {
            ExtractApplicationName();

            // Remove switches and associated info.
            HandleSwitches();

            // Split parameters.
            SplitParameters();

            return true;
        }
		#endregion

		#region public object InternalValue(string name)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object InternalValue(string name) {
            if (_switches != null)
                for (int i = 0; i < _switches.Count; i++)
                    if (string.Compare(_switches[i].Name, name, true) == 0)
                        return _switches[i].InternalValue;
            return null;
        }
		#endregion

        /* *******************************************************************
         *  Constructors
         * *******************************************************************/
        #region public Parser(string commandLine)
        /// <summary>
        /// Initializes a new instance of the <b>Parser</b> class.
        /// </summary>
        /// <param name="commandLine"></param>
        public Parser(string commandLine) {
            _commandLine = commandLine;
        }
        #endregion

        #region public Parser(string commandLine, object classForAutoAttributes)
        /// <summary>
        /// Initializes a new instance of the <b>Parser</b> class.
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="classForAutoAttributes"></param>
        public Parser(string commandLine, object classForAutoAttributes) {
            _commandLine = commandLine;

            Type type = classForAutoAttributes.GetType();
            System.Reflection.MemberInfo[] members = type.GetMembers();

            for (int i = 0; i < members.Length; i++) {
                object[] attributes = members[i].GetCustomAttributes(false);
                if (attributes.Length > 0) {
                    SwitchRecord rec = null;

                    foreach (Attribute attribute in attributes) {
                        if (attribute is CommandLineSwitchAttribute) {
                            CommandLineSwitchAttribute switchAttrib =
                                (CommandLineSwitchAttribute)attribute;

                            // Get the property information.  We're only handling
                            // properties at the moment!
                            if (members[i] is System.Reflection.PropertyInfo) {
                                System.Reflection.PropertyInfo pi = (System.Reflection.PropertyInfo)members[i];

                                rec = new SwitchRecord(switchAttrib.Name,switchAttrib.Description,pi.PropertyType)
                                {
                                    SetMethod = pi.GetSetMethod(),
                                    GetMethod = pi.GetGetMethod(),
                                    PropertyOwner = classForAutoAttributes
                                };

                                // Map in the Get/Set methods.

                                // Can only handle a single switch for each property
                                // (otherwise the parsing of aliases gets silly...)
                                break;
                            }
                        }
                    }

                    // See if any aliases are required.  We can only do this after
                    // a switch has been registered and the framework doesn't make
                    // any guarantees about the order of attributes, so we have to
                    // walk the collection a second time.
                    if (rec != null) {
                        foreach (Attribute attribute in attributes) {
                            if (attribute is CommandLineAliasAttribute) {
                                CommandLineAliasAttribute aliasAttrib =
                                    (CommandLineAliasAttribute)attribute;
                                rec.AddAlias(aliasAttrib.Alias);
                            }
                        }
                    }

                    // Assuming we have a switch record (that may or may not have
                    // aliases), add it to the collection of switches.
                    if (rec != null) {
                        if (_switches == null)
                            _switches = new List<SwitchRecord>();
                        _switches.Add(rec);
                    }
                }
            }
        }
        #endregion
        #region public override string ToString()
        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="Meridium.Configuration.CommandLine.Parser"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="Meridium.Configuration.CommandLine.Parser"/>.</returns>
        public override string ToString() {
            return _commandLine;
        }
        #endregion
    }
}