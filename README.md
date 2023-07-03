# Library

RESTful API service which exposes 3 endpoints:

1.	Endpoint to search books (/api/search). The service can search books using following criteria:
a)	by author
b)	by text in book’s title or description
c)	by user who’s currently holding the book
d)	if more than one criterion specified - parameter defining how multiple criterions must be combined – either OR or AND condition.
Results: list of books which meet criteria

3.	Endpoint to invert words in Title of the given book (/api/invertwords). Book is identified by ID. When called, service must invert all words in the Title of the book (words are sequences of characters and numbers separated by spaces or other signs like commas, semicolons etc.) 
Returns: book object

3.	Endpoint to generate report (/api/report) – list of users with total count of books taken and total count of days this user holds all the books.
Returns: User details, Total books he/she is holding, Total days he/she holds them.
