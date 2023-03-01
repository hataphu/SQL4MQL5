# SQL4MQL5
Một thư viện để tương tác với SQL Server trong MQL5

##Creating an Expert Advisor in MQL5
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
  
