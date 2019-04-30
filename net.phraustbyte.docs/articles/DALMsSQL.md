# Data Access Layer (MsSQL)
The DAL includes an interface, but also has packages for Microsoft SQL, MySQL and DBISAM. The MS SQL package is the most polished and tested, which is
why this is used as the example of how to use the DAL.

There are optinos as to the method that can be used with the DAL. One option is to create one DAL class per BLL class, but that's a lot of the same thing
over and over again. The below methods use reflection, so only one DAL is needed (per SQL Schema).

## Constructor
The constructor requires a connection string regardless of which backend is used. I prefer to include that as part of the DAL rather then have the user
specify it in the BLL somewhere. While this article will not describe how to do this, I recommend putting it in the `appsettings.json` file and grabbing
it from there.

With that in mind, I use a constructor similar to the following:

     public DataLayer() : base(GetConnectionString()) { }

## Methods
These DAL methods are more or less the same. The idea is that it is fairly universal - as long as the stored procedures are all named with the same convention
`<Schema>.usp<TableName>_<Function>`, only one DAL is required per Schema. Therefore, the methods could look like this:

### Create
     
	 public override Task<Guid> Create<T>(T Obj)
     {
         var objName = Obj.GetType().Name;
         this.Query = $"dbo.usp{objName}_Insert";
         return base.Create<T>(Obj);
     }

### Delete

     public override Task Delete<T>(T Obj)
     {
         var objName = Obj.GetType().Name;
         this.Query = $"dbo.usp{objName}_Delete";
         return base.Delete(Obj);
     }

### Update

     public override Task Update<T>(T Obj)
     {
         var objName = Obj.GetType().Name;
         this.Query = $"dbo.usp{objName}_Update";
         return base.Update(Obj);
     }

### Read (One Record)

     public override Task<T> Read<T>(Guid Id)
     {
         var objName = typeof(T).Name;
         this.Query = $"dbo.usp{objName}_Select";
         return base.Read<T>(Id);
     }

### Read (All Records)

     public override Task<List<T>> ReadAll<T>()
     {
         var objName = typeof(T).Name;
         this.Query = $"dbo.usp{objName}_SelectAll";
         return base.ReadAll<T>();
     }

### Read (By Filter)

     public override Task<List<TOut>> ReadAllByFilter<TOut, TParam>(TParam FilterValue, string FilterKey)
     {
         var objName = typeof(TOut).Name;
         this.Query = $"dbo.usp{objName}_SelectAllByFilter";
         return base.ReadAllByFilter<TOut, TParam>(FilterValue, FilterKey);
     }
