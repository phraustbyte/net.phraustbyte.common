using System;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// Attribute - indicates to the generator that the value is not to be utilized
    /// </summary>
    public class IdentifierAttribute : Attribute
    {
        /// <summary>
        /// indicates to the generator that the value is not to be utilized
        /// </summary>
        public bool Identifier { get; }
       // public bool IsArray { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        public IdentifierAttribute()
        {
            this.Identifier = true;
            //this.IsArray = false;
        }
        ///// <summary>
        ///// Default constructor indicating that there are multiple 
        ///// </summary>
        ///// <param name="IsArray"></param>
        //public IdentifierAttribute(bool IsArray)
        //{
        //    this.Identifier = true;
        //    this.IsArray = IsArray;
        //}
    }
}
