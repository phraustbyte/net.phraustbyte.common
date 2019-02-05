using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace net.phraustbyte.dal.dynamodb
{
    public class BaseDAL : IBaseDAL
    {
        private static AmazonDynamoDBClient _client { get; set; }
        private RegionEndpoint AWSRegion { get; set; }
        private string AWSProfileName { get; set; }
        private string ServiceUrl { get; set; }
        protected AmazonDynamoDBClient Client {
            get {
                if (_client is null)
                {
                    AmazonDynamoDBConfig clientConfig;
                    if (String.IsNullOrEmpty(ConnectionString))
                        return null;
                    if (AWSRegion is null)
                        if (String.IsNullOrEmpty(ServiceUrl))
                            throw new Exception("Invalid Connection String");
                        else 
                            clientConfig = new AmazonDynamoDBConfig { ServiceURL = this.ServiceUrl };
                    else
                        clientConfig = new AmazonDynamoDBConfig { RegionEndpoint = this.AWSRegion };
                    _client = new AmazonDynamoDBClient(clientConfig);
                }
                return _client;
            }
        }

        public string ConnectionString { get; }

        public string Query { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BaseDAL(string connectionString)
        {
            //ServerUrl=;Region=;ProfileName=;
            var keyValues = connectionString.Split(';');
            foreach (var key in keyValues)
            {
                var param = key.Split('=');
                switch (param[0])
                {
                    case "ServerUrl":
                        this.ServiceUrl = param[1]; break;
                    case "Region":
                        this.AWSRegion = RegionEndpoint.GetBySystemName(param[1]);
                        break;
                    case "ProfileName":
                        this.AWSProfileName = param[1];
                        break;
                    default:
                        break;
                }
            }
            this.ConnectionString = connectionString;
        }

        public async Task<TOut> Create<TIn, TOut>(TIn Obj)
        {
            var context = new DynamoDBContext(this.Client);
            await context.SaveAsync(Obj);
            return default(TOut);
        }

        public async Task Delete<T>(T Obj)
        {
            var context = new DynamoDBContext(this.Client);
            await context.DeleteAsync(Obj);
        }

        public List<IDataParameter> GetParameters<T>(T Obj)
        {
            throw new NotImplementedException();
        }

        public Task<TOut> Read<TIn, TOut>(TIn Id) where TOut : new()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ReadAll<T>() where T : new()
        {
            throw new NotImplementedException();
        }

        public Task<List<TOut>> ReadAllByFilter<TOut, TParam>(TParam FilterValue, string FilterKey) where TOut : new()
        {
            throw new NotImplementedException();
        }

        public async Task Update<T>(T Obj)
        {
            var context = new DynamoDBContext(this.Client);
            await context.
        }
    }
}
