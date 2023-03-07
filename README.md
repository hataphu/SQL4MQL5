# SQL for MQL5
A library for interacting with SQL Server in MQL5

# Creating an Expert Advisor in MQL5

Let us create a simple expert in MQL5. Its code can also be complied in the MQL4 editor, by changing the extension from "mq5" to "mq4". This expert is only for demonstrating the successful work with the database, so it will not perform any trading operations.

Run MetaEditor, press the "New" button. Select "Expert Advisor (template)" and press "Next". Specify the name "MqlSqlDemo". Also add one parameter — "ConnectionString" of type "string". This will be the connection string indicating how to connect to your database server. For example, you can set this initial value for the parameter:

Server=localhost;Database=master;Integrated Security=True

This connection string allows connecting to an unnamed ("Default Instance") database server installed on the same computer, where the MetaTrader terminal is running. There is no need to specify login and password — authorization by Windows account is used.

If you downloaded SQL Server Express and installed it on your computer without changing the parameters, then your SQL Server will be a "named instance". It will receive the name "SQLEXPRESS". It will have a different connection string:

Server=localhost\\SQLEXPRESS;Database=master;Integrated Security=True

When adding a string parameter to the Expert Advisor template, there is a limitation on the string size. A longer connection string (for example, to a named server "SQLEXPRESS") may not fit. But this is not a problem — the parameter value can be left blank at this stage. It can later be changed to any value when editing the expert code. It is also possible to specify the required connection string when launching the expert.
Click "Next". No more functions need to be added, so leave all the checkboxes on the next screen unchecked. Press "Next" again and receive the generated initial code for the expert.

The purpose of the expert is only to demonstrate the connection to the database and work with it. To do this, it is sufficient to use only the initialization function — OnInit. Drafts for other functions — OnDeinit and OnTick — can be removed right away.

As a result, we obtain the following:

  
```

//+------------------------------------------------------------------+
//|                                                   MQL5Demo.mq5 |
//|                        Copyright 2018, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2018, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict
//--- input parameters
input string   ConnectionString = "Server=localhost\\SQLEXPRESS;Database=master;Integrated Security=True";
//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
int OnInit()
  {
//---
  
//---
   return(INIT_SUCCEEDED);
  }
  
```
  
# Please note:
when connecting to a named instance (in this case, "SQLEXPRESS") it is necessary to repeat the "\" character twice: "localhost\\SQLEXPRESS". This is required both when adding the parameter to the expert template and in the code. If the character is specified only once, the compiler treats it as if the escape sequence (special character) "\S" is specified in the string, and reports that it was not recognized during compilation.

However, when attaching a compiled robot to a chart, its parameters will have only one "\" character, despite that two of them are specified in the code. This happens because all Escape sequences in strings are converted into corresponding characters during compilation. The sequence "\\" is converted into a single "\" character, and users (who do not need to work with the code) see a normal string. Therefore, if you specify the connection string not in the code, but when starting the Expert Advisor, only a single "\" character should be specified in the connection string.

Server=localhost\SQLEXPRESS;Database=master;Integrated Security=True

Now let us add functionality to the draft expert. First, it is necessary to import the functions for working with the database from the created DLL. Add the import section before the OnInit function. The imported functions are described almost in the same way as they are declared in the C# code. It is only necessary to remove all the modifiers and attributes:
```
// Description of the imported functions.
#import "SQL4MQL5.dll"

// Function for opening a connection:
int CreateConnection(string sConnStr);
// Function for reading the last message:
string GetLastMessage();
// Function for executing the SQL command:
int ExecuteSql(string sSql);
// Function for reading an integer:
int ReadInt(string sSql);
// Function for reading a string:
string ReadString(string sSql);
// Function for closing a connection:
void CloseConnection();

// End of import:
#import
```
For greater clarity of the code, declare constants for the results of the function execution. As in the DLL, these will be 0 on successful execution and 1 on error:
```
// Successful execution of the function:
#define iResSuccess  0
// Error while executing the function:
#define iResError 1
```
Now we can add calls to the functions for working with the database to the OnInit initialization function. Here is how it will look:
```
int OnInit()
  {
   // Try to open a connection:
   if (CreateConnection(ConnectionString) != iResSuccess)
   {
      // Failed to establish the connection.
      // Print the message and exit:
      Print("Error when opening connection. ", GetLastMessage());
      return(INIT_FAILED);
   }
   Print("Connected to database.");
   // The connection was established successfully.
   // Try to execute queries.
   // Create a table and write the data into it:
   if (ExecuteSql(
      "create table DemoTest(DemoInt int, DemoString nvarchar(10));")
      == iResSuccess)
      Print("Created table in database.");
   else
      Print("Failed to create table. ", GetLastMessage());
   if (ExecuteSql(
      "insert into DemoTest(DemoInt, DemoString) values(1, N'Test');")
      == iResSuccess)
      Print("Data written to table.");
   else
      Print("Failed to write data to table. ", GetLastMessage());
   // Proceed to reading the data. Read an integer from the database:
   int iTestInt = ReadInt("select top 1 DemoInt from DemoTest;");
   string sMessage = GetLastMessage();
   if (StringLen(sMessage) == 0)
      Print("Number read from database: ", iTestInt);
   else // Failed to read number.
      Print("Failed to read number from database. ", GetLastMessage());
   // Now read a string:
   string sTestString = ReadString("select top 1 DemoString from DemoTest;");
   sMessage = GetLastMessage();
   if (StringLen(sMessage) == 0)
      Print("String read from database: ", sTestString);
   else // Failed to read string.
      Print("Failed to read string from database. ", GetLastMessage());
   // The table is no longer needed - it can be deleted.
   if (ExecuteSql("drop table DemoTest;") != iResSuccess)
      Print("Failed to delete table. ", GetLastMessage());
   // Completed the work - close the connection:
   CloseConnection();
   // Complete initialization:
   return(INIT_SUCCEEDED);
  }
  ```

Compile the expert. That is it, the test expert is ready. You can run it. Before running the expert, it is necessary to add the DLL to the libraries folder of the MetaTrader profile you use. Start MetaTrader, in the "File" menu, select "Open Data Folder". Open the "MQL5" folder (for MetaTrader 4, the "MQL4" folder), then the "Libraries" folder. Place the created DLL file (MqlSqlDemo.dll) to this folder. The expert should already be compiled and ready for use by this time. Naturally, running Expert Advisors and importing functions from DLL should be allowed in the MetaTrader 5 settings, otherwise it will immediately fail with an error at startup.

Start the expert, changing the connection string values to your database server access parameters. If everything is done correctly, the expert will output the following to the log:
```
2018.07.10 20:36:21.428    MqlSqlDemo (EURUSD,H1)    Connected to database.
2018.07.10 20:36:22.187    MqlSqlDemo (EURUSD,H1)    Created table in database.
2018.07.10 20:36:22.427    MqlSqlDemo (EURUSD,H1)    Data written to table.
2018.07.10 20:36:22.569    MqlSqlDemo (EURUSD,H1)    Number read from database: 1
2018.07.10 20:36:22.586    MqlSqlDemo (EURUSD,H1)    String read from database: Test
```
# Good luck !
