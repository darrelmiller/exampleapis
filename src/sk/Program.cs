// See https://aka.ms/new-console-template for more information

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.OpenApi;

var kernel = new Kernel();

#pragma warning disable SKEXP0040

var kernelPlugin = await kernel.ImportPluginFromOpenApiAsync(
   pluginName: "examples",
   filePath: "D:/github/darrelmiller/exampleApis/src/agent/.generated/plugins/examplesapi-openapi.yml",
   executionParameters: new OpenApiFunctionExecutionParameters()
   {
      // Determines whether payload parameter names are augmented with namespaces.
      // Namespaces prevent naming conflicts by adding the parent parameter name
      // as a prefix, separated by dots
      EnablePayloadNamespacing = true
   }
);

#pragma warning restore SKEXP0040

var createCustomer = kernelPlugin["CreateCustomer"];
var createCustomerArgs = new KernelArguments
{
    { "name", "bob" },
    { "address_city", "montreal" },
    { "billingaddress_city", "toronto" }
};
var createResult = await kernel.InvokeAsync(createCustomer, createCustomerArgs);
Console.WriteLine(createResult.ToString());

var getCustomers = kernelPlugin["GetCustomers"];

var result = await kernel.InvokeAsync(getCustomers, new KernelArguments());
var response = result.GetValue<RestApiOperationResponse>();
var content = response.Content;
Console.WriteLine(content);
