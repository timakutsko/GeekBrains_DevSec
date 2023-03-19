using Grpc.Core;
using System;
using System.Threading.Tasks;
using ClientServiceProtos;
using static ClientServiceProtos.ClientService;

namespace CardStorageService.Services.Impl
{
    internal class GRPC_ClientService : ClientServiceBase
    {
        private readonly IClientService _service;

        public GRPC_ClientService(IClientService service)
        {
            _service = service;
        }

        public override Task<CreateClientResponse> Create(CreateClientRequest request, ServerCallContext context)
        {
            try
            {
                var clientId = _service.Create(new Data.Models.Client
                {
                    Surname = request.Surname,
                    FirstName = request.FirstName,
                });

                return Task.FromResult(new CreateClientResponse
                {
                    ClientId = clientId,
                    ErrorCode = 0,
                    ErrorMessage = String.Empty,
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CreateClientResponse
                {
                    ClientId = -1,
                    ErrorCode = 912,
                    ErrorMessage = $"Create client error: {ex.Message}",
                });
            }
        }

        public override Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
        {
            try
            {
                var client = _service.GetById(request.Id);

                return Task.FromResult(new GetByIdResponse
                {
                    Client = new Client
                    {
                        Id = client.Id,
                        Surname = client.Surname,
                        FirstName = client.FirstName,
                    },
                    ErrorCode = 0,
                    ErrorMessage = String.Empty,
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new GetByIdResponse
                {
                    Client = null,
                    ErrorCode = 901,
                    ErrorMessage = $"Get client error: {ex.Message}",
                });
            }
        }
    }
}
