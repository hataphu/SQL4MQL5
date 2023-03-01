using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace SQL4MQL5
{
    public static class SQL4MQL5
    {
        private static SqlConnection conn = null;
        private static SqlCommand com = null;
        private static string sMessage = string.Empty;
        public const int iResSuccess = 0;
        public const int iResError = 1;

        [DllExport("CreateConnection", CallingConvention = CallingConvention.StdCall)]
        public static int CreateConnection([MarshalAs(UnmanagedType.LPWStr)] string sConnStr)
        {
            // Clear the message string:
            sMessage = string.Empty;
            // If a connection exists - close it and change the
            // connection string to a new one, if not -
            // re-create the connection and command objects:
            if (conn != null)
            {
                conn.Close();
                conn.ConnectionString = sConnStr;
            }
            else
            {
                conn = new SqlConnection(sConnStr);
                com = new SqlCommand();
                com.Connection = conn;
            }
            // Try to open the connection:
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                // The connection was not opened for some reason.
                // Write the error information to the message string:
                sMessage = ex.Message;
                // Release the resources and reset the objects:
                com.Dispose();
                conn.Dispose();
                conn = null;
                com = null;
                // Error:
                return iResError;
            }
            // Everything went well, the connection is open:
            return iResSuccess;
        }

        [DllExport("GetLastMessage", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string GetLastMessage()
        {
            return sMessage;
        }

        [DllExport("ExecuteSql", CallingConvention = CallingConvention.StdCall)]
        public static int ExecuteSql([MarshalAs(UnmanagedType.LPWStr)] string sSql)
        {
            // Clear the message string:
            sMessage = string.Empty;
            // First, check if the connection is established.
            if (conn == null)
            {
                // The connection is not open yet.
                // Report the error and return the error flag:
                sMessage = "Connection is null, call CreateConnection first.";
                return iResError;
            }
            // The connection is ready, try to execute the command.
            try
            {
                com.CommandText = sSql;
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Error while executing the command.
                // Write the error information to the message string:
                sMessage = ex.Message;
                // Return the error flag:
                return iResError;
            }
            // Everything went well - return the flag of the successful execution:
            return iResSuccess;
        }

        [DllExport("ReadInt", CallingConvention = CallingConvention.StdCall)]
        public static int ReadInt([MarshalAs(UnmanagedType.LPWStr)] string sSql)
        {
            // Clear the message string:
            sMessage = string.Empty;
            // First, check if the connection is established.
            if (conn == null)
            {
                // The connection is not open yet.
                // Report the error and return the error flag:
                sMessage = "Connection is null, call CreateConnection first.";
                return iResError;
            }
            // Variable to receive the returned result:
            int iResult = 0;
            // The connection is ready, try to execute the command.
            try
            {
                com.CommandText = sSql;
                iResult = (int)com.ExecuteScalar();
            }
            catch (Exception ex)
            {
                // Error while executing the command.
                // Write the error information to the message string:
                sMessage = ex.Message;
            }
            // Return the obtained result:
            return iResult;
        }


        [DllExport("ReadString", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static string ReadString([MarshalAs(UnmanagedType.LPWStr)] string sSql)
        {
            // Clear the message string:
            sMessage = string.Empty;
            // First, check if the connection is established.
            if (conn == null)
            {
                // The connection is not open yet.
                // Report the error and return the error flag:
                sMessage = "Connection is null, call CreateConnection first.";
                return string.Empty;
            }
            // Variable to receive the returned result:
            string sResult = string.Empty;
            // The connection is ready, try to execute the command.
            try
            {
                com.CommandText = sSql;
                sResult = com.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                // Error while executing the command.
                // Write the error information to the message string:
                sMessage = ex.Message;
            }
            // Return the obtained result:
            return sResult;
        }

        [DllExport("CloseConnection", CallingConvention = CallingConvention.StdCall)]
        public static void CloseConnection()
        {
            // First, check if the connection is established.
            if (conn == null)
                // The connection is not open yet - meaning it does not need to be closed either:
                return;
            // The connection is open - is should be closed:
            com.Dispose();
            com = null;
            conn.Close();
            conn.Dispose();
            conn = null;
        }
    }
}
