# Business Logic Layer
There are two options with the BLL. Either the interface `IBaseBLL` or the abstract class `BaseBLL` can be used from the library. The abstract class
inherits from the interface already, so the prefered method is to use the abstract class.
## Members
By Default the interface has the following members:

     Guid Id { get; set; }
     DateTime CreatedDate { get; set; }
     string Changer { get; set; }
     bool Active { get; set; }

These members are required for use with the BaseBLL and the `Id` field and `Changer` field are required for the DAL. 

The Abstract class also contains the member DAL which represents the BaseDAL interface

     abstract IBaseDAL DAL {get;}

When inheriting the BaseBLL, this member is required. The recommended usage is:

     protected override IBaseDAL DAL { get; }

## Constructor
As the DAL is protected and only will allow sets as part of the contructor, it is recommended that the DAL be set as part of the default constructor.

     public InheritedClass() {
	 	 DAL = new BaseDAL()
	 }


## Methods
The Business Logic Layer is designed to apply some logic to the methods used for CRUD operations before sending them to the database. With that
in mind, the following examples are base/core code - the minimum required to use the crud operations.

### Create

        public async override Task<Guid> Create<T>()
        {
            return await DAL.Create(this);
        }

### Delete

        public async override Task Delete()
        {
            await DAL.Delete(this);
        }

### Read (One Record)

        public async override Task<T> Read<T>(Guid Id)
        {
            return await DAL.Read<T>(Id);
        }

### Read (All Records)

        public async override Task<List<T>> ReadAll<T>()
        {
            return await DAL.ReadAll<T>();
        }

### Read (By Filter)

        public async override Task<List<TOut>> ReadAllByFilter<TOut, TParam>(TParam FilterValue, string FilterKey)
        {
            return await DAL.ReadAllByFilter<TOut, TParam>(FilterValue, FilterKey);
        }

### Update

        public async override Task Update()
        {
            await DAL.Update(this);
        }

  
