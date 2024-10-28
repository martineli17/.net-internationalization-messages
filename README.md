# CultureInfo - Definindo retorno de mensagens de acordo com a cultura informada

Algumas aplicações de mercado exigem que as mensagens de retorno sejam adpatadas de acordo com a cultura na o request foi feito. 

Nesse contexto, o .NET consegue facilmente fazer esse trabalho de maneira fluída utilizando os próprios Resources da plataforma.

## Configurações

### 1. Definição dos Resources
Para armazenar as mensagens de retorno de acordo com cada uma da linguagem (cultura) desejada, é necessário definir um resource específico para elas. O detalhe é que o nome do Resource precisa terminar (antes da extensão do arquivo) com o nome da cultura.

Um exemplo: para definir um Resource para a cultura de EN-US, o nome do arquivo ficaria da seguinte maneira: `Messages.en-US.resx`. 

Neste exemplo, foi definido alguns Resources: um `padrão` (sem identificação da cultura no nome do arquivo), um para a linguagem `EN-US` e outro para a linguagem `PT-BR`.

[Resource padrão](https://github.com/martineli17/.net-internationalization-messages/blob/master/Api/Resources/Messages/ResourceMessages.resx)

[Resource EN-US](https://github.com/martineli17/.net-internationalization-messages/blob/master/Api/Resources/Messages/ResourceMessages.en-US.resx)

[Resource PT-BR](https://github.com/martineli17/.net-internationalization-messages/blob/master/Api/Resources/Messages/ResourceMessages.pt-BR.resx)

> Observação: Caso nennhuma cultura seja explícita no nome do arquivo, o .NET irá entender que aquele arquivo é o 'padrão' para ser utilizado quando nenhum arquivo é encontrado para a linguagem que foi requerida.

### 2. Middleware para definir a CultureInfo automaticamente pelo request via query params
Para que a CultureInfo da uma Thread seja definida automaticamente ao processar um request, o .NET disponibiliza um middleware para realizar esse processamento e realizar a definição de maneira fluída e sem esforços. Ele busca a cultura via queryparams, no qual o parâmetro `culture` é o responsável por essa definição.

Para aplicar esse recurso, é necessário adicionar o `service` e o `middleware`:

```csharp
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // CULTURAS PERMITIDAS
    string[] supportedCultures = ["pt-BR", "en-US"];
    options
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});
```

```csharp
app.UseRequestLocalization();
```

### 3. Utilização
Os resources foram utilizados em controller, para exemplificar o uso.
```csharp
public class MessageController : ControllerBase
    {
        [HttpGet("messages:success")]
        public IActionResult Success()
        {
            return Ok(new { Value = ResourceMessages.Success });
        }

        [HttpGet("messages:error")]
        public IActionResult Error()
        {
            return Ok(new { Value = ResourceMessages.Error });
        }

        [HttpGet("messages:culture")]
        public IActionResult Culture()
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            return Ok(new { Value = currentCulture });
        }
    }
```

Com o middleware e o service definidos, a definição da cultura pode ser definida via query params usando o param `culture`, por exemplo: `http://localhost:5075/messages:success?culture=pt-BR`

- Mensagem em `pt-BR` (retornanda via ResourceMessages.pt-BR.resx)
![{204C7841-20DC-4779-8D91-25061A72735B}](https://github.com/user-attachments/assets/8d65f64e-8a87-4147-b33d-6a9c31a4dc47)

- Mensagem em `en-US` (retornanda via ResourceMessages.en-US.resx)
![{6290EBAE-8B38-43B5-BB26-6EC6CAAF4478}](https://github.com/user-attachments/assets/6e8638bd-8fb4-412b-9dda-975f41947550)

- Mensagem padrão (retornanda via ResourceMessages.resx)
![{EFEB6027-BC23-4103-B8A3-161C3A68B449}](https://github.com/user-attachments/assets/e5392d4a-1544-40d1-bd66-70a7304c4cf6)



