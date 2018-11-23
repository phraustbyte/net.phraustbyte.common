using System;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// Attribute - sets the name of the table
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// represents the table name
        /// </summary>
        public string TableName { get; }
        /// <summary>
        /// Default cosntructor
        /// </summary>
        /// <param name="TableName"></param>
        public TableAttribute(string TableName)
        {
            this.TableName = TableName;
        }
    }
    
}
