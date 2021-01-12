using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
//            var input = new HelloRequest{Name = "Tim"};
//            var channel = GrpcChannel.ForAddress("https://localhost:5001");
//            var client = new Greeter.GreeterClient(channel);
//            var reply = await client.SayHelloAsync(input);
//            Console.WriteLine(reply.Message);

               var channel = GrpcChannel.ForAddress("https://localhost:5001");
               var customerClient = new Customer.CustomerClient(channel);

               var clientRequested = new CustomerLookupModel {UserId = 2};

               var customer = await customerClient.GetCustomerInfoAsync(clientRequested);

            Console.WriteLine($"{customer.FirstName} {customer.LastName}");

            Console.WriteLine();
            Console.WriteLine("New Customer List");
            Console.WriteLine();

            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCusomer = call.ResponseStream.Current;
                    Console.WriteLine($"{currentCusomer.FirstName} {currentCusomer.LastName} : {currentCusomer.EmailAddress}");
                }
            }

            Console.ReadLine();
        }
    }
}
