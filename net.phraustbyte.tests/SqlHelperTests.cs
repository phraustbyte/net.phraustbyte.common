using net.phraustbyte.dal;
using System;
using System.Data;
using Xunit;

namespace net.phraustbyte.tests
{
    public class SqlHelperTests
    {
        [Fact]
        public void String_to_NVarChar_Success()
        {
            string _string = "String";
            var result = _string.GetType().GetDbType();
            Assert.Equal(SqlDbType.NVarChar, result);
        }
        [Fact]
        public void CharArray_to_NVarChar_Success()
        {
            char[] _char = { 'q', 'e' };
            var result = _char.GetType().GetDbType();
            Assert.Equal(SqlDbType.NVarChar, result);
        }
        [Fact]
        public void Byte_to_TinyInt_Success()
        {
            byte _byte = 0x0D;
            var result = _byte.GetType().GetDbType();
            Assert.Equal(SqlDbType.TinyInt, result);
        }
        [Fact]
        public void Short_to_SmallInt_Success()
        {
            short _short = 1;
            var result = _short.GetType().GetDbType();
            Assert.Equal(SqlDbType.SmallInt, result);
        }
        [Fact]
        public void Int_to_Int_Success()
        {
            int _int = 3;
            var result = _int.GetType().GetDbType();
            Assert.Equal(SqlDbType.Int, result);
        }
        [Fact]
        public void Long_to_BigInt_Success()
        {
            long _long = 3214214132;
            var result = _long.GetType().GetDbType();
            Assert.Equal(SqlDbType.BigInt, result);
        }
        [Fact]
        public void ByteArray_to_VarBinary_Success()
        {
            byte[] _byteArray = { 0x32, 0x34, 0x36 };
            var result = _byteArray.GetType().GetDbType();
            Assert.Equal(SqlDbType.VarBinary, result);
        }
        [Fact]
        public void Bool_to_Bit_Success() {
            bool _bool = true;
            var result = _bool.GetType().GetDbType();
            Assert.Equal(SqlDbType.Bit, result);
        }
        [Fact]
        public void DateTime_to_DateTime2_Success() {
            DateTime _dateTime = DateTime.UtcNow;
            var result = _dateTime.GetType().GetDbType();
            Assert.Equal(SqlDbType.DateTime2, result);
        }
        [Fact]
        public void DateTimeOffset_to_DateTimeOffset_Success() {
            DateTimeOffset _dateTimeOffset = new DateTimeOffset();
            var result = _dateTimeOffset.GetType().GetDbType();
            Assert.Equal(SqlDbType.DateTimeOffset, result);
        }
        [Fact]
        public void Decimal_to_Money_Success() {
            decimal _decimal = 23.32M;
            var result = _decimal.GetType().GetDbType();
            Assert.Equal(SqlDbType.Money, result);
        }
        [Fact]
        public void Float_to_Real_Success() {
            float _float = 0.23456F;
            var result = _float.GetType().GetDbType();
            Assert.Equal(SqlDbType.Real, result);
        }
        [Fact]
        public void Double_to_Float_Success() {
            double _double = 0.2332;
            var result = _double.GetType().GetDbType();
            Assert.Equal(SqlDbType.Float, result);
        }
        [Fact]
        public void TimeSpan_to_Time_Success() {
            TimeSpan _TimeSpan = new TimeSpan(5, 4, 3, 2, 100);
            var result = _TimeSpan.GetType().GetDbType();
            Assert.Equal(SqlDbType.Time, result);
        }
        [Fact]
        public void Guid_to_UniqueIdentifier_Success() {
            Guid _Guid = new Guid();
            var result = _Guid.GetType().GetDbType();
            Assert.Equal(SqlDbType.UniqueIdentifier, result);
        }
        [Fact]
        public void Object_ThrowsException()
        {
            object obj = new object();
            Assert.Throws<ArgumentException>(() =>obj.GetType().GetDbType());
        }
        [Fact]
        public void TestDefaultTranslation()
        {
            //var x = default(Guid);
            var x = (Guid)Activator.CreateInstance(typeof(Guid));
            Guid guid = new Guid();
            Assert.Equal(guid, x);
        }
    }
}
