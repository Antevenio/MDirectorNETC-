using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication2;
using Newtonsoft.Json; //install JSON.NET via nuget

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //get request

            string email = "soa@gmail.com";
            string url = "http://www.mdirector.com/api_contact?email="+email;
            string consumerKey = "52216";
            string secretKey = "Qdl0AVLxIk";
            string contentType = "application/x-www-form-urlencoded";


            ServiceProvider ServiceProvider = new ServiceProvider(url, consumerKey, secretKey, "PLAINTEXT");
            string resultGet = ServiceProvider.GetData(contentType);

            var joGet = Newtonsoft.Json.Linq.JObject.Parse(resultGet);

            string resultado = joGet["response"].ToString();

            if (resultado == "ok") {
                Console.Out.WriteLine("Obtained user ok");
                Console.Out.WriteLine(resultGet);
                Console.Read();
            } else {
                Console.Out.WriteLine("Failed obtaining user");
                Console.Out.WriteLine(resultGet);
                Console.Read();
            }
            

            //post request

            /*
            string url = "http://www.mdirector.com/api_contact";
            string consumerKey = "52216";
            string secretKey = "Qdl0AVLxIk";
            string contentType = "application/x-www-form-urlencoded";

           
            ServiceProvider ServiceProvider = new ServiceProvider(url, consumerKey, secretKey, "PLAINTEXT");


            string data = "listId=1&name=Soa&surname=Gya&email=soa@gmail.com";


            string result = ServiceProvider.PostData(contentType, data);

            var jo = Newtonsoft.Json.Linq.JObject.Parse(result);

            string resultado = jo["response"].ToString();
            if (resultado == "ok") {
                Console.Out.WriteLine("Inserted user ok");
                Console.Out.WriteLine(result);
                Console.Read();
            } else {

                Console.Out.WriteLine("Failed Inserting new user");
                Console.Out.WriteLine(result);
                Console.Read();
            }

            */

            //delete request
            
            /*
            string url = "http://www.mdirector.com/api_contact";
            string consumerKey = "52216";
            string secretKey = "Qdl0AVLxIk";
            string contentType = "application/x-www-form-urlencoded";


            ServiceProvider ServiceProvider = new ServiceProvider(url, consumerKey, secretKey, "PLAINTEXT");


            string data = "conId=1&email=sergiogayarre@gmail.com&unsubscribe=true&reason=baja";


            string responseDelete = ServiceProvider.DeleteData(contentType, data);


            var joDelete = Newtonsoft.Json.Linq.JObject.Parse(responseDelete);

            string resultDeleted = joDelete["response"].ToString();

            if (resultDeleted == "ok") {
                Console.Out.WriteLine("Unsubscribed user ok");
                Console.Out.WriteLine(responseDelete);
                Console.Read();
            } else {

                Console.Out.WriteLine("Failed unsubscribing user");
                Console.Out.WriteLine(responseDelete);
                Console.Read();
            }

            */

              //update request
            
            /*
            string url = "http://www.mdirector.com/api_contact";
            string consumerKey = "52216";
            string secretKey = "Qdl0AVLxIk";
            string contentType = "application/x-www-form-urlencoded";


            ServiceProvider ServiceProvider = new ServiceProvider(url, consumerKey, secretKey, "PLAINTEXT");

            string data = "listId=1&conId=10&email=soa@gmail.com&name=paquito";
            string responseUpdate = ServiceProvider.PutData(contentType, data);

            var joUpdate = Newtonsoft.Json.Linq.JObject.Parse(responseUpdate);
            string resultUpdated = joUpdate["response"].ToString();

            if (resultUpdated == "ok") {
                Console.Out.WriteLine("Updating user ok");
                Console.Out.WriteLine(responseUpdate);
                Console.Read();
            } else {
                Console.Out.WriteLine("Failed updating user");
                Console.Out.WriteLine(responseUpdate);
                Console.Read();
            }
            
            */
         

           
            

        }
    }
}
