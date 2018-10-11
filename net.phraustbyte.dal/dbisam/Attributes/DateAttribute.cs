using System;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// Attribute - indicates the field is a Date field
    /// </summary>
    public class DateAttribute:Attribute
    {
        /// <summary>
        /// indicates the field is a date field
        /// </summary>
        public bool Date { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        public DateAttribute()
        {
            Date = true;
        }
    }
}
