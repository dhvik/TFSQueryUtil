using System;
using System.Collections.Generic;

namespace Meridium.Configuration.CommandLine {
    /// <summary>
    /// The CommandLineParametersBase is a baseclass for creating a application specific command line parameters class.
    /// </summary>
    /// <remarks>
    /// 2009-06-23 dan: Created
    /// </remarks>
    public abstract class CommandLineParametersBase {
        /* *******************************************************************
         *  Properties 
         * *******************************************************************/
        private readonly Parser _parser;
        #region public bool DisplayHelp
        /// <summary>
        /// Get/Sets the DisplayHelp of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("help", "Displays help")]
        [CommandLineAlias("h")]
        [CommandLineAlias("?")]
        public bool DisplayHelp { get; set; }
        #endregion
        #region public bool AnswerYes
        /// <summary>
        /// Get/Sets the AnswerYes of the CommandLineParameters
        /// </summary>
        /// <value></value>
        [CommandLineSwitch("y", "Answers yes on all questions")]
        public bool AnswerYes { get; set; }
        #endregion
        #region protected List<string> Errors
        /// <summary>
        /// Gets the Errors of the BaseOperation
        /// </summary>
        /// <value></value>
        public List<string> Errors {
            get { return _errors; }
        }
        private readonly List<string> _errors = new List<string>();
        #endregion
        #region protected string HelpMessage
        /// <summary>
        /// Get/Sets the HelpMessage of the CommandLineParametersBase
        /// </summary>
        /// <value></value>
        protected string HelpMessage { get; set; }
        #endregion

        /* *******************************************************************
         *  Constructors 
         * *******************************************************************/
        #region protected CommandLineParametersBase()
        /// <summary>
        /// Initializes a new instance of the <b>CommandLineParameters</b> class.
        /// </summary>
        protected CommandLineParametersBase()
        {
            _parser = new Parser(Environment.CommandLine, this);
            _parser.Parse();
        }
        #endregion
        /* *******************************************************************
         *  Methods 
         * *******************************************************************/
        #region public static bool AskYesNo(string question)
        /// <summary>
        /// Displays a question and waits for the user to enter y or n
        /// </summary>
        /// <param name="question">The question to display</param>
        /// <returns>True if y was pressed, false if n was pressed</returns>
        public bool AskYesNo(string question) {
            if (AnswerYes)
                return true;
            Console.WriteLine(question + " [y|n]");
            while (true) {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Y)
                    return true;
                if (info.Key == ConsoleKey.N)
                    return false;
            }
        }
        #endregion
        #region public void ShowHelp()
        /// <summary>
        /// 
        /// </summary>
        public void ShowHelp() {
            if (Errors.Count > 0) {
                Console.WriteLine("Error found while parsing parameters for " + _parser);
                Console.WriteLine();
                foreach (string error in Errors) {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Run " + _parser.ApplicationName + " /? for more information");
                return;
            }
            Console.WriteLine(HelpMessage);
        }
        #endregion

        /* *******************************************************************
         *  Event methods 
         * *******************************************************************/
        #region public abstract int Execute()
        /// <summary>
        /// Executes the commandLine
        /// </summary>
        /// <returns>The return code of the operation where 0 means success and anything else is an error. (should be returned to the console and will be 
        /// visible as the %ERRORLEVEL% environment variable.</returns>
        public abstract int Execute();
        #endregion
    }
}