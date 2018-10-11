using System;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// Attribute - indicates the value of the property is an AutoIncremental
    /// </summary>
    public class AutoIncAttribute:Attribute
    {
        /// <summary>
        /// indicates the value of the property is an AutoIncremental
        /// </summary>
        public bool AutoInc { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        public AutoIncAttribute()
        {
            this.AutoInc = true;
        }
    }
}
