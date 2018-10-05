using System;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// Attribute - indicates that the field is a time based field
    /// </summary>
    public class TimeAttribute : Attribute
    {
        /// <summary>
        /// indicates that the field is a time based field
        /// </summary>
        public bool Time { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        public TimeAttribute()
        {
            Time = true;
        }
    }
}
