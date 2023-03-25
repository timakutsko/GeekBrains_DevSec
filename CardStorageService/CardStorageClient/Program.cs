using Grpc.Net.Client;
using System;
using ClientServiceProtos;
using static ClientServiceProtos.ClientService;

namespace CardStorageClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (GrpcChannel chanel = GrpcChannel.ForAddress("https://localhost:5001"))
            {
                ClientServiceClient serviceClient = new ClientServiceClient(chanel);

                Console.Write("Укажи имя нового сотрудника: ");
                var userFirstNameInput = Console.ReadLine();
                Console.Write("Укажи фамилию нового сотрудника: ");
                var userSurnameInput = Console.ReadLine();
                var createClientResponse = serviceClient.Create(new CreateClientRequest
                {
                    FirstName = userFirstNameInput,
                    Surname = userSurnameInput,
                });
                if (createClientResponse.ErrorCode == 0)
                {
                    Console.WriteLine($"Инфо по запрашиваемому клиенту:" +
                        $"\nId: {createClientResponse.ClientId}");
                }
                else
                    Console.WriteLine($"Возникла проблема при создании клиента: {createClientResponse.ErrorCode} - {createClientResponse.ErrorMessage}");


                Console.Write("Укажи id для поиска: ");
                var userIdInput = Console.ReadLine();
                if (int.TryParse(userIdInput, out int id))
                {
                    var getByIdResponse = serviceClient.GetById(new GetByIdRequest
                    {
                        Id = id,
                    });

                    if (getByIdResponse.ErrorCode == 0)
                    {
                        Console.WriteLine($"Инфо по запрашиваемому клиенту:" +
                            $"\nId: {getByIdResponse.Client.Id} / FirstName: {getByIdResponse.Client.FirstName} / LastName: {getByIdResponse.Client.Surname}");
                    }
                    else
                        Console.WriteLine($"Возникла проблема при поиске клиента: {getByIdResponse.ErrorCode} - {getByIdResponse.ErrorMessage}");
                }
            
                Console.ReadKey();
            }
        }
    }
}
