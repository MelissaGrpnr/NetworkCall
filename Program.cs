using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace RestService
{
        class Program
        {
            static List<Book> books = new List<Book>();

            static void Main(string[] args)
            {
                // Add some sample books
                books.Add(new Book { Id = 1, Title = "Book 1", Author = "Author 1" });
                books.Add(new Book { Id = 2, Title = "Book 2", Author = "Author 2" });

                // Start the server
                using (HttpListener listener = new HttpListener())
                {
                    listener.Prefixes.Add("http://localhost:8080/");
                    listener.Start();
                    Console.WriteLine("Listening for requests on http://localhost:8080/");

                    while (true)
                    {
                        HttpListenerContext context = listener.GetContext();
                        ProcessRequest(context);
                    }
            }
            string getUrl = "http://localhost:8080/";
            Fetch(getUrl, "GET", null, (responseJson) =>
            {
                Console.WriteLine(responseJson);
            });

            string postUrl = "http://localhost:8080/";
            string requestBody = "{\"Title\":\"New Book\",\"Author\":\"New Author\"}";
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(requestBody);

            Fetch(postUrl, "POST", postData, (responseJson) =>
            {
                Console.WriteLine(responseJson);
            });

        }

            static void ProcessRequest(HttpListenerContext context)
            {
                string requestMethod = context.Request.HttpMethod;
                string responseJson = string.Empty;

                if (requestMethod == "GET")
                {
                    // Handle GET request
                    responseJson = GetBooks();
                }
                else if (requestMethod == "POST")
                {
                    // Handle POST request
                    string requestBody = new StreamReader(context.Request.InputStream).ReadToEnd();
                    AddBook(requestBody);
                    responseJson = "Book added successfully.";
                }

                // Set response content type to JSON
                context.Response.ContentType = "application/json";

                // Write the JSON response
                byte[] buffer = Encoding.UTF8.GetBytes(responseJson);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
            }

            static string GetBooks()
            {
                // Convert the list of books to JSON
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(books);
                return json;
            }

            static void AddBook(string requestBody)
            {
                // Parse the JSON request body to a Book object
                Book newBook = Newtonsoft.Json.JsonConvert.DeserializeObject<Book>(requestBody);

                // Generate a unique ID for the new book
                int newBookId = books.Count + 1;
                newBook.Id = newBookId;

                // Add the new book to the list
                books.Add(newBook);
            }
            static void Fetch(string url, string method, byte[] data, Action<string> callback)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;

                if (method == "POST")
                {
                    request.ContentType = "application/json";
                    request.ContentLength = data.Length;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(data, 0, data.Length);
                    }
                }

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseJson = reader.ReadToEnd();
                            callback(responseJson);
                        }
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
        }


    }
