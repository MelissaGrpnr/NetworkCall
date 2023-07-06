# NetworkCall
1.	First, we include the necessary namespaces (System, System.Collections.Generic, System.IO, System.Net, System.Text, System.Web).

2.	Within the SimpleRestService namespace, we define the Program class, which represents our REST service.

3.	We create a List<Book> named books. This list represents a collection where books are stored.

4.	We add sample books to the books list.

5.	We start the server using the HttpListener class, which is used to listen for HTTP requests.

6.	We specify the URL that the server will listen to with the line listener.Prefixes.Add("http://localhost:8080/");. In this example, the server listens to the address http://localhost:8080/.

7.	We start the server: listener.Start();

8.	We start an infinite loop. Inside this loop, we will write the code to listen for and process incoming requests.

9.	We capture the incoming request using the listener.GetContext() method and retrieve the HttpListenerContext object.

10.	We process the incoming request using the ProcessRequest method.

11.	We check the request method (GET, POST, etc.) to determine how to handle the request.

12.	If the request is a GET request, we call the GetBooks method to retrieve the books and return the result in JSON format.

13.	If the request is a POST request, we call the AddBook method to add the book from the request content. Then, we return a success message in JSON format.

14.	We set the response content type to JSON: context.Response.ContentType = "application/json";

15.	We convert the JSON response data into bytes: byte[] buffer = Encoding.UTF8.GetBytes(responseJson);

16.	We set the response length: context.Response.ContentLength64 = buffer.Length;

17.	We send the response: context.Response.OutputStream.Write(buffer, 0, buffer.Length);

18.	Finally, we close the connection: context.Response.OutputStream.Close();

19.	The GetBooks method converts the list of books to JSON format and returns it.

20.	The AddBook method parses the JSON request content into a Book object, generates a unique ID for the new book, and adds it to the list of books.


   
