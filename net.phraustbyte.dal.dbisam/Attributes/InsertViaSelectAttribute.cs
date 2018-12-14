using System;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// attribute - indicates what type of insert statement should be used
    /// </summary>
    public class InsertViaSelectAttribute:Attribute
    {
        /// <summary>
        /// indicates what type of insert statement should be used
        /// </summary>
        public bool InsertViaSelect { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        public InsertViaSelectAttribute()
        {
            this.InsertViaSelect = true;
        }
    }
}
