using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Xml.Serialization;
using System.IO;


public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void uspSplitString(string str, string sep)
    {
        SqlPipe pipe = SqlContext.Pipe;

        String[] subStrings = str.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);

        SqlDataRecord record = new SqlDataRecord(
               new SqlMetaData("substringIndex", SqlDbType.Int),
               new SqlMetaData("substring", SqlDbType.NVarChar, 100)
               );

        // Mark the begining of the result-set.
        SqlContext.Pipe.SendResultsStart(record);

        int index = 0;
        foreach (string subStr in subStrings)
        {
            // Set values for each column in the row.
            record.SetInt32(0, index++);
            record.SetString(1, subStr);

            // Send the row back to the client.
            SqlContext.Pipe.SendResultsRow(record);
        }

        // Mark the end of the result-set.
        SqlContext.Pipe.SendResultsEnd();
    }
};
