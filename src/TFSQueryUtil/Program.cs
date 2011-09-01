using System;
using Meridium;

namespace TFSQueryUtil {
    class Program {
        #region static int Main()
        /// <summary>
        /// The main method of the application.
        /// </summary>
        /// <returns></returns>
        static int Main() {

            try {
                return new CommandLineParameters().Execute();
            } catch (Exception e) {
                Console.WriteLine("Error in application");
                Console.WriteLine(BaseException.ExceptionToString(e));
                return 1;
            }

        }
        #endregion

    }
}
