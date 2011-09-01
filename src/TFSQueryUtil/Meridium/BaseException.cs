using System;

namespace Meridium {
	/// <summary>
	/// Summary description for BaseException.
	/// </summary>
	[Serializable]
	public abstract class BaseException : Exception {
		/* *******************************************************************
		 *  Static Methods
		 * *******************************************************************/
		#region public static string ExceptionToString(Exception ex)
		/// <summary>
		/// Creates a string of the exception that is a combination of all inner exceptions found, 
		/// plus the stacktrace
		/// </summary>
		/// <param name="ex">The exception to read from </param>
		/// <returns>A descriptive text</returns>
		public static string ExceptionToString(Exception ex) {
			return ExceptionToString(ex,null);
		}
		#endregion
		#region public static string ExceptionToString(Exception ex, string innerMessage)
		/// <summary>
		/// Creates a string of the exception that is a combination of all inner exceptions found, 
		/// plus the stacktrace
		/// </summary>
		/// <param name="ex">The exception to read from </param>
		/// <param name="innerMessage">An inner message to be prepended to the list of messages</param>
		/// <returns>A descriptive text</returns>
		public static string ExceptionToString(Exception ex, string innerMessage) {
			if (ex == null) {
				throw new ArgumentNullException("ex");
			}
			string message = ex.GetType().FullName+" : ";
			if(innerMessage!=null)
				message+=innerMessage+" ";
			message+=ex.Message;
			if(ex.InnerException != null)
				message+="\n"+ExceptionToString(ex.InnerException,null);
			return message+Environment.NewLine+ex.StackTrace;
		}
		#endregion
	}
}
