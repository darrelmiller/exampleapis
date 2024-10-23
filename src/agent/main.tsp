import "@typespec/http";
import "@typespec/openapi3";
import "@microsoft/typespec-copilot-skills";

using TypeSpec.Http;
using TypespecCopilotSkills;
using TypespecCopilotSkills.Agents;


/** Agent Examples has all kinds of examples for calling APIs */
@agent(
    "Agent Examples (v\${{TEAMS_APP_VERSION}})",
    "Agent Examples has all kinds of examples for calling APIs"
)
@instructions("""
    You are an agent that invokes a range of different functions for evaluating what kinds of functions Copilot can call successfully.
    Always use a function or tool to get information to response with. **DO NOT HALLUCINATE ANSWERS**.
    """)
@conversationStarter(#{
    text: "List all customers",
    title: "List all Customers",
})
namespace AgentExamples {

    op websearch is AgentCapabilities.WebSearch;

    /** API for ordering assets */
    @service()
    @skill({
        nameForHuman: "Examples API";
        descriptionForModel: "Search for customers, and lots of other stuff";
    })
    @server("https://0l7j96tl-5056.use.devtunnels.ms","Assets API")
    namespace ExamplesApi
    {
        /** List all available customers  */
        @route("/customers")
        @state(orchestratorState.reasoning,#{
            description: """
>>> help(examplesapi_GetCustomers)
examplesapi_GetCustomers(name: str = None) -> str
List all available customers 
    :param name: name
""",
            instructions: "Invoke this tool to get a list of customers. Provide a name query parameter to filter the results."
        })
        @get op GetCustomers(@query name?: string, @header prefer?: string): string;

        /** Create a customer */
        @route("/customers")
        // @state(orchestratorState.reasoning,#{
        //     description: "Create a customer",
        //     instructions: "Do not provide an id value when creating a customer. The API will generate one for you."
        // })
        @post op CreateCustomer(@body customer: Customer): Customer;
    }

    model Customer {
        id?: string;
        name: string;
        isActive?: boolean;
        createdDateTime?: utcDateTime;
        address?: Address;
        billingAddress?: Address;
    }
    model Address {
        street?: string;
        city?: string;
        state?: string;
        zip?: string;
        country?: string;
    }
}

enum orchestratorState {
    reasoning,
    responding
}