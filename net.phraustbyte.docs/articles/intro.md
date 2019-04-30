# Backend Generation
*2019-04-27 Graham A Campbell*

The backend libraries are designed to expidite the creation of both Data and Business Logic Layers. 
Basically, it creates an alternative to things like Entity Framework with the added advantage of being able to easily swap the backend simply by changing the interface.


## Base Interfaces / Abastrct Class
There are two base interfaces and one abstract class that creates the foundation of the libraries. 

### Base DAL
The BaseDAL contains an interface with 7 methods that facilitate actions that will be performed by the selected backend database. 
There are also 2 properties representing the query (or usp if applicable) and connection strings.

### Base BLL
The BaseBLL contains both an interface and an abstract class. The interface contains some default properties that are fairly standard across database designs (such as Id, change user, change date, etc).
The interface also contains similar methods to the DAL that facilitate CRUD operations.

The Abstract class extends the interface further, including a property for the DAL. 


## Extended Classes
### DAL DBISAM
Look, if you have never heard of this, consider yourself lucky. It is mostly used with Pascal... but in this case, it was needed for use with a ODBC connection to a database for use with a pascal application. Do not ask.
In order to make it work, it is likely that most of the operations will have to be overridden anyway. 

### DAL MsSQL
This class is the most extensively tested backend framework. The whole concept was really designed around M$ SQL. It essentially provides a backend option for the baseDAL interface.
Connect this up to the code, and it *should* allow pretty straight-forward connection to the database.... However, the stored procedures do have to follow a fairly specific format.

### DAL MySQL
This class is modeled after the M$ SQL version. It is largly untested, but *should* work. 