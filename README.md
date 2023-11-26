# Universe Repository

O objetivo deste projeto é aplicar na prática diversos conceitos de desenvolvimento web. O foco será a criação de uma plataforma temática que permitirá aos usuários elaborar playlists (chamadas de repositórios) de conteúdos específicos, simplificando a busca por recursos relevantes para estudos futuros.

Atualmente, a internet é vasta em conteúdos, abrangendo praticamente todos os tópicos imagináveis. No entanto, essa abundância de informações muitas vezes resulta em um cenário caótico e desorganizado. A pesquisa e descoberta de conhecimento tornaram-se tarefas árduas. Nesse contexto, o propósito deste projeto é atuar como uma forma de curadoria. Usuários com expertise consolidada em um determinado assunto podem criar repositórios contendo conteúdos valiosos e previamente validados por eles. Essa abordagem visa simplificar o processo de aprendizagem para outros usuários interessados no mesmo tema, proporcionando uma fonte confiável e organizada de recursos.

Os aspectos centrais dessa temática envolvem:
- Centralização de links de conteúdos selecionados em um repositório
- Possibilidade de acesso e compartilhamento do repositório


## Aspectos Técnicos

Técnologias utilizadas no projeto:
- .NET 6
- Entity Framework Core
- SQL Server

O projeto compreende uma API desenvolvida em .NET, fazendo uso do Entity Framework Core como ORM para efetuar a recuperação e persistência de dados no banco.

A API possui os endpoints descritos abaixo, para mais detalhes da documentação sobre cada endpoint basta consultadar a página do Swagger da aplicação acessando o caminho `/swagger` no seu browser após executar o projeto.

- `GET/repository`
- `GET/repository/{repositoryId}`
- `GET/repository/{repositoryName}`
- `GET/repository`
- `POST/repository`
- `PUT/repository`
- `DELETE/repository`
- `PUT/repository/content`
- `POST/login`
- `GET/user`
- `GET/user/{userId}`
- `POST/user`
- `PUT/user`

A API incorpora um fluxo de autenticação necessário para acessar a maioria dos endpoints. Para obter um token de acesso, basta realizar uma requisição ao endpoint `POST/login` com o seu e-mail e senha, conforme o exemplo em JSON abaixo:

```json
{
  "email": "string",
  "password": "string"
}
```

Para mais detalhes sobre os objetos de entrada e saída dos endpoints, bem como as opções de status code de retorno, consulte a página do Swagger da aplicação.

## Funcionamento

Para executar o projeto, as seguintes dependências são necessárias:
- .NET 6
- SQL Server
- Entity Framework Tools

Satisfazendo as dependências, basta trocar as credencias de acesso do banco de dados no arquivo `appsettings.json`, em seguida execute os seguintes comandos:
- `dotnet restore`
- `dotnet build`
- `dotnet ef database update`
- `dotnet run`