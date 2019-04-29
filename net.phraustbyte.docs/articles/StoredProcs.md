# SQL Stored Procedures - Guide
The basic premise behind the framework is to minimize the amount of effort that it takes to create a solution by handling
the backend work for you. Generally, that means less coding on the DAL and BLL side, but doesn't do much for SQL.
Regardless of the framework (MsSQL or MySQL), the process is more or less the same. For each CRUD operation, a corrisponding
procedure is required.

Notes: The Base BLL requires a number of fields that are, likewise, required in SQL. These fields should be entered into each table as follows:

 | FieldName   |  DataType        | Default      |
 :- | :- | :- |
 |Id          |UniqueIdentifier |NEWID()      |
 |Active      |BIT              |1            |
 |CreatedDate |DATETIME         |GETUTCDATE() |
 |Changer     |VARCHAR(20)      |             |

 Keep in mind that this is just a guide. As long as the parameters match, you could put whatever SQL code you want in the stored procedure.

## Read
The read methods (or Select methods) are used to retrieve one or more records from a database. Note: The DAL will require the names
of the stored procedures to be exactly the same.... however, by overriding the BaseDAL, it is possible to utilize reflection to
derive the names of the stored procedure if the names are consistant.
For example: utilizing the standard of `<Schema>.usp<TableName>_<Function>` will allow the DAL to determine, based on the name
of the class, which function it is calling. (A class of "Resource" could utilize `App.uspResource_Select` or `App.uspResources_Create`
without having to change a significant amount of code.)

### Select One
The read method requires one parameter - ID. Ergo, the following is the BASE of what you would need to create a read method

     CREATE PROCEDURE [App].[uspTable_Select]
     	@Id UNIQUEIDENTIFIER
     AS
     	SELECT 
     		Id,
     		CreatedDate,
     		Changer,
     		Active
     	FROM Table
     	WHERE Id = @Id

### Select All
Unlike the Select One method, the Select All method contains no parameters (hence the term "All"). Some use cases, however, do warrant
the use of a filter in which only active records are selected (active meaning not deleted).

     CREATE PROCEDURE [App].[uspTable_SelectAll]
     AS
     SELECT 
     	Id,
     	CreatedDate,
     	Changer,
     	Active
     FROM Table
     WHERE Active = 1 --OPTIONAL

### Select By Filter
The Select By Filter is a way to "customize" the items retrieved from the database. Unlike the previous two, the parameter 
is not as "defined"... At least it is defined on the SQL side, but on the C# side, it allows more flexability. The below example
uses the CreatedDate column as the filter

     CREATE PROCEDURE [App].[uspTable_SelectAllByFilter]
		@CreatedDate DATETIME
     AS
     SELECT 
     	Id,
     	CreatedDate,
     	Changer,
     	Active
     FROM Table
     WHERE CreatedDate = @CreatedDate

## Create
Create procedures require input parameter per public column in the C# class, with exception to the Id column which must be an
output parameter. The idea behind this requirement is that you can return the Id of the record created and utilize that to update
a table, or add to another record as a key.

     CREATE PROCEDURE [App].[uspTable_Insert]
     	@Id INT OUT,
     	@CreatedDate DATETIME = NULL,
     	@Changer VARCHAR(20) = NULL,
     	@Active BIT = NULL
     AS
     	INSERT INTO Table (CreatedDate,Changer,Active)
     		VALUES (GETUTCDATE(),@Changer,@Active)
     	SELECT @Id = SCOPE_IDENTITY()

## Update
Updates, like Creates, require one input parameter per public column in the C# class. The major difference is that every field
in the update is nullable, with exception of the Id field. The advantage here is that you can get the current record from
the table, and only update the fields that come across. In the below example, we will be updating a field called "Resource"

     CREATE PROCEDURE [App].[uspTable_Update]
     	@Id INT,
     	@CreatedDate DATETIME = NULL,
     	@Changer VARCHAR(20) = NULL,
     	@Active BIT = NULL,
     	@Resource INT = NULL
     AS
     	DECLARE @Current TABLE 
     	(	
     		[Id] INT,
     		[CreatedDate] DATETIME,
     		[Changer] VARCHAR(20),
     		[Active] BIT,
     		[Resource] INT
     	)
     	INSERT INTO @Current
     		SELECT * FROM Table 
     	WHERE [Id] = @Id;
     	UPDATE a SET
     		Resource = ISNULL(@Resource,c.Resource)
     	FROM Table a
     	JOIN @Current c
     		ON a.[Id] = c.[Id]

## Delete
Deletes, like creates, require one input parameter per public column in the C# class. The actual contents of the columns are irrelevant
with exception to the Id column, and can be marked nullable. 
Preferablly, records are not deleted, but rather marked as inactive. That way, if a user buggers something up, it is easier to restore

     CREATE PROCEDURE [App].[uspTable_Delete]
     	@Id INT,
     	@CreatedDate DATETIME = NULL,
     	@Changer VARCHAR(20) = NULL,
     	@Active BIT = NULL
     AS
     	UPDATE Table SET Active = 0 WHERE Id = @Id


# Audits
One recommendation, particularly for sensitive data, is to add audit functionality to each procedure. An easy way to do this is to create
an audit procedure that takes in a JSon or XML version of the table row before and after (where applicable), as well as the changer, table
and function being performed. This can be stored in another table (usually in it's own schema) for further usage, and could be purged after
a specified time depending on your requirements.

# Transactions
Each function noted here can be wrapped in a transaction.... If there is a failure with one of the functions, it should throw
an error, which would generate an exception on the C# side.